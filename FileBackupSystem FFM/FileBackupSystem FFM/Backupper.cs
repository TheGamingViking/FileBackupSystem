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
            foreach (string directory in sourceDirs)
            {
                foreach (string file in System.IO.Directory.GetFiles(directory.ToString()))
                {
                    string[] tempDirPath;
                    tempDirPath = file.Split('\\');
                    System.IO.Directory.CreateDirectory($"{destDir}\\{tempDirPath[tempDirPath.Length - 2]}");
                    System.IO.File.Copy($"{file}", $"{destDir}\\{tempDirPath[tempDirPath.Length - 2]}\\{tempDirPath.Last()}");
                }
            }
            destDir.Remove(2);
        }
        public void UpdateBackup()
        {

        }
    }
}
