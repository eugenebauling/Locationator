using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Locationator.Enums;

namespace Locationator
{
    public static class Settings
    {
        public static GpsPointCollectionMode GpsLocationMode {
            get
            {
                //Lovely hack because GetString works while GetInt throws Java cast exceptions
                string setting = PreferenceManager.GetDefaultSharedPreferences(Application.Context).GetString(Constants.SETTINGS_KEY_GPS_TYPE, "1");
                switch (Convert.ToInt32(setting))
                {
                    case 0:
                        return GpsPointCollectionMode.Manual;

                    case 1:
                        return
                            GpsPointCollectionMode.SystemSpecified;
                }

                return GpsPointCollectionMode.SystemSpecified;
            }
        }
        public static Accuracy GPSAutoAccuracy
        {
            get
            {
                string setting = PreferenceManager.GetDefaultSharedPreferences(Application.Context).GetString(Constants.SETTING_KEY_GPS_ACCURACY, "0");

                switch (Convert.ToInt32(setting))
                {
                    case 0:
                        return Accuracy.NoRequirement;
                    case 1:
                        return Accuracy.Low;
                        //return Accuracy.Fine;
                    case 2:
                        return Accuracy.Medium;
                        //return Accuracy.Coarse;
                    case 3:
                        return Accuracy.High;
                }

                return Accuracy.NoRequirement;
            }
        }
        public static Power GPSAutoPower
        {
            get
            {
                string setting = PreferenceManager.GetDefaultSharedPreferences(Application.Context).GetString(Constants.SETTING_KEY_GPS_POWER, "0");
                switch (Convert.ToInt32(setting))
                {
                    case 0:
                        return Power.NoRequirement;
                    case 1:
                        return Power.Low;
                    //return Accuracy.Fine;
                    case 2:
                        return Power.Medium;
                    //return Accuracy.Coarse;
                    case 3:
                        return Power.High;
                }

                return Power.NoRequirement;
            }
        }
        public static bool UseGPS
        {
            get
            {
                var setting = PreferenceManager.GetDefaultSharedPreferences(Application.Context).GetBoolean(Constants.SETTINGS_KEY_GPS_GPS, false);
                return setting;
            }
        }
        public static bool UseNetwork
        {
            get
            {
                var setting = PreferenceManager.GetDefaultSharedPreferences(Application.Context).GetBoolean(Constants.SETTINGS_KEY_GPS_GSM, false);
                return setting;
            }
        }
        public static bool UsePassive
        {
            get
            {
                var setting = PreferenceManager.GetDefaultSharedPreferences(Application.Context).GetBoolean(Constants.SETTINGS_KEY_GPS_PAS, true);
                return setting;
            }
        }
        public static int GpsLocationUpdateIntervalMilliseconds
        {
            get
            {
                return Convert.ToInt32(PreferenceManager.GetDefaultSharedPreferences(Application.Context).GetString(Constants.SETTINGS_KEY_GPS_UPDATE_INTERVAL_MILLISECONDS, "1000"));
            }
        }
        public static int GpsLocationUpdateIntervalMetres
        {
            get
            {
                return Convert.ToInt32(PreferenceManager.GetDefaultSharedPreferences(Application.Context).GetString(Constants.SETTINGS_KEY_GPS_UPDATE_INTERVAL_METRES, "1"));
            }
        }
        public static DataAccessMode DataAccessMode { get; set; }

        public static void SetDefaults()
        {
            DataAccessMode = DataAccessMode.WebService;
        }
    }
}