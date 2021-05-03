using System;
using System.IO;
using System.Linq;

namespace TriggerAction.Utilities
{
    public static class FileSystemHelper
    {
        /// <summary>
        /// Checks whether the given path is a fully qualified absolute path
        /// https://stackoverflow.com/a/35046453/15813553
        /// </summary>
        /// <param name="path">Path to check</param>
        /// <returns><c>true</c> if path is fully qualified</returns>
        public static bool IsFullPath(string path)
        {
            return !string.IsNullOrWhiteSpace(path)
                && path.IndexOfAny(Path.GetInvalidPathChars().ToArray()) == -1
                && Path.IsPathRooted(path)
                && !Path.GetPathRoot(path).Equals(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal);
        }
    }
}
