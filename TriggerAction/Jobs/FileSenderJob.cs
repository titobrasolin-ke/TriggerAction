using Quartz;
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
using System.Threading;
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
            var submissionDelay = AppSettings.Get("scps.submissionDelay", 0);

            var path = FileSystemHelper.IsFullPath(outgoingFolder) ?
                outgoingFolder : Path.Combine(Constants.ApplicationDataFolder, outgoingFolder);

            // Crea tutte le directory e le sottodirectory nel percorso specificato a meno che non esistano già.
            var directoryInfo = Directory.CreateDirectory(path);
            var files = Directory.GetFiles(directoryInfo.FullName, "*.json", SearchOption.TopDirectoryOnly);

            var ResourceIds = new List<string> { }; // Considereremo al massimo un file per ciascun "resource_id".

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

                    string jsonToPush = default;
                    PushRequest pr = null;

                    try
                    {
                        jsonToPush = fileInfo.ReadAllText();
                        pr = jsonToPush.FromJson<PushRequest>();
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

                    /*
                     * Prima di tentare il "push" consideriamo la possibilità che l'ultimo dataset inviato con
                     * succcesso al server (codice "02") differisca da quello corrente solo per la marca temporale. In
                     * questo caso tutto il "verticale" è probabilmente fermo ed omettiamo perciò l'invio del dataset,
                     * così che la situazione si rispecchi negli "Interoperability Indicators" della SCP-GUI.
                     *
                     * Fidandoci della marca temporale presente nel nome del file, supponiamo che il dataset più
                     * recente sia l'ultimo in ordine alfabetico.
                     */

                    string lastSuccessfulFile;
                    var pushSuccesfulFolderPath = Path.Combine(fileInfo.Directory.FullName, "processed", "02");

                    try
                    {
                        lastSuccessfulFile = Directory.GetFiles(pushSuccesfulFolderPath, $"*_{pr.ResourceId}*.json", SearchOption.TopDirectoryOnly)
                            .OrderBy(f => f).LastOrDefault();
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.Message, ex);
                        lastSuccessfulFile = default;
                    }

                    if (!string.IsNullOrEmpty(lastSuccessfulFile))
                    {
                        try
                        {
                            var history = File.ReadAllText(lastSuccessfulFile).FromJson<PushRequest>();
                            history.Dataset.UrbanDataset.Context.Timestamp = pr.Dataset.UrbanDataset.Context.Timestamp;
                            history.Dataset.UrbanDataset.Context.TimeZone = pr.Dataset.UrbanDataset.Context.TimeZone;
                            if (JsonSerializer.SerializeToString(history) == jsonToPush)
                            {
                                Log.InfoFormat("{0} data already sent, skipping.", pr.ResourceId);
                                File.Move(fileInfo.FullName, Path.Combine(fileInfo.Directory.CreateSubdirectory("skipped").FullName, fileInfo.Name));
                                continue; // foreach
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex.Message, ex);
                        }
                    }

                    /*
                     * Se il file non è stato saltato procediamo con il tentativo di invio alla SCP, dopo aver
                     * eventualmente atteso qualche secondo per evitare di sovraccaricare il server.
                     */
                    if (submissionDelay > 0) Thread.Sleep(new TimeSpan(0, 0, submissionDelay < 60 ? submissionDelay : 60));
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
                        // NOTA: Tecnicamente non sarebbe necessario limitare il numero di esecuzioni del ciclo "do",
                        // preferisco tuttavia evitare ogni possibilità di un loop senza uscita causato da eventuali
                        // comportamenti "anomali" del server che al momento potrei non essere in grado di prevedere.

                        int maxRetries = 2;
                        string code;
                        do
                        {
                            var pushResp = Client.Post<PushResponse>("/push", jsonToPush);

                            // In data 9 luglio 2021 gli UrbanDataset esistenti *non* vengono aggiornati, perciò se il
                            // server ha risposto "UrbanDataset already exists" lo cancelliamo dalla SCP. Se l'operazione
                            // ha successo *non* spostiamo il file tra quelli elaborati, ritenteremo poi l'invio.

                            if (pushResp.Detail == "UrbanDataset already exists")
                            {
                                var deleteResp = Client.Post(new DeleteRequest { ResourceId = pr.ResourceId, Timestamp = pr.Dataset.UrbanDataset.Context.Timestamp });
                                code = deleteResp.Code;
                            }
                            else
                            {
                                code = pushResp.Code;
                            }

                            if (code != "08") // "08" => "Delete Successful"
                            {
                                var moveTo = fileInfo.Directory.CreateSubdirectory(Path.Combine("processed", pushResp.Code));
                                File.Move(fileInfo.FullName, Path.Combine(moveTo.FullName, fileInfo.Name));
                            }
                        } while (code == "08" && --maxRetries > 0);
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
