using Quartz;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Topshelf.Logging;
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

            List<string> resourceIds;
            try
            {
                resourceIds = dataMap.GetString("BatchOperationTypeIn")?.FromJsv<List<string>>()
                    .Select(x => x.Trim()).Distinct().ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                resourceIds =  new List<string> { };
            }

            foreach (var id in resourceIds)
            {
                object v = HostContext.ServiceController.Execute(new BasicRequest { ResourceId = id });
                if (v is BasicResponse resp && resp.Code == "03") // Request-Response Successful
                {
                    var outgoingFolder = AppSettings.GetRequiredString("scps.outgoingFolder");

                    var path1 = FileSystemHelper.IsFullPath(outgoingFolder) ?
                        outgoingFolder : Path.Combine(Constants.ApplicationDataFolder, outgoingFolder);

                    foreach (var item in resp.Dataset)
                    {
                        var pushRequest = new PushRequest { ResourceId = id, Dataset = item };
                        var path = Path.Combine(path1, Guid.NewGuid().ToString() + ".json");
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
