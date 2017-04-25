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
    public partial class Settings : Window
    {
        //database fields
        SQLiteConnection connection;
        SQLiteCommand commander;
        string command;
        SQLiteDataReader reader;
        //fields
        List<ScheduleTime> schedule;
        readonly List<string> weekDays;

        //constructor
        public Settings(SQLiteConnection connection, List<string> weekDays)
        {
            InitializeComponent();
            this.weekDays = weekDays;
            //Load database tabels into lists.
            
            this.connection = connection;
            schedule = new List<ScheduleTime>();
            
            foreach (string day in weekDays)
            {
                command = $"select * from {day};";
                commander = new SQLiteCommand(command, connection);
                reader = commander.ExecuteReader();
                while (reader.Read())
                {
                    schedule.Add(new ScheduleTime() { Day = day, Time = (string)reader[0] });
                    lV_schedule.Items.Add(schedule.Last());
                }
            }
            command = $"select * from BackupsToKeep;";
            commander = new SQLiteCommand(command, connection);
            reader = commander.ExecuteReader();
            while (reader.Read())
            {
                comBox_backups.Text = Convert.ToString(reader[0]);
            }
            comBox_day.Text = "Monday";
            comBox_hours.Text = "00:00";

        }
        //methods
        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            schedule.Clear();
            this.Close();
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            schedule.Add(new ScheduleTime() { Day = comBox_day.Text, Time = comBox_hours.Text });
            lV_schedule.Items.Add(schedule.Last());
        }

        private void btn_remove_Click(object sender, RoutedEventArgs e)
        {
            //Code for removing selected times form schedule.
            try
            {
                foreach (ScheduleTime item in schedule)
                {
                    if (item.Equals(lV_schedule.SelectedItem))
                    {
                        schedule.Remove(item);
                        break;
                    }
                }
                lV_schedule.Items.Remove(lV_schedule.SelectedItem);
            }
            catch (ArgumentOutOfRangeException)
            {

            }
        }

        private void btn_confirm_Click(object sender, RoutedEventArgs e)
        {
            //Save schedule/settings to database and close schedule window.
            foreach (string day in weekDays)
            {
                command = $"select * from {day};";
                commander = new SQLiteCommand(command, connection);
                reader = commander.ExecuteReader();
                while (reader.Read())
                {
                    ScheduleTime temp = new ScheduleTime { Day = day, Time = (string)reader[0] };
                    if (schedule.Contains(temp))
                    {
                        schedule.Remove(temp);
                    }
                    else
                    {
                        command = $"delete from {day} where time = '{temp.Time}';";
                        commander = new SQLiteCommand(command, connection);
                        commander.ExecuteNonQuery();
                        schedule.Remove(temp);
                    }
                }
            }
            foreach (ScheduleTime item in schedule)
            {
                command = $"insert into {item.Day} values('{item.Time}');";
                commander = new SQLiteCommand(command, connection);
                commander.ExecuteNonQuery();
            }
            command = $"update BackupsToKeep set number = {comBox_backups.Text};";
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
