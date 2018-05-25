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
using Android.Content.Res;
using Locationator.Enums;

namespace Locationator
{
    public static class Constants
    {
        public const int GPS_LOCATION_UPDATE_INTERVAL_TIME = 1000;
        public const int GPS_LOCATION_UPDATE_INTERVAL_DISTANCE_METRES = 1;
        public const int APP_ID = 999;

        public const string URL_SAVE_LOCATION_POINT = "http://fleetcrmdal.bbasoftware.co.za/api/Position/AddToQueue/";

    }

}