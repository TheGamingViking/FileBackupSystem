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
    /// <summary>
    /// Interaction logic for History.xaml
    /// </summary>
    public partial class History : Window
    {
        //Fields
        string curatedBackup;
        string destDir;
        string temp;

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
            //Add backups to listBox
            command = $"select * from SourceDirPaths;";
            commander = new SQLiteCommand(command, connection);
            reader = commander.ExecuteReader();
            while (reader.Read())
            {
                temp = (string)reader[0];
                listBox.Items.Add(new SourceDirectory() { Name = DateTime.FromOADate(Convert.ToDouble(temp.Split('\\').Last().TrimEnd('_'))).ToString(), Path = temp });
            }
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

    public class SourceDirectory
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }
}
