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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Data.SQLite;

namespace FileBackupSystem_FFM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Fields
        Settings settingsWindow;
        History historyWindow;
        List<string> sourceDirs;
        string curatedBackup;
        readonly List<string> weekDays = new List<string>() { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
        //Database Fields
        SQLiteCommand commander;
        SQLiteDataReader reader;
        string command;
        SQLiteConnection connection;
        string database = "Backup.db";

        //Constructor
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                connection = new SQLiteConnection($"Data Source = {database};Version = 3");
                connection.Open();
                command = "create table Monday(time text primary key);";
                commander = new SQLiteCommand(command, connection);
                commander.ExecuteNonQuery();
                command = "create table Tuesday(time text primary key);";
                commander = new SQLiteCommand(command, connection);
                commander.ExecuteNonQuery();
                command = "create table Wednesday(time text primary key);";
                commander = new SQLiteCommand(command, connection);
                commander.ExecuteNonQuery();
                command = "create table Thursday(time text primary key);";
                commander = new SQLiteCommand(command, connection);
                commander.ExecuteNonQuery();
                command = "create table Friday(time text primary key);";
                commander = new SQLiteCommand(command, connection);
                commander.ExecuteNonQuery();
                command = "create table Saturday(time text primary key);";
                commander = new SQLiteCommand(command, connection);
                commander.ExecuteNonQuery();
                command = "create table Sunday(time text primary key);";
                commander = new SQLiteCommand(command, connection);
                commander.ExecuteNonQuery();
                command = "create table BackupsToKeep(number integer primary key);";
                commander = new SQLiteCommand(command, connection);
                commander.ExecuteNonQuery();
                command = "create table SourceDirPaths(path text primary key);";
                commander = new SQLiteCommand(command, connection);
                commander.ExecuteNonQuery();
                command = "create table RepositoryPath(path text primary key);";
                commander = new SQLiteCommand(command, connection);
                commander.ExecuteNonQuery();
                command = "insert into BackupsToKeep(number) values (2);";
                commander = new SQLiteCommand(command, connection);
                commander.ExecuteNonQuery();
            }
            catch (SQLiteException)
            {
                Console.WriteLine("An exception of SQLiteException occurred. Exception handled.");
            }

            //Load repository path to txtBox_backupLocation
            command = "select * from RepositoryPath;";
            commander = new SQLiteCommand(command, connection);
            reader = commander.ExecuteReader();
            while (reader.Read())
            {
                txtBox_backupLocation.Text = (string)reader[0];
            }
        }

        //Methods
        private void btn_schedule_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                settingsWindow.Close();
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("No Settings.cs to close. NullReferenceException handled.");
            }
            finally
            {
                settingsWindow = new Settings(connection, weekDays);
                Schedule schedule = new Schedule(sourceDirs, txtBox_backupLocation.Text, curatedBackup, weekDays, connection);
                schedule.DateAndTimeCheck();
                settingsWindow.Show();
            }
        }

        private void txtBox_filepathInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Check filepath validity
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            if (txtBox_filepathInput.Text.Length < 248 && txtBox_filepathInput.Text.Length >= 4)
            {
                command = $"insert into table SourceDirPaths values({txtBox_filepathInput.Text})";
                commander = new SQLiteCommand(command, connection);
                commander.ExecuteNonQuery();
                System.Windows.Controls.CheckBox checkbox = new System.Windows.Controls.CheckBox();
                checkbox.Content = txtBox_filepathInput.Text;
                listBox.Items.Add(checkbox);
            }
            txtBox_filepathInput.Clear();
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btn_history_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                historyWindow.Close();
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("No History.cs to close. NullReferenceException handled.");
            }
            finally
            {
                historyWindow = new History(curatedBackup, connection, txtBox_backupLocation.Text);
                historyWindow.Show();
            }
        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void btn_closeToSleep_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_runBackup_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("Unchecked folders will be removed from list of folders to back-up.\nAre you sure you wish to back-up selected folders?", "Confirm backup", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                List<System.Windows.Controls.CheckBox> toRemove = new List<System.Windows.Controls.CheckBox>();
                foreach (System.Windows.Controls.CheckBox item in listBox.Items)
                {
                    if (!(bool)item.IsChecked)
                    {
                        toRemove.Add(item);
                    }
                }
                foreach (System.Windows.Controls.CheckBox item in toRemove)
                {
                    command = $"delete from SourceDirPaths where path = {item.Content}";
                    commander = new SQLiteCommand(command, connection);
                    commander.ExecuteNonQuery();
                    listBox.Items.Remove(item);
                }
                toRemove.Clear();

                List<string> sourceDirs = new List<string>();
                foreach (System.Windows.Controls.CheckBox checkBox in listBox.Items)
                {
                    sourceDirs.Add(checkBox.Content.ToString());
                }
                command = $"select * from RepositoryPath;";
                commander = new SQLiteCommand(command, connection);
                reader = commander.ExecuteReader();
                while (reader.Read())
                {
                    if ((string)reader[0] != txtBox_backupLocation.Text)
                    {
                        command = $"update RepositoryPath set path = {txtBox_backupLocation.Text}";
                    }
                }
                Backupper backup = new Backupper(BackupType.Manual, sourceDirs, txtBox_backupLocation.Text, ref curatedBackup);
            }
            else
            {

            }
        }

        private void btn_browse_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.ShowDialog();
            txtBox_filepathInput.Text = folderBrowser.SelectedPath;
        }

        private void btn_setRepository_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog repositoryBrowser = new FolderBrowserDialog();
            repositoryBrowser.ShowDialog();
            txtBox_backupLocation.Text = repositoryBrowser.SelectedPath;
            //Save to database in close method
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            sourceDirs = new List<string>();
            foreach (System.Windows.Controls.CheckBox checkBox in listBox.Items)
            {
                sourceDirs.Add(checkBox.Content.ToString());
            }
            Backupper backup = new Backupper(BackupType.Automatic, sourceDirs, txtBox_backupLocation.Text, ref curatedBackup);
        }
    }
}
