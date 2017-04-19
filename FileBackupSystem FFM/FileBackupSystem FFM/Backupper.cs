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
        List<string> modifiedFilePaths;

        //Properties

        //Constructor
        public Backupper(BackupType backupType, System.Collections.IList sourceDirs, string destDir, ref string curatedBackup)
        {
            sourceDirPaths = new string[sourceDirs.Count];
            modifiedFilePaths = new List<string>();
            sourceDirs.CopyTo(sourceDirPaths, 0);
            if (backupType == BackupType.Automatic)
            {
                FindChanges(sourceDirPaths, destDir, curatedBackup);
                UpdateBackup();
            }
            if (backupType == BackupType.Manual)
            {
                MakeBackup(sourceDirPaths, destDir, ref curatedBackup);
                UpdateBackup();
            }
        }

        //Methods
        public void FindChanges(string[] sourceDirs, string destDir, string curatedBackup)
        {
            Microsoft.VisualBasic.Devices.Computer curator = new Microsoft.VisualBasic.Devices.Computer();
            DateTime lastWriteOfFile;
            string currentBackupDir;
            string[] dirSplit;
            List<string> baseDirBits;
            List<string> sourceSubDirAndDir = new List<string>();
            sourceSubDirAndDir = sourceDirs.ToList();

            foreach (string directory in sourceDirs)
            {
                foreach (var subDir in curator.FileSystem.GetDirectories(directory, Microsoft.VisualBasic.FileIO.SearchOption.SearchAllSubDirectories))
                {
                    sourceSubDirAndDir.Add(subDir);
                }
            }

            foreach (string directory in sourceSubDirAndDir)
            {
                dirSplit = directory.Split('\\');
                currentBackupDir = $"{curatedBackup}";
                foreach (string pathBit in dirSplit)
                {
                    foreach (string baseDir in sourceDirs)
                    {
                        baseDirBits = baseDir.Split('\\').ToList();
                        baseDirBits.RemoveAt(baseDirBits.Count - 1);
                        if (!baseDirBits.Contains(pathBit))
                        {
                            currentBackupDir += $"\\{pathBit}";
                        }
                    }
                }
                foreach (string file in System.IO.Directory.GetFiles(directory))
                {
                    currentBackupDir += $"\\{file.Split('\\').Last()}";
                    lastWriteOfFile = System.IO.Directory.GetLastWriteTime(file);
                    if (lastWriteOfFile > System.IO.File.GetLastWriteTime($"{currentBackupDir}"))
                    {
                        modifiedFilePaths.Add(file);
#if DEBUG
                        System.Windows.MessageBox.Show($"Found a file! {file}\nLast write: {lastWriteOfFile}\nBackupped: {currentBackupDir}\nLast write: {System.IO.Directory.GetLastWriteTime(currentBackupDir)}", "Test", System.Windows.MessageBoxButton.OK);
#endif
                    }
                }
            }
#if DEBUG
            System.Windows.MessageBox.Show($"Search complete. Found {modifiedFilePaths.Count} modified files.");
#endif
        }
        public void MakeBackup(string[] sourceDirs, string destDir, ref string curatedBackup)
        {
            string tempestDir;
            destDir += $"\\{DateTime.Now.ToOADate()}";
            Microsoft.VisualBasic.Devices.Computer directoryBackupper = new Microsoft.VisualBasic.Devices.Computer();

            foreach (string directory in sourceDirs)
            {
                tempestDir = $"{destDir}\\{directory.Split('\\').Last()}";
                System.IO.Directory.CreateDirectory(tempestDir);
                directoryBackupper.FileSystem.CopyDirectory(directory, tempestDir);
            }

            curatedBackup = destDir;

            //Old code for copying files from sourceDirs
            //Cannot copy subdirectories from non-zipped files
            /*foreach (string directory in sourceDirs)
            {
                foreach (string file in System.IO.Directory.GetFiles(directory.ToString()))
                {
                    string[] tempDirPath;
                    tempDirPath = file.Split('\\');
                    System.IO.Directory.CreateDirectory($"{destDir}\\{tempDirPath[tempDirPath.Length - 2]}");
                    System.IO.File.Copy($"{file}", $"{destDir}\\{tempDirPath[tempDirPath.Length - 2]}\\{tempDirPath.Last()}");
                }
            }*/
        }
        public void UpdateBackup()
        {

        }
    }
}
