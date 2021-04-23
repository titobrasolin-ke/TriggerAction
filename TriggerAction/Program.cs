using ServiceStack;
using ServiceStack.Text;
using SimpleHelper;
using System;
using Topshelf;
using Topshelf.Logging;

namespace TriggerAction
{
    class Program
    {
        private static readonly LogWriter Log = HostLogger.Get(typeof(Program));
        private static readonly string urlBase = "http://localhost:8088/";

        static void Main(string[] args)
        {
            Log.InfoFormat("##########   Starting service '{0}', V '{1}'   ##########",
                AssemblyHelper.AssemblyTitle,
                AssemblyHelper.AssemblyVersion);

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
                x.RunAsLocalSystem();
                x.UseAssemblyInfoForServiceInfo();
            });

            Log.InfoFormat("##########   Stoppping service '{0}', V '{1}'   ##########",
                AssemblyHelper.AssemblyTitle,
                AssemblyHelper.AssemblyVersion);

            var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());
            Environment.ExitCode = exitCode;

        }
    }
}
