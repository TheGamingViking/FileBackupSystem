﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;
using System.Timers;

namespace FileBackupSystem_FFM
{
    //Code not intergrated. From experiment with services.
    class ServiceClass : ServiceBase
    {
        //Fields
        protected Timer timer = new Timer(30000);

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
            System.Windows.MessageBox.Show("Timer elapsed!", "Elapsed timer", System.Windows.MessageBoxButton.OK);
        }
    }
}
