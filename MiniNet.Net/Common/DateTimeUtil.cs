using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniNet.Net.Common
{
    public class DateTimeUtil
    {
        public static DateTime ParseTimeStamp(long dateVal)
        {
            DateTime timeStamp = new DateTime(1970, 1, 1);

            long t = dateVal * 10000 + timeStamp.Ticks;
            DateTime dt = new DateTime(t);

            return dt.AddHours(8);
        }

        public static long GetTimeStamp()
        {
            DateTime timeStamp = new DateTime(1970, 1, 1);

            long t = ((DateTime.UtcNow.Ticks / 1000 - timeStamp.Ticks / 1000) / 10000) * 1000;

            return t;
        }
    }
}
