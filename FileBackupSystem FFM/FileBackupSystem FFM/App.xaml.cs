using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.ServiceProcess;

namespace FileBackupSystem_FFM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            string[] commandLineArgs = Environment.GetCommandLineArgs();

            if (commandLineArgs.Length > 1 && commandLineArgs[1].Equals("-service"))
            {
                ServiceBase.Run(new ServiceClass());
            }
            else
            {
                MainWindow main = new FileBackupSystem_FFM.MainWindow();
                main.ShowDialog();
                this.Shutdown();
            }
        }
    }
}
