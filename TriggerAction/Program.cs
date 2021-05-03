using ServiceStack;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Topshelf;
using Topshelf.HostConfigurators.AssemblyExtensions;
using Topshelf.Logging;

namespace TriggerAction
{
    internal static class Program
    {
        private static readonly LogWriter Log = HostLogger.Get(typeof(Program));
        private static readonly string urlBase = "http://localhost:8088/";

        static void Main(string[] args)
        {
            string appGuid = Assembly.GetExecutingAssembly().GetAttribute<GuidAttribute>().TryGetProperty(x => x.Value);
            string appTitle = Assembly.GetExecutingAssembly().GetAttribute<AssemblyTitleAttribute>().TryGetProperty(x => x.Title);
            string appVersion = Assembly.GetExecutingAssembly().GetAttribute<AssemblyVersionAttribute>().TryGetProperty(x => x.Version);

            Log.InfoFormat("{0} v{1} Console starting up", appTitle, appVersion);

            var rc = HostFactory.Run(x =>
            {
                x.UseNLog();

                x.Service<AppHost>(s =>
                {
                    s.ConstructUsing(name => new AppHost());
                    s.WhenStarted(ah =>
                    {
                        Log.InfoFormat("ServiceStack Self Host with Razor listening at {0}", urlBase);
                        ah.Init();
                        ah.Start(urlBase);
                    });
                    s.WhenStopped(ah =>
                    {
                        ah.Stop();
                    });
                });

                x.OnException(ex =>
                {
                    Log.Error(ex.Message, ex);
                });

                // Service Start mode
                x.StartAutomaticallyDelayed();

                // Service RunAs
                x.RunAsLocalSystem();

                // Service information
                x.UseAssemblyInfoForServiceInfo();

                /*
                 * EnableServiceRecovery Configuration
                 * http://appetere.com/post/topshelf-enableservicerecovery-configuration
                 */
                x.EnableServiceRecovery(src =>
                {
                    src.OnCrashOnly();
                    src.RestartService(delayInMinutes: 0);
                    src.RestartService(delayInMinutes: 1);
                    src.SetResetPeriod(days: 1);
                });
            });

            Log.InfoFormat("{0} v{1} Console shutting down", appTitle, appVersion);

            var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());
            Environment.ExitCode = exitCode;
        }
    }
}
