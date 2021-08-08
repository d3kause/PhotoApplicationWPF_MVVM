using PhotoAppWPF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PhotoAppWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (Directory.Exists(ConfigureParams.ImagesDirectory) == false)
            {
                Directory.CreateDirectory(ConfigureParams.ImagesDirectory);
            }
        }
    }
}
