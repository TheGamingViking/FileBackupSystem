using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration.Install;
using System.ComponentModel;
using System.ServiceProcess;
using System.Collections;

namespace FileBackupSystem_FFM
{
    [RunInstaller(true)]
    class BackupInstallerAsService : Installer
    {
        //Fields
        ServiceInstaller serviceInstaller;
        ServiceProcessInstaller processInstaller;

        //Properties

        //Constructor
        public BackupInstallerAsService()
        {
            serviceInstaller = new ServiceInstaller();
            processInstaller = new ServiceProcessInstaller();

            processInstaller.Account = ServiceAccount.LocalSystem;

            serviceInstaller.StartType = ServiceStartMode.Automatic;
            serviceInstaller.ServiceName = "File_Backup_Service";

            Installers.Add(serviceInstaller);
            Installers.Add(processInstaller);
        }

        //Methods
        protected override void OnBeforeInstall(IDictionary savedState)
        {
            base.OnBeforeInstall(savedState);
        }
        protected override void OnBeforeUninstall(IDictionary savedState)
        {
            base.OnBeforeUninstall(savedState);
        }
    }
}
