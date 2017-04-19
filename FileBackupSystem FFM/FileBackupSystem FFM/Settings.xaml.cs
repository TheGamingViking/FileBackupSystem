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
        //fields
        List<string> Monday;
        List<string> Tuesday;
        List<string> Wednesday;
        List<string> Thursday;
        List<string> Friday;
        List<string> Saturday;
        List<string> Sunday;
        //constructor
        public Settings(SQLiteConnection connection)
        {
            InitializeComponent();
            this.connection = connection;
            List<ScheduleTime> schedule = new List<ScheduleTime>();
            //Load database tabels into lists.
            Monday = new List<string>();
            Tuesday = new List<string>();
            Wednesday = new List<string>();
            Thursday = new List<string>();
            Friday = new List<string>();
            Saturday = new List<string>();
            Sunday = new List<string>();

            comBox_backups.Text = "1";
            comBox_day.Text = "Monday";
            comBox_hours.Text = "00:00";

        }
        //methods
        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            Monday.Clear();
            Tuesday.Clear();
            Wednesday.Clear();
            Thursday.Clear();
            Friday.Clear();
            Saturday.Clear();
            Sunday.Clear();
            this.Close();
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            switch (comBox_day.Text)
            {
                case "Monday":
                    Monday.Add(comBox_hours.Text);
                    break;
                case "Tuesday":
                    Tuesday.Add(comBox_hours.Text);
                    break;
                case "Wednesday":
                    Wednesday.Add(comBox_hours.Text);
                    break;
                case "Thursday":
                    Thursday.Add(comBox_hours.Text);
                    break;
                case "Friday":
                    Friday.Add(comBox_hours.Text);
                    break;
                case "Saturday":
                    Saturday.Add(comBox_hours.Text);
                    break;
                case "Sunday":
                    Sunday.Add(comBox_hours.Text);
                    break;
            }
            lV_schedule.Items.Add(new ScheduleTime() { Day = comBox_day.Text, Time = comBox_hours.Text});
        }

        private void btn_remove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBox.Show(Convert.ToString(lV_schedule.View.GetLocalValueEnumerator().Current));
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
