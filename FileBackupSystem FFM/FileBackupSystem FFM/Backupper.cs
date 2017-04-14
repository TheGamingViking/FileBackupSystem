using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FileBackupSystem_FFM
{
    enum BackupType
    {
        Manual,
        Automatic
    }

    class Backupper
    {
        //Fields
        string[] sourceDirPaths;

        //Properties

        //Constructor
        public Backupper(BackupType backupType, System.Collections.IList sourceDirs, string destDir)
        {
            sourceDirPaths = new string[sourceDirs.Count];
            //sourceDirs.ToString().CopyTo(0, sourceDirPaths, 0, sourceDirs.Count);
            sourceDirs.CopyTo(sourceDirPaths, 0);
            if (backupType == BackupType.Automatic)
            {
                FindChanges();
                UpdateBackup();
            }
            if (backupType == BackupType.Manual)
            {
                MakeBackup(sourceDirPaths, destDir);
                UpdateBackup();
            }
        }

        //Methods
        public void FindChanges()
        {

        }
        public void MakeBackup(string[] sourceDirs, string destDir)
        {
            destDir += $"\\{DateTime.Now.ToOADate()}";
            System.IO.Directory.CreateDirectory(destDir);
            foreach (string directory in sourceDirs)
            {
                foreach (string file in System.IO.Directory.GetFiles(directory.ToString()))
                {
                    System.IO.File.Copy($"{file}", $"{destDir}\\{file.Split('\\').Last()}");
                }
            }
        }
        public void UpdateBackup()
        {

        }
    }
}
