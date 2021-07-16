using Quartz;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Topshelf.Logging;
using TriggerAction.ServiceInterface;
using TriggerAction.ServiceModel;
using TriggerAction.Utilities;

namespace TriggerAction.Jobs
{
    [DisallowConcurrentExecution]
    public class UrbanDatasetProducerJob : IJob
    {
        private static readonly LogWriter Log = HostLogger.Get(typeof(UrbanDatasetProducerJob));
        public IAppSettings AppSettings { get; set; }

        public Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;

            List<string> batchOperationTypes;
            try
            {
                batchOperationTypes = dataMap.GetString("BatchOperationTypeIn")?.FromJsv<List<string>>()
                    .Select(x => x.Trim()).Distinct().ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                batchOperationTypes = new List<string> { };
            }

            foreach (var batchOperationType in batchOperationTypes)
            {
                var match = Regex.Match(batchOperationType, UrbanDatasetGatewayService.ResourceIdPattern);
                if (!match.Success)
                {
                    continue;
                }

                /*
                 * N.B. Come marca temporale per la generazione del dataset
                 * utilizzeremo l'istante di esecuzione pianificato per il job.
                 */
                object requestDto = new BasicRequest
                {
                    ResourceId = batchOperationType,
                    TimestampLessThan = context.ScheduledFireTimeUtc?.LocalDateTime
                };

                object v = HostContext.ServiceController.Execute(requestDto);
                if (v is BasicResponse resp && resp.Code == "03") // Request-Response Successful
                {
                    var outgoingFolder = AppSettings.GetRequiredString("scps.outgoingFolder");

                    var path1 = FileSystemHelper.IsFullPath(outgoingFolder) ?
                        outgoingFolder : Path.Combine(Constants.ApplicationDataFolder, outgoingFolder);

                    var resource_id = match.Groups["ResourceId"].Value;

                    foreach (var item in resp.Dataset)
                    {
                        var pushRequest = new PushRequest { ResourceId = resource_id, Dataset = item };
                        /*
                         * Costruiamo il nome del file premettendo al "resource_id" una marca temporale in formato
                         * ISO 8601 sulla falsariga della SCP-GUI. Dobbiamo tuttavia aggiungere un ulteriore marcatore
                         * poiché non è a priori escluso che più "flussi" contemporanei possano convergere verso la
                         * stessa risorsa.
                         */
                        var prefix = item.UrbanDataset.Context.Timestamp.ToString("yyyyMMddTHHmmss");
                        var ticks = DateTime.Now.Ticks;
                        var path = Path.Combine(path1, $"{prefix}_{batchOperationType}_{ticks:D20}.json");
                        using (var stream = File.OpenWrite(path))
                        {
                            JsonSerializer.SerializeToStream(pushRequest, stream);
                        }
                    }
                }
            }
            return context.AsTaskResult();
        }
    }
}
