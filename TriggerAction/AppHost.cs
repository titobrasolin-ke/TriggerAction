using Funq;
using Quartz;
using Quartz.Job;
using ServiceStack;
using ServiceStack.Admin;
using ServiceStack.Api.OpenApi;
using ServiceStack.Configuration;
using ServiceStack.Data;
using ServiceStack.Logging;
using ServiceStack.Logging.NLogger;
using ServiceStack.OrmLite;
using ServiceStack.Quartz;
using ServiceStack.Razor;
using ServiceStack.Text;
using ServiceStack.Text.Common;
using ServiceStack.Validation;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using TriggerAction.ServiceInterface;
using TriggerAction.ServiceModel.Types;

namespace TriggerAction
{
    public class AppHost : AppSelfHostBase
    {
        /// <summary>
        /// Base constructor requires a Name and Assembly where web service implementation is located
        /// </summary>
        public AppHost()
            : base("TriggerAction", typeof(DeviceService).Assembly)
        {
            LogManager.LogFactory = new NLogFactory();
        }

        /// <summary>
        /// Application specific configuration
        /// This method should initialize any IoC resources utilized by your web service classes.
        /// </summary>
        public override void Configure(Container container)
        {
            SetConfig(new HostConfig
            {
#if DEBUG
                DebugMode = true,
                WebHostPhysicalPath = Path.GetFullPath(Path.Combine("~".MapServerPath(), "..", "..")),
#else
                DebugMode = AppSettings.Get("DebugMode", Env.IsWindows),
#endif
                /*
                 * Consentiamo l'estensione "json" per poter rendere eventualmente
                 * accessibili via web gli UrbanDataset registrati su file system.
                 */
                AllowFileExtensions = { "json" },
            });

            AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(Config.WebHostPhysicalPath, "App_Data"));

            /*
             * Configuriamo il database SQL Server predefinito e registriamo
             * una connessione opzionale SQLite denominata "Local".
             */
            var defaultDbConn = AppSettings.GetConnectionString("DefaultConnection")
                ?? @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|aspnetdb.mdf;Integrated Security=True";

            var dbFactory = new OrmLiteConnectionFactory(defaultDbConn, SqlServer2012Dialect.Provider);
            dbFactory.RegisterConnection("Local", Path.Combine(Constants.ApplicationDataFolder, "db.sqlite"), SqliteDialect.Provider);
            container.Register<IDbConnectionFactory>(dbFactory);

            // Wrap all code in using statement to not forget about using db.Close()
            using (var db = dbFactory.Open("Local"))
            {
                db.CreateTableIfNotExists<PushResponseLog>();
                db.CreateTableIfNotExists<Collaboration>();
            }

            CsvConfig.ItemSeperatorString = CultureInfo.CurrentCulture.TextInfo.ListSeparator;

            JsConfig.SkipDateTimeConversion = true;
            JsConfig<DateTime>.SerializeFn = dateTime => dateTime.ToString(DateTimeSerializer.DateTimeFormatSecondsNoOffset);

            container.RegisterAs<FileScanJob, IJob>(); // TODO: Come registrare gli altri "IJob"?

            var quartzConfig = ConfigureQuartz();
            var quartzFeature = new QuartzFeature { Config = quartzConfig };

            Plugins.Add(quartzFeature);
            Plugins.Add(new RazorFormat());
            Plugins.Add(new AdminFeature());
            Plugins.Add(new OpenApiFeature());
            /*
             * I validatori (ad esempio "UpdateDeviceValidator") sono definiti
             * nell'assembly contenente le implementazioni dei servizi web.
             */
            Plugins.Add(new ValidationFeature());
            Plugins.Add(new AutoQueryFeature
            {
                IncludeTotal = true,
            });

            /*
             * Per utilizzare AutoQuery CRUD implementiamo una mappatura con
             * l'API Auto Mapping Populator incorporata di ServiceStack.
             */
            AutoMapping.RegisterPopulator((Dictionary<string, object> target, UpdateDevice source) =>
            {
                target[nameof(Device.Location)] = source.ConvertTo<LocationInfo>();
            });
        }

        private NameValueCollection ConfigureQuartz()
        {
            var properties =
                new NameValueCollection
                {
                    ["quartz.scheduler.instanceName"] = "ServerScheduler",
                    ["quartz.threadPool.type"] = "Quartz.Simpl.DefaultThreadPool, Quartz",
                    ["quartz.threadPool.threadCount"] = "5",
                    ["quartz.plugin.xml.type"] = "Quartz.Plugin.Xml.XMLSchedulingDataProcessorPlugin, Quartz.Plugins",
                    ["quartz.plugin.xml.fileNames"] = "~/Configuration/QuartzJobs.config",
                    ["quartz.plugin.xml.failOnFileNotFound"] = "true",
                    ["quartz.plugin.xml.scanInterval"] = "10",

                };
            return properties;
        }
    }
}
