using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.Common.Extensions
{
    public static class DateExtensions
    {
        public static DateTime AddBusinessDays(this DateTime value, int days)
        {
            if (days < 0)
                throw new ArgumentException("Incorrect value.");

            int counter = 0;

            for (int i = 0; i < days; i++)
                if (value.AddDays(i).DayOfWeek == DayOfWeek.Saturday || value.AddDays(i).DayOfWeek == DayOfWeek.Sunday)
                    counter++;

            var result = value.AddDays(days + counter);
            counter = (3 - (((int)result.DayOfWeek % 5) * ((int)result.DayOfWeek / 5))) % 3;

            return result.AddDays(counter);
        }
    }
}
