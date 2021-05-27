using ServiceStack;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TriggerAction.ServiceModel;
using TriggerAction.ServiceModel.Types;

namespace TriggerAction.ServiceInterface
{
    class UrbanDatasetGatewayService : Service
    {
        // TODO: Non servono tutti i gruppi di cattura e la convalida dovrebbe essere più restrittiva.
        private static readonly string resourceIdPattern =
            @"^(?<ResourceId>(?<SmartCityId>SCP-[^_]+)_(?<SolutionId>(?<NomeSolution>[^-]+)-[^_]+)_(?<DatasetId>(?<NomeUrbanDataset>[^-]+)-(?<VersioneOntologia>[^_]+))_(?<CollaborationStart>[0-9]{14}))$";

        public object Get(TestRequest request)
        {
            return new TestResponse() { Code = "00", Message = "Succesful" };
        }

        public object Post(BasicRequest request)
        {
            string resourceId = request?.ResourceId;
            if (resourceId.IsNullOrEmpty())
                throw new ArgumentNullException("ResourceId");

            var match = Regex.Match(resourceId, resourceIdPattern);
            if (!match.Success)
                throw new ArgumentException("ResourceId");

            var ResourceId = match.Groups["ResourceId"].Value;
            var SolutionId = match.Groups["SolutionId"].Value;
            var DatasetId = match.Groups["DatasetId"].Value;

            var templateBaseDir = Path.Combine("~/SCPSWebLibrary/template".MapAbsolutePath());
            string fileName = DatasetId + "-Template.json";
            var filePath = Path.Combine(templateBaseDir, fileName);

            var template = JsonSerializer.DeserializeFromStream<Template>(File.OpenRead(filePath));

            template.UrbanDataset.Context.Producer.Id = SolutionId;
            template.UrbanDataset.Context.Producer.SchemeId = null; // TODO: Verificare il significato di "SchemeId".

            template.UrbanDataset.Context.Coordinates.Format = "WGS84-DD";
            // template.UrbanDataset.Context.Coordinates.Height = 0; // Facoltativo.
            template.UrbanDataset.Context.Coordinates.Latitude = 44.417778; // TODO: Quali sono le coordinate predefinite?
            template.UrbanDataset.Context.Coordinates.Longitude = 12.199444;

            // Preserviamo il modello della riga, quindi vuotiamo la lista.
            var lineTemplate = template.UrbanDataset.Values.Line.FirstOrDefault();
            template.UrbanDataset.Values.Line.Clear();

            object q = HostContext.ServiceController.Execute(new DeviceValueQuery { BatchOperationType = resourceId });
            if (!(q is QueryResponse<DeviceValue> qr))
            {
                return q;
            }
            else
            {
                bool isWhatever = template.UrbanDataset.Specification.Id.Value.StartsWith("Whatever-");

                var deviceIds = qr.Results.SafeWhere(x => x.DeviceId.HasValue).Select(x => x.DeviceId.Value).Distinct().OrderBy(x => x).ToList();
                int lineId = 0;

                // Definiamo una lista di proprietà che non andranno in nessun caso rimosse dalle "PropertyDefinition".
                var propertyNames = new List<string> { "period", "start_ts", "end_ts" };

                foreach (var deviceId in deviceIds)
                {
                    q = HostContext.ServiceController.Execute(new DeviceRequest { DeviceId = deviceId });
                    if (!(q is DeviceResponse dr))
                    {
                        return q;
                    }
                    else
                    {
                        lineId += 1;
                        Line newLine = new Line { Id = lineId, Property = new List<ServiceModel.Types.Property>() };

                        if (isWhatever ||
                            template.UrbanDataset.Specification.Properties.PropertyDefinition.Exists(x => x.PropertyName == "coordinates"))
                        {
                            if (dr.Location.Latitude.HasValue &&
                                dr.Location.Longitude.HasValue )
                            {
                                newLine.Coordinates = new Coordinates {
                                    Format = "WGS84-DD",
                                    Latitude = dr.Location.Latitude.Value,
                                    Longitude = dr.Location.Longitude.Value,
                                    Height = 0 // TODO: Campo "Height".
                                };
                                propertyNames.AddIfNotExists("coordinates");
                                propertyNames.AddIfNotExists("format");
                                propertyNames.AddIfNotExists("latitude");
                                propertyNames.AddIfNotExists("longitude");
                                propertyNames.AddIfNotExists("height");
                            }

                            /*
                             * TODO: Valutare la possibilità di utilizzare le coordinate del contesto come default.
                             */

                            //else
                            //{
                            //    newLine.Coordinates = new Coordinates {
                            //        Format = template.UrbanDataset.Context.Coordinates.Format,
                            //        Latitude = template.UrbanDataset.Context.Coordinates.Latitude,
                            //        Longitude = template.UrbanDataset.Context.Coordinates.Longitude
                            //    };
                            //}
                        }

                        if (isWhatever ||
                            template.UrbanDataset.Specification.Properties.PropertyDefinition.Exists(x => x.PropertyName == "period"))
                        {
                            newLine.Period = new ServiceModel.Types.Period { StartTs = dr.Period.StartTs.DateTime, EndTs = dr.Period.EndTs.DateTime };
                        }

                        foreach (var item in dr.Values.Where(x => x.BatchOperationType.StartsWith(resourceId) || resourceId.StartsWith(x.BatchOperationType)))
                        {
                            if (isWhatever ||
                                template.UrbanDataset.Specification.Properties.PropertyDefinition.Exists(x => x.PropertyName == item.Label && x.UnitOfMeasure == item.Unit))
                            {
                                propertyNames.AddIfNotExists(item.Label);
                                newLine.Property.Add(new ServiceModel.Types.Property { Name = item.Label, Val = item.Value.ToString(CultureInfo.InvariantCulture) });
                            }
                        }

                        template.UrbanDataset.Values.Line.Add(newLine);
                    }
                }

                if (!isWhatever)
                {
                    // Rimuoviamo dalle "PropertyDefinition" gli elementi non presenti nelle righe.
                    template.UrbanDataset.Specification.Properties.PropertyDefinition
                        = template.UrbanDataset.Specification.Properties.PropertyDefinition.Where(x => propertyNames.Contains(x.PropertyName)).ToList();
                }
            }

            template.UrbanDataset.Context.Timestamp = DateTime.Now;
            template.UrbanDataset.Context.TimeZone = DateTimeOffset.Now.ToString("'UTC'zzz");

            // TODO: Al momento in assenza di dati restituiamo il template vuoto, in futuro meglio dare una risposta più pertinente.
            return new BasicResponse { Code = "03", Message = "Request-Response Successful", Dataset = new List<Template> { template } };
        }
    }
}
