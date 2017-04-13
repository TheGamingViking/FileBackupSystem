using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;
using System.Timers;

namespace FileBackupSystem_FFM
{
    class ServiceClass : ServiceBase
    {
        //Fields
        protected Timer timer = new Timer(30000);

        //Properties

        //Constructors
        public ServiceClass()
        {
            CanPauseAndContinue = true;
            CanShutdown = true;
            ServiceName = "File_Backup_Service";
        }

        //Methods
        protected override void OnStart(string[] args)
        {
            timer.Start();
            timer.AutoReset = true;
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
        }
        protected override void OnStop()
        {
            
        }
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            System.Windows.MessageBox.Show("Timer elapsed bitches!", "Elapsed timer", System.Windows.MessageBoxButton.OK);
        }
        static void Main()
        {
            System.ServiceProcess.ServiceBase.Run(new ServiceClass());
        }
    }
}
