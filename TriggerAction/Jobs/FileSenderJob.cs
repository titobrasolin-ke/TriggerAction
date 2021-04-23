using Quartz;
using Quartz.Util;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.Text;
using SJP.Sherlock;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Topshelf.Logging;
using TriggerAction.ServiceInterface;
using TriggerAction.ServiceModel;

namespace TriggerAction.Jobs
{
    [DisallowConcurrentExecution]
    public class FileSenderJob : IJob
    {
        private static readonly LogWriter Log = HostLogger.Get(typeof(FileSenderJob));
        private JsonServiceClient _client;
        public JsonServiceClient Client
        {
            get
            {
                if (_client == null)
                {
                    _client = new JsonServiceClient(AppSettings.GetRequiredString("scps.baseUri"));
                }
                return _client;
            }
        }
        public IAppSettings AppSettings { get; set; }
        public DeviceService Services { get; set; }

        public Task Execute(IJobExecutionContext context)
        {
            var username = AppSettings.GetRequiredString("scps.username");
            var password = AppSettings.GetRequiredString("scps.password");

            var directoryInfo = Directory.CreateDirectory("~/test".MapAbsolutePath());
            var files = Directory.GetFiles(directoryInfo.FullName, "*.json", SearchOption.TopDirectoryOnly);

            foreach (var fileInfo in files.Select(fileName => new FileInfo(fileName)))
            {
                try
                {
                    var processedFilesDirectory = fileInfo.Directory.CreateSubdirectory("processed");
                    if (fileInfo.IsFileLocked())
                    {
                        continue;
                    }

                    if (Client.BearerToken.IsNullOrEmpty())
                    {
                        try
                        {
                            var loginResponse = Client.Post(new LoginRequest { Username = username, Password = password });
                            if (loginResponse.Code == "01" &&
                                loginResponse.Message == "Authentication Successful" &&
                                !loginResponse.Token.IsNullOrWhiteSpace())
                            {
                                Client.BearerToken = loginResponse.Token;
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex);
                        }
                    }
                    else
                    {
                        try
                        {
                            var isAliveResponse = Client.PostBody(new IsAliveRequest { }, "");
                            if (isAliveResponse.Code != "00" ||
                                isAliveResponse.Message != "Successful" ||
                                isAliveResponse.Username != username)
                            {
                                Client.BearerToken = "";
                                var loginResponse = Client.Post(new LoginRequest { Username = username, Password = password });
                                if (loginResponse.Code == "01" &&
                                    loginResponse.Message == "Authentication Successful" &&
                                    !loginResponse.Token.IsNullOrWhiteSpace())
                                {
                                    Client.BearerToken = loginResponse.Token;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex);
                        }
                    }

                    if (!Client.BearerToken.IsNullOrEmpty())
                    {
                        var pushResp = Client.Post<PushResponse>("/push", File.ReadAllText(fileInfo.FullName));
                        var moveTo = fileInfo.Directory.CreateSubdirectory(Path.Combine("processed", pushResp.Code));

                        File.Move(fileInfo.FullName, Path.Combine(moveTo.FullName, fileInfo.Name));
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
            }
            return context.AsTaskResult();
        }
        class OutgoingFile
        {
            private bool hasPushRequest = false;
            private PushRequest pushRequest;

            private PushResponse pushResponse;

            public OutgoingFile(string txPath)
            {
                TxPath = txPath;
            }

            private PushResponse PushResponse { get => pushResponse; set => pushResponse = value; }
            private PushRequest PushRequest
            {
                get
                {
                    if (!hasPushRequest)
                    {
                        try
                        {
                            pushRequest = JsonSerializer.DeserializeFromStream<PushRequest>(File.OpenRead(TxPath));
                        }
                        finally
                        {
                            hasPushRequest = true;
                        }
                    }
                    return pushRequest;
                }
            }

            public string TxPath { get; }
            public string TargetPath { get; }
            public string ResourceId { get => PushRequest?.ResourceId; }
        }
    }
}
