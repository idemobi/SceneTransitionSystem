using System;

namespace SceneTransitionSystem
{
    /// <summary>
    /// A helper class for converting DateTime to Unix Timestamp and vice versa.
    /// </summary>
    public class STSDateHelper
    {
        /// <summary>
        /// Convert a DateTime to a Unix Timestamp (since 1 January 1970)
        /// </summary>
        /// <param name="sDate">A DateTime object</param>
        /// <returns>The number of seconds since the Unix epoch as a double</returns>
        public static double ConvertToTimestamp(DateTime sDate)
        {
            return sDate.ToUniversalTime().Subtract(
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            ).TotalSeconds;
        }

        /// <summary>
        /// Converts a Unix Timestamp (since 1 January 1970) to a DateTime.
        /// </summary>
        /// <param name="sTimeStamp">A Unix Timestamp.</param>
        /// <returns>A converted Unix Timestamp to DateTime.</returns>
        public static DateTime ConvertFromTimestamp(double sTimeStamp)
        {
            DateTime rDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            rDateTime = rDateTime.AddSeconds(sTimeStamp);
            return rDateTime;
        }
    }
}