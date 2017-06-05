using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Locations;
using Android.Util;

namespace Locationator
{
    [Activity(Label = "Locationator", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {

        private LocationManager locMgr;
        private const string tag = "MainActivity";
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
            locMgr = GetSystemService(Context.LocationService) as LocationManager;
            gpsPoints = new GpsPointProvider(locMgr);
            gpsPoints.StartGettingLocationPoints();

            gpsText = FindViewById<TextView>(Resource.Id.gpsText);
            gpsShowBtn = FindViewById<Button>(Resource.Id.gpsRequestButton);

            LinkEvents();
        }

        protected override void OnResume()
        {
            base.OnResume();
            
            gpsPoints.StartGettingLocationPoints();
            gpsText.Text += GetGpsPointText() + "\r\n";

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
            return "Longitude: " + gpsPoints.CurrentLongitude + "; Latitude: " + gpsPoints.CurrentLatitude + "; Accuracy: " + gpsPoints.CurrentAccuracy;
        }

        private void LinkEvents()
        {
            gpsShowBtn.Click += GpsShowBtn_Click;
        }

        private void GpsShowBtn_Click(object sender, EventArgs e)
        {
            gpsText.Text += GetGpsPointText() + "\r\n";
        }
    }
}

