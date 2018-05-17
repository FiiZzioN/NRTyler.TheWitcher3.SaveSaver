// ***********************************************************************
// Assembly         : NRTyler.TheWitcher3.SaveSaver
//
// Author           : Nicholas Tyler
// Created          : 05-16-2018
//
// Last Modified By : Nicholas Tyler
// Last Modified On : 05-17-2018
//
// License          : MIT License
// ***********************************************************************

using System;

namespace NRTyler.TheWitcher3.SaveSaver
{
    public static class BackupLabeller
    {
        /// <summary>
        /// Returns the current date in a format that's compatible with directory name character rules.
        /// </summary>
        public static string GetValidDateFormat()
        {
            var dateTime = DateTime.Now;

            var month = dateTime.Month;
            var day   = dateTime.Day;
            var year  = dateTime.Year;

            return $"{month}-{day}-{year}";
        }

        /// <summary>
        /// Returns the current time in a format that's compatible with directory name character rules.
        /// </summary>
        public static string GetValidTimeFormat()
        {
            var dateTime = DateTime.Now.TimeOfDay;

            var hour   = dateTime.Hours;
            var minute = dateTime.Minutes;
            var second = dateTime.Seconds;

            return hour < 12 ? $"{hour}.{minute}.{second} AM" : $"{hour - 12}.{minute}.{second} PM";
        }
    }
}