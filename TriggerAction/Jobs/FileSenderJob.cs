﻿using Quartz;
using Quartz.Util;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.Text;
using SJP.Sherlock;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Topshelf.Logging;
using TriggerAction.Config;
using TriggerAction.ServiceInterface;
using TriggerAction.ServiceModel;
using TriggerAction.ServiceModel.Types;
using TriggerAction.Utilities;

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
                    _client = new JsonServiceClient(_baseUri);
                }
                return _client;
            }
        }

        private string _baseUri;

        public IAppSettings AppSettings { get; set; }
        public DeviceService Services { get; set; }

        public Task Execute(IJobExecutionContext context)
        {
            var settings = (TriggerActionSettings)ConfigurationManager.GetSection("TriggerActionSettings");
            var envSettings = settings.Environments.First(x => x.Name == settings.DefaultEnvironment);

            _baseUri = envSettings.WebServiceURL;

            var username = "";
            var password = "";

            var outgoingFolder = AppSettings.GetRequiredString("scps.outgoingFolder");

            var path = FileSystemHelper.IsFullPath(outgoingFolder) ?
                outgoingFolder : Path.Combine(Constants.ApplicationDataFolder, outgoingFolder);

            // Crea tutte le directory e le sottodirectory nel percorso specificato a meno che non esistano già.
            var directoryInfo = Directory.CreateDirectory(path);
            var files = Directory.GetFiles(directoryInfo.FullName, "*.json", SearchOption.TopDirectoryOnly);

            var ResourceIds = new List<string>{ }; // Considereremo al massimo un file per ciascun "resource_id".

            foreach (var fileInfo in files.Select(fileName => new FileInfo(fileName)))
            {
                try
                {
                    var processedFilesDirectory = fileInfo.Directory.CreateSubdirectory("processed");
                    if (fileInfo.IsFileLocked())
                    {
                        continue;
                    }

                    // Indirizziamo il file in base alla "collaboration" com'è definita nella relativa tabella.

                    PushRequest pr = null;
                    try
                    {
                        pr = fileInfo.ReadAllText().FromJson<PushRequest>();
                        var qr = HostContext.ServiceController.Execute(new QueryCollaboration { ResourceId = pr.ResourceId }) as QueryResponse<Collaboration>;
                        username = qr.Results.First().Username;
                        password = envSettings.Accounts.First(x => x.Username == username).Password;
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.Message, ex);
                        fileInfo.MoveTo(Path.ChangeExtension(fileInfo.FullName, "error"));
                        continue; // foreach
                    }

                    if (ResourceIds.Contains(pr.ResourceId))
                    {
                        continue; // Consideriamo al massimo un file per ciascun "resource_id".
                    }

                    ResourceIds.Add(pr.ResourceId);

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
                            Log.Error(ex.Message, ex);
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
                            Log.Error(ex.Message, ex);
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
