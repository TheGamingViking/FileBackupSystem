using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileBackupSystem_FFM
{
    public class Schedule
    {
        public void DateAndTimeCheck()
        {
            Dictionary <string, List<string>> schedule = new Dictionary<string, List<string>>() ;
            #region DictionaryFIll
            List<string> Monday = new List<string>();
            Monday.Add("09:00");
            Monday.Add("12:00");
            Monday.Add("15:00");
            schedule.Add("Monday", Monday);
            #endregion
            DateTime conversion;
            string temp;
            int x = 60;
            for (int i = 0; i < schedule.Count; i++)
            {
                conversion = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                if (schedule.Keys.ElementAt(i).ToLower() == conversion.DayOfWeek.ToString().ToLower())
                {
                    foreach (string time in schedule.Values.ElementAt(i))
                    {
                        temp = time;
                        temp = temp.Remove(2);
                        TimeSpan ts = new TimeSpan(Convert.ToInt32(temp.TrimStart('0')), DateTime.Now.Minute, DateTime.Now.Second);
                        conversion = conversion.Date + ts;
                        MessageBox.Show(Convert.ToString(conversion.AddMinutes(x)));
                        if (conversion.AddMinutes(x) > DateTime.Now && conversion < DateTime.Now)
                        {
                            MessageBox.Show("Fuck Yeah");
                            //Run auto backup
                        }
                    }
                }
            }
            
            
            /*
            for (int i = 0; i < schedule.Values.Count; i++)
            {
                //Check time.Now vs. schedule.Values[i]
                temp = schedule.Values.ElementAt(i).Remove(2);
                conversion = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(temp.TrimStart('0')), DateTime.Now.Minute, DateTime.Now.Second);
                if (conversion > DateTime.Now && conversion < DateTime.Now.AddMinutes(x))
                {
                    if (schedule.Keys.ElementAt(i).ToLower() == conversion.DayOfWeek.ToString().ToLower())
                    {

                    }
                }
            }
            */


        }
    }
}
