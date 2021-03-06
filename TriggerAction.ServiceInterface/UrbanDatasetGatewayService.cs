using org.matheval; // https://github.com/matheval/expression-evaluator-c-sharp/blob/main/LICENSE
using ServiceStack;
using ServiceStack.Logging;
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
    public class UrbanDatasetGatewayService : Service
    {
        public ILog log = LogManager.GetLogger(typeof(UrbanDatasetGatewayService));
        /*
         * Validiamo il parametro "ResourceId" sulla base del formato definito nelle specifiche SCPS Collaboration 2.0
         * https://smartcityplatform.enea.it/#/it/specification/collaboration/2.0/index.html#definizioneproduzione
         *
         * Consideriamo però che possa essere esteso con un suffisso contenente informazioni aggiuntive.
         * TODO: Non servono tutti i gruppi di cattura e la convalida potrebbe essere più restrittiva.
         */
        public static readonly string ResourceIdPattern =
            "^(?<ResourceId>(?<SmartCityId>SCP-[^_]+)" +
            "_(?<SolutionId>(?<NomeSolution>[^-]+)-[^_]+)" +
            "_(?<DatasetId>(?<NomeUrbanDataset>[^-]+)-(?<VersioneOntologia>[^_]+))" +
            "_(?<CollaborationStart>[0-9]{14}))" +
            "(?:_(?:[^_]*))?$"; // <- Suffisso fuori specifica SCPS Collaboration 2.0

        public object Get(TestRequest request)
        {
            return new TestResponse() { Code = "00", Message = "Succesful" };
        }

        public object Post(BasicRequest request)
        {
            string resourceId = request?.ResourceId;
            if (resourceId.IsNullOrEmpty())
                throw new ArgumentNullException("ResourceId");

            var match = Regex.Match(resourceId, ResourceIdPattern);
            if (!match.Success)
                throw new ArgumentException("ResourceId");

            var ResourceId = match.Groups["ResourceId"].Value;
            var SolutionId = match.Groups["SolutionId"].Value;
            var DatasetId = match.Groups["DatasetId"].Value;

            /*
             * Se il parametro "TimestampLessThan" è specificato e non è
             * ambiguo lo utilizziamo anche come timestamp di generazione del
             * dataset. Se non è valido o è ambiguo solleviamo un'eccezione.
             */
            DateTime timestampLessThan;
            DateTimeOffset timestamp;

            if (request.TimestampLessThan.HasValue)
            {
                timestampLessThan = DateTime.SpecifyKind(request.TimestampLessThan.Value, DateTimeKind.Local);
                if (TimeZoneInfo.Local.IsInvalidTime(timestampLessThan))
                {
                    throw new ArgumentException("TimeZoneInfo.Local.IsInvalidTime", "TimestampLessThan");
                }
                if (TimeZoneInfo.Local.IsAmbiguousTime(timestampLessThan))
                {
                    throw new ArgumentException("TimeZoneInfo.Local.IsAmbiguousTime", "TimestampLessThan");
                }
                timestamp = new DateTimeOffset(timestampLessThan);
            }
            else
            {
                timestamp = DateTimeOffset.Now;
                timestampLessThan = timestamp.DateTime;
            }

            var offset = timestamp.Offset;
            var timezoneCode = "UTC"; // http://smartcityplatform.enea.it/SCPSWebLibrary/property?name=timezone
            if (offset != TimeSpan.Zero)
            {
                var hours = Math.Abs(offset.Hours).ToString(CultureInfo.InvariantCulture);
                var minutes = Math.Abs(offset.Minutes).ToString(CultureInfo.InvariantCulture);
                timezoneCode += (offset < TimeSpan.Zero ? "-" : "+") + hours;
                if (minutes != "0")
                {
                    timezoneCode += ":" + (minutes.Length == 1 ? "0" + minutes : minutes);
                }
            }

            var templateBaseDir = Path.Combine("~/SCPSWebLibrary/template".MapAbsolutePath());
            string fileName = DatasetId + "-Template.json";
            var filePath = Path.Combine(templateBaseDir, fileName);

            var template = JsonSerializer.DeserializeFromStream<Template>(File.OpenRead(filePath));

            template.UrbanDataset.Context.Producer.Id = SolutionId;
            template.UrbanDataset.Context.Producer.SchemeId = null; // TODO: Verificare il significato di "SchemeId".

            template.UrbanDataset.Context.Coordinates.Format = "WGS84-DD";
            template.UrbanDataset.Context.Coordinates.Height = 4; // Coordinate del centro di Ravenna.
            template.UrbanDataset.Context.Coordinates.Latitude = 44.417778;
            template.UrbanDataset.Context.Coordinates.Longitude = 12.199444;

            // Preserviamo il modello della riga, quindi vuotiamo la lista.
            var lineTemplate = template.UrbanDataset.Values.Line.FirstOrDefault();
            template.UrbanDataset.Values.Line.Clear();

            /*
             * N.B. Ha senso utilizzare BatchOperationTypeStartsWith nella query seguente anche perché resourceId è
             * stato convalidato in modo abbastanza stringente all'inizio di questa funzione. Tipicamente
             * BatchOperationType conterrà o un resourceId puro e semplice, oppure un resourceId seguito da ulteriori
             * istruzioni (per esempio un'espressione da valutare per correggere il valore prima dell'invio).
             */
            object q = HostContext.ServiceController.Execute(new DeviceValueQuery { BatchOperationTypeStartsWith = resourceId });
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
                    q = HostContext.ServiceController.Execute(new DeviceRequest { DeviceId = deviceId, TimestampLessThan = timestampLessThan });
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
                                dr.Location.Longitude.HasValue)
                            {
                                newLine.Coordinates = new Coordinates
                                {
                                    Format = "WGS84-DD",
                                    Latitude = dr.Location.Latitude.Value,
                                    Longitude = dr.Location.Longitude.Value,
                                    // TODO: Campo "Height". Per il momento replichiamo l'altitudine del contesto.
                                    Height = template.UrbanDataset.Context.Coordinates.Height,
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
                                var valueToSend = item.Value; // Default

                                // Consideriamo la possibilità che BatchOperationType oltre ad iniziare con il
                                // ResourceId contenga un'espressione da applicare al valore prima dell'invio.
                                var formular = item.BatchOperationType.SafeSubstring(resourceId.Length).TrimStart(' ', '|');
                                if (!formular.IsNullOrEmpty())
                                {
                                    if (decimal.TryParse(item.Value, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal val))
                                    {
                                        var expr = new Expression(formular);
                                        var errors = expr.GetError();
                                        if (errors.Count == 0)
                                        {
                                            try
                                            {
                                                valueToSend = expr.Bind("val", val).Eval<decimal>().ToString(CultureInfo.InvariantCulture);
                                            }
                                            catch (Exception ex)
                                            {
                                                log.Error(ex, "{0}|{1}", resourceId, formular);
                                            }
                                        }
                                        else
                                        {
                                            foreach (var error in errors)
                                            {
                                                log.WarnFormat("{0}|{1}|{2}", error, resourceId, formular);
                                            }
                                        }
                                    }
                                }

                                propertyNames.AddIfNotExists(item.Label);
                                newLine.Property.Add(new ServiceModel.Types.Property { Name = item.Label, Val = valueToSend });
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

            template.UrbanDataset.Context.Timestamp = timestamp.DateTime;
            template.UrbanDataset.Context.TimeZone = timezoneCode;

            // TODO: Al momento in assenza di dati restituiamo il template vuoto, in futuro meglio dare una risposta più pertinente.
            return new BasicResponse { Code = "03", Message = "Request-Response Successful", Dataset = new List<Template> { template } };
        }
    }
}
