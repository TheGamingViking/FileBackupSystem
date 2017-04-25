using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SQLite;

namespace FileBackupSystem_FFM
{
    //Code incomplete and server no function other than a menu that shows the amount of backups.
    public partial class History : Window
    {
        //Fields
        string curatedBackup;
        string destDir;
        string temp;
        List<BackupDirectory> backupDirectories;

        //Database Fields
        SQLiteConnection connection;
        string command;
        SQLiteCommand commander;
        SQLiteDataReader reader;

        //Constructor
        public History(string curatedBackup, SQLiteConnection connection, string destDir)
        {
            InitializeComponent();
            this.curatedBackup = curatedBackup;
            this.connection = connection;
            this.destDir = destDir;
            backupDirectories = new List<BackupDirectory>();
            //Add backups to listBox
            command = $"select * from BackupDirectories;";
            commander = new SQLiteCommand(command, connection);
            reader = commander.ExecuteReader();
            while (reader.Read())
            {
                temp = (reader[0] as string).Split('\\').Last();
                if (temp.Contains("Curated"))
                {
                    temp = temp.Remove(temp.LastIndexOf('_'));
                }
                backupDirectories.Add(new BackupDirectory() { Name = DateTime.FromOADate(Convert.ToDouble(temp)).ToString(), Path = temp });
            }
            listBox.ItemsSource = backupDirectories;
            listBox.DisplayMemberPath = "Name";
        }

        //Methods
        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void checkBox2_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }

    public class BackupDirectory
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }
}
