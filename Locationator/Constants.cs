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
        public const int APP_ID = 999;

        public const string URL_SAVE_LOCATION_POINT = "http://fleetcrmdal.bbasoftware.co.za/api/Position/AddToQueue/";

        public const string SETTINGS_KEY_GPS_TYPE = "GpsSetting";
        public const string SETTINGS_KEY_GPS_ADVANCED = "GpsAdvanced";
        public const string SETTINGS_KEY_GPS_GPS = "GpsUseGps";
        public const string SETTINGS_KEY_GPS_GSM = "GpsUseGsm";
        public const string SETTINGS_KEY_GPS_PAS = "GpsUsePassive";
        public const string SETTING_KEY_GPS_ACCURACY = "GpsAccuracy";
        public const string SETTING_KEY_GPS_POWER = "GpsPower";
        public const string SETTINGS_KEY_GPS_UPDATE_INTERVAL_MILLISECONDS = "GpsUpdateMilSec";
        public const string SETTINGS_KEY_GPS_UPDATE_INTERVAL_METRES = "GpsUpdateMetres";
    }
}