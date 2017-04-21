using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Data.SQLite;

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
        List<string> backupFilesToUpdate;
        //Database fields
        SQLiteConnection connection;
        string command;
        SQLiteCommand commander;

        //Properties

        //Constructor
        public Backupper(BackupType backupType, System.Collections.IList sourceDirs, string destDir, ref string curatedBackup, SQLiteConnection connection)
        {
            this.connection = connection;
            sourceDirPaths = new string[sourceDirs.Count];
            sourceDirs.CopyTo(sourceDirPaths, 0);
            modifiedFilePaths = new List<string>();
            backupFilesToUpdate = new List<string>();
            if (backupType == BackupType.Automatic)
            {
                FindChanges(sourceDirPaths, destDir, curatedBackup);
                UpdateBackup();
            }
            if (backupType == BackupType.Manual)
            {
                MakeBackup(sourceDirPaths, destDir, ref curatedBackup);
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
                    try
                    {
                        lastWriteOfFile = System.IO.File.GetLastWriteTime(file);
                        if (lastWriteOfFile > System.IO.File.GetLastWriteTime($"{currentBackupDir}"))
                        {
                            modifiedFilePaths.Add(file);
                            backupFilesToUpdate.Add(currentBackupDir);
#if DEBUG
                            System.Windows.MessageBox.Show($"Found a file! {file}\nLast write: {lastWriteOfFile}\nBackupped: {currentBackupDir}\nLast write: {System.IO.Directory.GetLastWriteTime(currentBackupDir)}", "Test", System.Windows.MessageBoxButton.OK);
#endif
                        }
                    }
                    catch (System.IO.PathTooLongException)
                    {
                        System.Windows.MessageBox.Show("The specified path was too long, automatic back-up could not be completed.", "Error Encountered", System.Windows.MessageBoxButton.OK);
                    }
                }
            }
#if DEBUG
            System.Windows.MessageBox.Show($"Search complete. Found {modifiedFilePaths.Count} modified files.");
#endif
        }
        public void MakeBackup(string[] sourceDirs, string destDir, ref string curatedBackup)
        {
            bool exceptionEncountered = false;
            string tempestDir = "";
            destDir += $"\\{DateTime.Now.ToOADate()}";
            Microsoft.VisualBasic.Devices.Computer directoryBackupper = new Microsoft.VisualBasic.Devices.Computer();
            //Creates the static manual backup/restore point
            try
            {
                foreach (string directory in sourceDirs)
                {
                    tempestDir = $"{destDir}\\{directory.Split('\\').Last()}";
                    System.IO.Directory.CreateDirectory(tempestDir);
                    directoryBackupper.FileSystem.CopyDirectory(directory, tempestDir);
                }
                command = $"insert into BackupDirectories values('{destDir}');";
                commander = new SQLiteCommand(command, connection);
                commander.ExecuteNonQuery();
                //Creates the continously curated backup directory
                if (directoryBackupper.FileSystem.DirectoryExists(curatedBackup))
                {
                    string curatedBackupDirectory = curatedBackup.Split('\\').Last();
                    directoryBackupper.FileSystem.RenameDirectory(curatedBackup, $"{curatedBackupDirectory.Remove(curatedBackupDirectory.LastIndexOf('_'))}_OldCurated");
                    command = $"update BackupDirectories set path = '{curatedBackupDirectory.Remove(curatedBackupDirectory.LastIndexOf('_'))}_OldCurated' where path = '{curatedBackup}'";
                    commander = new SQLiteCommand(command, connection);
                    commander.ExecuteNonQuery();
                }
                destDir += "_Curated";
                foreach (string directory in sourceDirs)
                {
                    tempestDir = $"{destDir}\\{directory.Split('\\').Last()}";
                    System.IO.Directory.CreateDirectory(tempestDir);
                    directoryBackupper.FileSystem.CopyDirectory(directory, tempestDir);
                }
                command = $"insert into BackupDirectories values('{destDir}');";
                commander = new SQLiteCommand(command, connection);
                commander.ExecuteNonQuery();
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                System.Windows.MessageBox.Show("The directory you tried to back-up was invalid or does not exist", "Error Encountered", System.Windows.MessageBoxButton.OK);
            }
            catch (System.IO.IOException)
            {
                System.Windows.MessageBox.Show("Something went wrong. Did you try to back-up a rootfolder?", "Error Encountered", System.Windows.MessageBoxButton.OK);
                exceptionEncountered = true;
            }
            catch (InvalidOperationException)
            {
                System.Windows.MessageBox.Show("Invalid operation. Did you try to back-up your back-up repository?\nBack-up must be deleted to continue", "Error Encountered", System.Windows.MessageBoxButton.OK);
                exceptionEncountered = true;
                directoryBackupper.FileSystem.DeleteDirectory(tempestDir, Microsoft.VisualBasic.FileIO.DeleteDirectoryOption.DeleteAllContents);
            }
            if (!exceptionEncountered)
            {
                curatedBackup = destDir;
            }

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
            Microsoft.VisualBasic.Devices.Computer updater = new Microsoft.VisualBasic.Devices.Computer();

            for (int i = 0; i < backupFilesToUpdate.Count; i++)
            {
                try
                {
                    updater.FileSystem.DeleteFile(backupFilesToUpdate.ElementAt(i));
                }
                catch (System.IO.FileNotFoundException)
                {
                    Console.WriteLine($"File: {backupFilesToUpdate.ElementAt(i)} not found in backup directory. Exception handled.");
                }
                if (updater.FileSystem.DirectoryExists(backupFilesToUpdate.ElementAt(i).Remove(backupFilesToUpdate.ElementAt(i).LastIndexOf('\\'))))
                {
                    updater.FileSystem.CopyFile(modifiedFilePaths.ElementAt(i), backupFilesToUpdate.ElementAt(i));
                }
                else
                {
                    updater.FileSystem.CreateDirectory(backupFilesToUpdate.ElementAt(i).Remove(backupFilesToUpdate.ElementAt(i).LastIndexOf('\\')));
                    updater.FileSystem.CopyFile(modifiedFilePaths.ElementAt(i), backupFilesToUpdate.ElementAt(i));
                }
            }
        }
    }
}
