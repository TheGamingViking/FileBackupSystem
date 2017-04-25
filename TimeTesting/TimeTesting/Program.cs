using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTesting
{
    class Program
    {
        
        static void Main(string[] args)
        {
            DateTime dt = new DateTime();
            dt = DateTime.Now;
            Console.WriteLine(dt.TimeOfDay);
            Console.WriteLine(dt.DayOfWeek);
            Console.WriteLine(dt.DayOfWeek);
            Console.ReadLine();
        }
    }
}
