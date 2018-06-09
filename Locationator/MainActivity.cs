﻿using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Locations;
using Android.Util;
using Locationator.LocationProvider;

namespace Locationator
{
    [Activity(Label = "Locationator", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {

        private LocationManager locMgr;
        private string tag;
        private GpsPointProvider gpsPoints;

        #region Controls

        TextView gpsText;
        Button gpsShowBtn;

        #endregion

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            Settings.SetDefaults();
            tag = this.BaseContext.GetText(Resource.String.TAG_MAIN_ACTIVITY);
            locMgr = GetSystemService(Context.LocationService) as LocationManager;
            gpsPoints = new GpsPointProvider(this.BaseContext, locMgr);
            gpsPoints.StartGettingLocationPoints();

            gpsText = FindViewById<TextView>(Resource.Id.gpsText);
            gpsShowBtn = FindViewById<Button>(Resource.Id.gpsRequestButton);

            LinkBtnEvents();
        }

        protected override void OnResume()
        {
            base.OnResume();
            
            gpsPoints.StartGettingLocationPoints();
            gpsText.Text += GetGpsPointText();

            Log.Info(tag, GetGpsPointText());
        }

        protected override void OnPause()
        {
            base.OnPause();
            gpsPoints.StartGettingLocationPoints();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            locMgr.Dispose();
            locMgr = null;
            gpsPoints.Dispose();
            gpsPoints = null;
        }

        private string GetGpsPointText()
        {
            return "Longitude: " + gpsPoints.CurrentLongitude + "; Latitude: " + gpsPoints.CurrentLatitude + "; Accuracy: " + gpsPoints.CurrentAccuracy + "\r\n";
        }

        private void LinkBtnEvents()
        {
            gpsShowBtn.Click += GpsShowBtn_Click;
        }

        private void GpsShowBtn_Click(object sender, EventArgs e)
        {
            gpsText.Text += GetGpsPointText();
        }
    }
}

