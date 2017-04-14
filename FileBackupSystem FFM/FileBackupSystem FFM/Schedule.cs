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
        DateTime midnight = new DateTime(2017, 4, 14, 23, 59, 59, 999);
        DateTime deadline = new DateTime(2017, 4, 14, 0, 1, 0, 0);
        DateTime dt = new DateTime();
        int i = 1;
        int j = 1;
        List<string> userdays = new List<string> { "Monday", "Tuesday", "Wensday", "Thursday", "Friday", "Saturday", "Sunday" };
        List<string> usertimes = new List<string> { "11:00", "15:00", "02:00", "12:00", "09:00", "19:00", "20:00" };

        public void DateAndTimeCheck()
        {
            dt = DateTime.Now;
            foreach (var day in userdays)
            {
                if (day == dt.DayOfWeek.ToString())
                {
                    MessageBox.Show("Correct day");

                    foreach (var time in usertimes)
                    {
                        string time1 = time.Remove(2);
                        if ((time1.Trim('0') == dt.Hour.ToString())&&(i==j))
                        {
                            MessageBox.Show("The time is right");
                            //kør autobackup
                            break;
                        }
                        else if (i==j)
                        {
                            TimeSpan deadlinets = new TimeSpan(Convert.ToInt32(time1.Trim('0')), 0, 0);
                            deadline = deadline.Date + deadlinets;
                            TimeSpan duration = deadline.Subtract(dt);
                            if (Convert.ToString(duration).Contains('-'))
                            {
                                MessageBox.Show(Convert.ToString(duration));
                                MidNightEvent();
                                break;
                            }
                            else
                            {
                                //lav event til køre DateAndTimeCheck() igen når tiden i duration er 0
                                break;
                            }
                            
                            
                        }
                        j++;
                    }
                    break;
                }
                else if (i==userdays.Count)
                {
                    MidNightEvent();
                }
                i++;
            }
            
        }
        public void MidNightEvent()
        {
            TimeSpan duration = midnight.Subtract(dt);
            MessageBox.Show(Convert.ToString(duration));
            //lav event til køre DateAndTimeCheck() basert på duration så vi har passeret midnat.
        }
    }
}
