using System;
using System.Collections.Generic;
using System.Linq;
using Hourly.Time;

namespace Hourly.Utils
{
    public static class Extensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null || !enumerable.Any();
        }

        public static bool IsToday(this DateTime? dateTime)
        {
            return dateTime != null && TimeProvider.GetCurrentTime().Date == dateTime?.Date;
        }
    }
}