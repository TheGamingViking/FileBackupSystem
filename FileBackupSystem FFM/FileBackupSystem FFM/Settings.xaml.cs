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

namespace FileBackupSystem_FFM
{
    /// <summary>
    /// Interaction logic for Schedule.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
            List<ScheduleTime> schedule = new List<ScheduleTime>();
            char[] charsToTrim = new char[3];
            charsToTrim[0] = ':';
            charsToTrim[1] = '0';
            charsToTrim[2] = '0';
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            lV_schedule.Items.Add(new ScheduleTime() { Day = comBox_day.Text, Time = comBox_hours.Text});
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
    }

    public class ScheduleTime
    {
        public string Day { get; set; }
        public string Time { get; set; }
    }
}
