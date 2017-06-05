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
using Android.Locations;
using Android.Util;

namespace Locationator
{

    public class GPSPointProvider: Java.Lang.Object, ILocationListener
    {
        private LocationManager locMgr;
        private double currentLong = 0.0;
        private double currentLat = 0.0;
        private const string tag = "GPSPointProvider";

        public double CurrentLatitude { get { return currentLat; } }
        public double CurrentLongitude { get { return currentLong; } }

        public GPSPointProvider(LocationManager _locMgr)
        {
            locMgr = _locMgr;
            Log.Info(tag, "Location Manager " + locMgr);
        }

        public void StartGettingLocationPoints()
        {
            //string provider;

            //#region Request Location Way 1

            //Criteria locationCriteria = new Criteria();

            //locationCriteria.Accuracy = Accuracy.Fine;
            //locationCriteria.PowerRequirement = Power.High;

            //provider = locMgr.GetBestProvider(locationCriteria, true);
            //Log.Info(tag, "Provider: " + provider);
            //if (provider != null)
            //{
            //    locMgr.RequestLocationUpdates(provider, Constants.GPS_LOCATION_UPDATE_INTERVAL_TIME, Constants.GPS_LOCATION_UPDATE_INTERVAL_DISTANCE_METRES, this);
            //}
            //else
            //{
            //    Log.Info(tag, "No location providers available");
            //}

            //#endregion

            #region Request Location Way 2

            //if (locMgr.IsProviderEnabled(provider))
            //{
                locMgr.RequestLocationUpdates(LocationManager.GpsProvider, Constants.GPS_LOCATION_UPDATE_INTERVAL_TIME, Constants.GPS_LOCATION_UPDATE_INTERVAL_DISTANCE_METRES, this);
                locMgr.RequestLocationUpdates(LocationManager.NetworkProvider, Constants.GPS_LOCATION_UPDATE_INTERVAL_TIME, Constants.GPS_LOCATION_UPDATE_INTERVAL_DISTANCE_METRES, this);
                locMgr.RequestLocationUpdates(LocationManager.PassiveProvider, Constants.GPS_LOCATION_UPDATE_INTERVAL_TIME, Constants.GPS_LOCATION_UPDATE_INTERVAL_DISTANCE_METRES, this);
            //}
            //else
            //{
            //    Log.Info(tag, provider + " is not available. Does the device have location services enabled?");
            //}

            #endregion
        }

        public void StopGettingLocationPoints()
        {
            locMgr.RemoveUpdates(this);
        }

        public void OnLocationChanged(Location location)
        {
            currentLat = location.Latitude;
            currentLong = location.Longitude;
            Log.Info(tag, "Longitude: " + currentLong + "; Latitude: " + currentLat);
        }

        public void OnProviderDisabled(string provider)
        {
            StartGettingLocationPoints();
        }

        public void OnProviderEnabled(string provider)
        {
            StartGettingLocationPoints();
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            if (status == Availability.Available)
                StartGettingLocationPoints();
            else
                StopGettingLocationPoints();
        }
    }
}