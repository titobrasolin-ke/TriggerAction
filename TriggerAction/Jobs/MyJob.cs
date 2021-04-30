using Quartz;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Topshelf.Logging;
using TriggerAction.ServiceInterface;
using TriggerAction.ServiceModel;
using TriggerAction.ServiceModel.Types;

namespace TriggerAction.Jobs
{
    [DisallowConcurrentExecution]
    public class MyJob : IJob
    {
        private static readonly LogWriter Log = HostLogger.Get(typeof(MyJob));
        private static readonly string resourceIdPattern = @"^(?<ResourceId>(?<SmartCityId>SCP-[^_]+)_(?<SolutionId>(?<NomeSolution>[^-]+)-[^_]+)_(?<DatasetId>(?<NomeUrbanDataset>[^-]+)-(?<VersioneOntologia>[^_]+))_(?<CollaborationStart>[0-9]{14}))(?<ExtraInfo>.*)$";

        public IAppSettings AppSettings { get; set; }
        public DeviceService Services { get; set; }

        public Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;

            string[] BatchOperationTypeIn;
            try
            {
                BatchOperationTypeIn = dataMap.GetString("BatchOperationTypeIn")?.FromJsv<string[]>();
            }
            catch
            {
                BatchOperationTypeIn = null;
            }

            // N.B. I filtri di tipo ICollection come BatchOperationTypeIn se sono vuoti vengono ignorati:
            // https://github.com/ServiceStack/ServiceStack/blob/master/src/ServiceStack.Server/AutoQueryFeature.cs#L1080

            object q = HostContext.ServiceController.Execute(new DeviceValueQuery
            {
                BatchOperationTypeStartsWith = "SCP-",
                BatchOperationTypeIn = BatchOperationTypeIn,
            });
            if (q is QueryResponse<DeviceValue> qr)
            {
                var pushRequests = new Dictionary<string, PushRequest>();
                var deviceValues = qr.Results.SafeWhere(x => x.DeviceId.HasValue).OrderBy(x => x.DeviceId).ToList();

                foreach (var match in deviceValues.Select(x => Regex.Match(x.BatchOperationType, resourceIdPattern)).Where(x => x.Success))
                {
                    var batchOperationType = match.Value;
                    if (pushRequests.ContainsKey(batchOperationType))
                    {
                        continue; // Operazione batch già presente.
                    }
                    var ResourceId = match.Groups["ResourceId"].Value;
                    var SolutionId = match.Groups["SolutionId"].Value;
                    var DatasetId = match.Groups["DatasetId"].Value;

                    var templateBaseDir = Path.Combine("~/SCPSWebLibrary/template".MapAbsolutePath());
                    string fileName = DatasetId + "-Template.json";
                    var filePath = Path.Combine(templateBaseDir, fileName);
                    if (!File.Exists(filePath))
                    {
                        Log.Warn("File does not exist: " + filePath);
                        continue;
                    }
                    try
                    {
                        var dataset = JsonSerializer.DeserializeFromStream<Template>(File.OpenRead(filePath));

                        dataset.UrbanDataset.Context.Producer.Id = SolutionId;
                        dataset.UrbanDataset.Context.Producer.SchemeId = null;

                        dataset.UrbanDataset.Context.Coordinates.Format = "WGS84-DD";
                        dataset.UrbanDataset.Context.Coordinates.Height = 0;
                        dataset.UrbanDataset.Context.Coordinates.Latitude = 0;
                        dataset.UrbanDataset.Context.Coordinates.Longitude = 0;

                        dataset.UrbanDataset.Values.Line.Clear();
                        pushRequests.Add(batchOperationType, new PushRequest { ResourceId = ResourceId, Dataset = dataset });
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex);
                        continue;
                    }
                }

                foreach (var deviceId in deviceValues.Select(x => x.DeviceId).Distinct())
                {
                    q = Services.Any(new DeviceRequest { DeviceId = deviceId.Value });
                    if (q is DeviceResponse dr)
                    {
                        var batchOperationTypes = dr.Values.Select(x => x.BatchOperationType)
                            .Distinct().Where(key => pushRequests.ContainsKey(key));

                        var extraValues = dr.Values.Where(x => !pushRequests.ContainsKey(x.BatchOperationType));

                        foreach (var batchOperationType in batchOperationTypes)
                        {
                            Template t = pushRequests[batchOperationType].Dataset;
                            bool isWhatever = t.UrbanDataset.Specification.Id.Value.StartsWith("Whatever-");
                            int lineId = 1 + (t.UrbanDataset.Values.Line.Max(x => (int?)x.Id) ?? 0);

                            Line line = new Line { Id = lineId, Period = new ServiceModel.Types.Period { StartTs = dr.Period.StartTs.DateTime, EndTs = dr.Period.EndTs.DateTime }, Property = new List<ServiceModel.Types.Property>() };

                            foreach (var item in dr.Values.Where(x => x.BatchOperationType == batchOperationType))
                            {
                                if (isWhatever ||
                                    t.UrbanDataset.Specification.Properties.PropertyDefinition.Exists(x => x.PropertyName == item.Label && x.UnitOfMeasure == item.Unit))
                                {
                                    line.Property.Add(new ServiceModel.Types.Property { Name = item.Label, Val = item.Value.ToString(CultureInfo.InvariantCulture) });
                                }
                            }

                            foreach (var item in extraValues)
                            {
                                if (batchOperationType.StartsWith(item.BatchOperationType))
                                {
                                    if (isWhatever ||
                                        t.UrbanDataset.Specification.Properties.PropertyDefinition.Exists(x => x.PropertyName == item.Label && x.UnitOfMeasure == item.Unit))
                                    {
                                        line.Property.Add(new ServiceModel.Types.Property { Name = item.Label, Val = item.Value.ToString(CultureInfo.InvariantCulture) });
                                    }
                                }
                            }

                            t.UrbanDataset.Values.Line.Add(line);
                        }
                    }
                    else
                    {
                        Log.Error(q);
                    }
                }

                // Salviamo localmente i vari template.
                foreach (var item in pushRequests)
                {
                    // Prima del salvataggio aggiorniamo l'istante di generazione di questo UrbanDataset (ovvero il
                    // momento in cui i dati sono stati aggregati).

                    var pushRequest = item.Value;

                    pushRequest.Dataset.UrbanDataset.Context.Timestamp = DateTime.Now;
                    pushRequest.Dataset.UrbanDataset.Context.TimeZone = DateTimeOffset.Now.ToString("'UTC'zzz");

                    var path = Path.Combine("test", Guid.NewGuid().ToString() + ".json");
                    using (var stream = File.OpenWrite(path))
                    {

                        JsonSerializer.SerializeToStream(pushRequest, stream);
                    }
                }
            }
            else
            {
                Log.Error(q);
            }

            return context.AsTaskResult();
        }
    }
}
