using System;
using System.IO;

namespace TriggerAction
{
    public static class Constants
    {
        public static readonly string ApplicationTitle;
        public static readonly string ApplicationDataFolder;

        static Constants()
        {
            ApplicationTitle = "TriggerAction";
            ApplicationDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), ApplicationTitle);
        }
    }
}
