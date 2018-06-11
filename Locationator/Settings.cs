using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Locationator.Enums;

namespace Locationator
{
    public static class Settings
    {
        public static GpsPointCollectionMode GpsLocationMode { get; set; }
        public static DataAccessMode DataAccessMode { get; set; }
        public static Accuracy GPSAutoAccuracy { get; set; }
        public static Power GPSAutoPower { get; set; }
        public static bool UseGPS { get; set; }
        public static bool UseNetwork { get; set; }
        public static bool UsePassive { get; set; }

        public static void SetDefaults()
        {
            GpsLocationMode = GpsPointCollectionMode.SystemSpecified;
            GPSAutoAccuracy = Accuracy.Fine;
            GPSAutoPower = Power.High;
            DataAccessMode = DataAccessMode.WebService;
        }
    }
}