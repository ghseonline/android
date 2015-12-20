using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Net;

namespace GHSE_Online.Activities
{
    class Unix_Timestamp
    {
        
        public static DateTime ConvertFromUnixTimestamp(int timestamp)
        {
            
            DateTime origin = new DateTime(1970, 1, 1, 1, 0, 0, 0);
            
            origin = origin.AddSeconds(timestamp);

            return origin;



        }
        public static long UnixTimeNow()
        {
            var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)timeSpan.TotalSeconds;
        }
    }
}