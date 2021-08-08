using System.Diagnostics;
using System.IO;

namespace PhotoAppWPF.Infrastructure
{
    /// <summary>
    /// Global project variables
    /// </summary>
    internal static class ConfigureParams
    {
        /// <summary>
        /// Full path to directory for save\read photo
        /// </summary>
        /// <value>
        /// Full path to directory for save\read photo
        /// </value>
        public static string ImagesDirectory => Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\Images\";

    }
}
