using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace VehicleTracking_Api.Utilities.General
{
    public static class Util
    {
        public static bool BeAValidDateTime(string arg)
        {
            DateTime dateTime;

            return DateTime.TryParseExact(arg, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
        }
    }
}
