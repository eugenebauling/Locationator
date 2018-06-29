using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Support.V7.App;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Preferences;
using Locationator.Screens.Fragments;
using Locationator.Enums;

namespace Locationator.Screens.Activities
{
    [Activity(Label = "SettingsActivity")]
    public class SettingsActivity : PreferenceActivity, ISharedPreferencesOnSharedPreferenceChangeListener
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            AddPreferencesFromResource(Resource.Layout.Preferences);

            PreferenceManager.GetDefaultSharedPreferences(this).RegisterOnSharedPreferenceChangeListener(this);
            SetGpsSetting();
        }

        public void OnSharedPreferenceChanged(ISharedPreferences sharedPreferences, string key)
        {
            switch(key) {
			case Constants.SETTINGS_KEY_GPS_TYPE:
                    SetGpsSetting();
                break;
            }
        }

        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void OnPause()
        {
            base.OnPause();
            PreferenceManager.GetDefaultSharedPreferences(this).UnregisterOnSharedPreferenceChangeListener(this);
        }

        protected override void OnResume()
        {
            base.OnResume();
            PreferenceManager.GetDefaultSharedPreferences(this).RegisterOnSharedPreferenceChangeListener(this);
        }

        private void SetGpsSetting()
        {

            ListPreference gpsTypePref = (ListPreference)FindPreference(Constants.SETTINGS_KEY_GPS_TYPE);
            PreferenceScreen advancedGpsPref = (PreferenceScreen)FindPreference(Constants.SETTINGS_KEY_GPS_ADVANCED);

            var setting = PreferenceManager.GetDefaultSharedPreferences(this).GetString(Constants.SETTINGS_KEY_GPS_TYPE, "");
            int title = 0;
            int summary = 0;
            int advSummary = 0;
            int dialogTitle = Resource.String.SETTINGS_GPS_SELECT;

            if (Convert.ToInt32(setting) == (int)GpsPointCollectionMode.SystemSpecified)
            {
                title = Resource.String.SETTINGS_GPS_SETTING_SYS;
                summary = Resource.String.SETTINGS_GPS_SETTING_DESC_SYSTEM;
                advSummary = Resource.String.SETTINGS_GPS_ADVANCED_SYS;
            }
            else if (Convert.ToInt32(setting) == (int)GpsPointCollectionMode.Manual)
            {
                title = Resource.String.SETTINGS_GPS_SETTING_MAN;
                summary = Resource.String.SETTINGS_GPS_SETTING_DESC_MANUAL;
                advSummary = Resource.String.SETTINGS_GPS_ADVANCED_MAN;
            }

            if (gpsTypePref != null)
            {
                gpsTypePref.SetSummary(summary);
                gpsTypePref.SetTitle(title);
                gpsTypePref.SetDialogTitle(dialogTitle);
            }

            if (advancedGpsPref != null)
            {
                advancedGpsPref.SetSummary(advSummary);
            }
        }
    }
}