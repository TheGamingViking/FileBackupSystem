using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;

namespace FileBackupSystem_FFM
{
    public class Schedule
    {
        public void DateAndTimeCheck()
        {
            Dictionary <string, List<string>> schedule = new Dictionary<string, List<string>>() ;
            DateTime conversion;
            string temp;
            int x = 5;

            #region DictionaryTestFill
            List<string> Monday = new List<string>();
            Monday.Add("09:00");
            Monday.Add("12:00");
            Monday.Add("15:00");
            schedule.Add("Monday", Monday);
            #endregion
            
            for (int i = 0; i < schedule.Count; i++)
            {
                conversion = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                //Check if it is the right day for example Monday
                if (schedule.Keys.ElementAt(i).ToLower() == conversion.DayOfWeek.ToString().ToLower())
                {
                    //Get the timestamp from the list belonging to the given day.
                    foreach (string time in schedule.Values.ElementAt(i))
                    {
                        //Trim the string so that it can be converted and compared to a Datetime.
                        temp = time;
                        temp = temp.Remove(2);
                        TimeSpan ts = new TimeSpan(Convert.ToInt32(temp.TrimStart('0')), 0, 0);
                        conversion = conversion.Date + ts;
                        //Check if the CUrrent time (DateTime.Now) is larger than the scheduled time (Conversion) and that current time is smaller than conversion + 5 min.
                        if (conversion < DateTime.Now && conversion.AddMinutes(x) > DateTime.Now)
                        {
                            //Run auto backup
                        }
                    }
                }
            }
            //Set the timer for the next auto backup check in about an hour.
            SetTimer();
        }
        public void SetTimer()
        {
            //Calculate the time remaning until the next whole hour.
            DateTime current = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            DateTime NextHour = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 27, 30);
            NextHour = NextHour.AddHours(0);
            TimeSpan TilWholeHour = NextHour - current;
            MessageBox.Show(Convert.ToString(TilWholeHour));
            //Give the timer the calculated time so that it can call the event at the next whole hour.
            var timer = new System.Timers.Timer(TilWholeHour.TotalMilliseconds);
            timer.Elapsed += TimerEvent;
            timer.Enabled = true;

        }
        private void TimerEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            //Call the metod to check if there is a scheduled back up at this hour.
            DateAndTimeCheck();
        }
    }
}
