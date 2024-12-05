using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace api.Extensions
{
    public static class DateTimeExtensions
    {
        private static Dictionary<string,string> timezoneDictionary => new Dictionary<string, string>
        {
            { "Australia/Sydney", "AUS Eastern Standard Time" },
            { "Asia/Kolkata", "India Standard Time" },
            { "America/Sao_Paulo", "E. South America Standard Time" },
            { "Europe/Berlin", "W. Europe Standard Time" },
            { "Hongkong", "China Standard Time" },
            { "Africa/Johannesburg", "South Africa Standard Time" },
            { "Asia/Seoul", "Korea Standard Time" },
            { "Europe/London", "GMT Standard Time" },
            { "America/New_York", "Eastern Standard Time" },
            { "Asia/Shanghai", "China Standard Time" },
            { "Europe/Zurich", "W. Europe Standard Time" },
            { "Asia/Tokyo", "Tokyo Standard Time" },
            { "America/Toronto", "Eastern Standard Time" },
            { "Europe/Amsterdam", "W. Europe Standard Time" },
            { "Europe/Brussels", "Romance Standard Time" },
            { "Europe/Budapest", "Central Europe Standard Time" },
            { "America/Argentina/Buenos_Aires", "Argentina Standard Time" },
            { "Africa/Cairo", "Egypt Standard Time" },
            { "America/Chicago", "Central Standard Time" },
            { "Europe/Copenhagen", "Romance Standard Time" },
            { "Asia/Dubai", "Arabian Standard Time" },
            { "Asia/Qatar", "Arab Standard Time" },
            { "Europe/Paris", "Romance Standard Time" },
            { "Europe/Helsinki", "FLE Standard Time" },
            { "Atlantic/Reykjavik", "Greenwich Standard Time" },
            { "Europe/Dublin", "GMT Standard Time" },
            { "Asia/Istanbul", "Turkey Standard Time" },
            { "Asia/Jakarta", "SE Asia Standard Time" },
            { "Asia/Kuala_Lumpur", "Singapore Standard Time" },
            { "Pacific/Auckland", "New Zealand Standard Time" },
            { "Europe/Oslo", "W. Europe Standard Time" },
            { "Europe/Prague", "Central Europe Standard Time" },
            { "Asia/Riyadh", "Arab Standard Time" },
            { "Asia/Bangkok", "SE Asia Standard Time" },
            { "Europe/Stockholm", "W. Europe Standard Time" },
            { "Asia/Taipei", "Taipei Standard Time" },
            { "Asia/Tel_Aviv", "Israel Standard Time" },
            { "Europe/Vienna", "W. Europe Standard Time" },
            { "Europe/Warsaw", "Central European Standard Time" }
        };

        public static TimeSpan Age(this DateTime dateTime){
            //Console.WriteLine($"Age ({(DateTime.UtcNow-dateTime).TotalHours}) -> {DateTime.UtcNow.ToString()} {dateTime.ToString()} ");
            return(DateTime.UtcNow-dateTime);
        }
        public static DateTime TodayTime(this DateTime dateTime){
            DateTime rtn=DateTime.Today;
            rtn.AddHours(dateTime.Hour);
            rtn.AddMinutes(dateTime.Minute);
            rtn.AddSeconds(dateTime.Second);
            return rtn;

        }

        public static DateTime UtcTime(this string timeString,string timeZone){
            Console.WriteLine("UtcTime start "+timeString);
            string[] timeSplit=timeString.Split(' ');
            Console.WriteLine($"UtcTime split: {string.Join(",", timeSplit)}");
            if(timeSplit.Length==1)return DateTime.UtcNow;
            
            timeString=timeString.Substring(0, timeString.Length-(timeSplit[timeSplit.Length-1].Length)).Trim();
            string format="h:mm tt";
            //Console.WriteLine("Pure time:"+timeString);
            
            DateTime localTime=DateTime.ParseExact(timeString,format,CultureInfo.InvariantCulture);
            //Console.WriteLine($"localTime:"+localTime.ToString());
            if(!timezoneDictionary.TryGetValue(timeZone,out string timeZoneString)){
                return localTime;
            }
            // Console.WriteLine("Time zone:"+timeZoneString);
            TimeZoneInfo tz=TimeZoneInfo.FindSystemTimeZoneById(timeZoneString);
            DateTime utcTime=TimeZoneInfo.ConvertTimeToUtc(localTime, tz);
            // Console.WriteLine($"UtcTime result"+utcTime.ToString());
            return utcTime;
        }
    }
}