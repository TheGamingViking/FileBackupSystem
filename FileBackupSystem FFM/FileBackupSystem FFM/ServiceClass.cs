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
        protected Timer timer = new Timer(300000);

        //Properties

        //Constructors
        public ServiceClass()
        {
            CanPauseAndContinue = false;
            CanShutdown = true;
            ServiceName = "File_Backup_Service";
        }

        //Methods
        protected override void OnStart(string[] args)
        {
            timer.Enabled = true;
            timer.AutoReset = true;
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
        }
        protected override void OnStop()
        {
            timer.Enabled = false;
        }
        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            //Zug Zug
        }
    }
}
