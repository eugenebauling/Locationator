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
using Locationator.Enums;

namespace Locationator
{
    public static class Settings
    {
        public static GpsPointCollectionMode GpsLocationMode { get; set; }
        public static DataAccessMode DataAccessMode { get; set; }

        public static void SetDefaults()
        {
            GpsLocationMode = GpsPointCollectionMode.Manual;
            DataAccessMode = DataAccessMode.WebService;
        }

    }
}