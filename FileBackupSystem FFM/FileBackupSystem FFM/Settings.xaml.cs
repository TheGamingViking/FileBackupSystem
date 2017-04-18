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
    /// Interaction logic for Schedule.xaml
    /// </summary>
    public partial class Settings : Window
    {
        //database fields
        SQLiteConnection connection;
        SQLiteCommand commander;
        string command;
       

        //constructor
        public Settings(SQLiteConnection connection)
        {
            InitializeComponent();
            this.connection = connection;
            List<ScheduleTime> schedule = new List<ScheduleTime>();
            comBox_backups.Text = "1";
            comBox_day.Text = "Monday";
            comBox_hours.Text = "00:00";

        }
        //methods
        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            lV_schedule.Items.Add(new ScheduleTime() { Day = comBox_day.Text, Time = comBox_hours.Text });

        }

        private void btn_remove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lV_schedule.Items.Remove(lV_schedule.SelectedItem);
            }
            catch (ArgumentOutOfRangeException)
            {

            }
        }

        private void btn_confirm_Click(object sender, RoutedEventArgs e)
        {
            //Save settings
            command = $";";
            commander = new SQLiteCommand(command, connection);
            commander.ExecuteNonQuery();

            this.Close();
        }
    }

    public class ScheduleTime
    {
        public string Day { get; set; }
        public string Time { get; set; }
    }
}
