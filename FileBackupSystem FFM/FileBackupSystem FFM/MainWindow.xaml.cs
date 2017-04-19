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

namespace FileBackupSystem_FFM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Fields
        string curatedBackup;

        //Constructor
        public MainWindow()
        {
            InitializeComponent();
            //Load repository path to txtBox_backupLocation
        }

        private void btn_schedule_Click(object sender, RoutedEventArgs e)
        {
            Settings settingsWindow = new Settings();
            Schedule schedule = new Schedule();
            schedule.DateAndTimeCheck();
            settingsWindow.Show();
        }

        private void txtBox_filepathInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Check filepath validity
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            if (txtBox_filepathInput.Text != "" || txtBox_filepathInput.Text != " ")
            {
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
            History historyWindow = new History();
            historyWindow.Show();
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
                    listBox.Items.Remove(item);
                }
                toRemove.Clear();

                List<string> sourceDirs = new List<string>();
                foreach (System.Windows.Controls.CheckBox checkBox in listBox.Items)
                {
                    sourceDirs.Add(checkBox.Content.ToString());
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
            List<string> sourceDirs = new List<string>();
            foreach (System.Windows.Controls.CheckBox checkBox in listBox.Items)
            {
                sourceDirs.Add(checkBox.Content.ToString());
            }
            Backupper backup = new Backupper(BackupType.Automatic, sourceDirs, txtBox_backupLocation.Text, ref curatedBackup);
        }
    }
}
