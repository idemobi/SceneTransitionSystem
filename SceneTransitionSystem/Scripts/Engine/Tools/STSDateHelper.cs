//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;

//=====================================================================================================================
namespace SceneTransitionSystem
{
	public class STSDateHelper
	{
        //-------------------------------------------------------------------------------------------------------------
        ///// <summary>
        ///// Convert a DateTime to an Unix Timestamp (since 1 january 1970)
        ///// </summary>
        ///// <param name="sDate">A DateTime</param>
        ///// <returns>A converted DateTime to Unix Timestamp.</returns>
        //public static double ConvertToUnixTimestamp(DateTime sDate)
        //{
        //    DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        //    TimeSpan diff = sDate.ToUniversalTime() - origin;
        //    return Math.Floor(diff.TotalSeconds);
        //}
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Convert a DateTime to an Unix Timestamp (since 1 january 1970)
        /// </summary>
        /// <param name="sDate">A DateTime</param>
        /// <returns>A converted DateTime to Unix Timestamp.</returns>
        public static double ConvertToTimestamp(DateTime sDate)
        {
            return sDate.ToUniversalTime().Subtract(
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            ).TotalSeconds;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Converts from timestamp.
        /// </summary>
        /// <returns>The from timestamp.</returns>
        /// <param name="sTimeStamp">S time stamp.</param>
        public static DateTime ConvertFromTimestamp(double sTimeStamp)
        {
            DateTime rDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            rDateTime = rDateTime.AddSeconds(sTimeStamp);
            return rDateTime;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================