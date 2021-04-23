﻿using Funq;
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
using System;
using System.Collections.Specialized;
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
                AllowFileExtensions = { "json" },
            });

            // We'll configure our Database to use a default SQL Server database and register an optional named
            // connection looking at a “Reporting” SQLite database.

            var defaultDbConn = AppSettings.GetConnectionString("DefaultConnection")
                ?? @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|aspnetdb.mdf;Integrated Security=True";

            var dbFactory = new OrmLiteConnectionFactory(defaultDbConn, SqlServer2012Dialect.Provider);
            dbFactory.RegisterConnection("Reporting", "~/App_Data/reporting.sqlite".MapAbsolutePath(), SqliteDialect.Provider);
            container.Register<IDbConnectionFactory>(dbFactory);

            using (var db = dbFactory.Open("Reporting"))
            {
                db.CreateTableIfNotExists<PushResponseLog>();
            }

            JsConfig.SkipDateTimeConversion = true;
            JsConfig<DateTime>.SerializeFn = dateTime => dateTime.ToString(DateTimeSerializer.DateTimeFormatSecondsNoOffset);

            container.RegisterAs<FileScanJob, IJob>(); // TODO: Come registrare gli altri "IJob"?

            var quartzConfig = ConfigureQuartz();
            var quartzFeature = new QuartzFeature { Config = quartzConfig };

            Plugins.Add(quartzFeature);
            Plugins.Add(new RazorFormat());
            Plugins.Add(new AdminFeature());
            Plugins.Add(new OpenApiFeature());
            Plugins.Add(new AutoQueryFeature
            {
                IncludeTotal = true,
            });
        }

        private NameValueCollection ConfigureQuartz()
        {
            var properties =
                new NameValueCollection
                {
                    ["quartz.scheduler.instanceName"] = "ServerScheduler",
                    ["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz",
                    ["quartz.threadPool.threadCount"] = "5",
                    ["quartz.threadPool.threadPriority"] = "Normal",
                    ["quartz.plugin.xml.type"] = "Quartz.Plugin.Xml.XMLSchedulingDataProcessorPlugin, Quartz.Plugins",
                    ["quartz.plugin.xml.fileNames"] = "~/Configuration/QuartzJobs.config",
                    ["quartz.plugin.xml.failOnFileNotFound"] = "true",
                    ["quartz.plugin.xml.scanInterval"] = "10",

                };
            return properties;
        }
    }
}