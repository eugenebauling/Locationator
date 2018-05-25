using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Locationator.Models;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Locations;
using Android.Util;
using Locationator.Enums;
using Locationator.DAL;

namespace Locationator.LocationProvider
{

    public class GpsPointProvider: Java.Lang.Object, ILocationListener
    {
        private LocationManager locMgr;
        private double currentLong = 0.0;
        private double currentLat = 0.0;
        private float currentAccuracy = 0;
        private readonly string tag;
        private IPositionRepo repo;

        public double CurrentLatitude { get { return currentLat; } }
        public double CurrentLongitude { get { return currentLong; } }
        public float CurrentAccuracy { get { return currentAccuracy; } }

        public GpsPointProvider(Context _context, LocationManager _locMgr)
        {
            tag = _context.GetText(Resource.String.TAG_GPS_POINT_PROVIDER);
            locMgr = _locMgr;
            repo = RepoManager.GetPositionRepo().Instance(_context);
            Log.Info(tag, "Location Manager " + locMgr);
        }

        public void StartGettingLocationPoints()
        {
            string provider;

            #region Request Location Way 1

            if (Settings.GpsLocationMode == GpsPointCollectionMode.SystemSpecified)
            {
                Criteria locationCriteria = new Criteria();

                locationCriteria.Accuracy = Accuracy.Fine;
                locationCriteria.PowerRequirement = Power.High;

                provider = locMgr.GetBestProvider(locationCriteria, true);
                Log.Info(tag, "Provider: " + provider);
                if (provider != null)
                {
                    locMgr.RequestLocationUpdates(provider, Constants.GPS_LOCATION_UPDATE_INTERVAL_TIME, Constants.GPS_LOCATION_UPDATE_INTERVAL_DISTANCE_METRES, this);
                }
                else
                {
                    Log.Info(tag, "No location providers available");
                }
            }

            #endregion

            #region Request Location Way 2

            if (Settings.GpsLocationMode == GpsPointCollectionMode.Manual)
            {

                locMgr.RequestLocationUpdates(LocationManager.GpsProvider, Constants.GPS_LOCATION_UPDATE_INTERVAL_TIME, Constants.GPS_LOCATION_UPDATE_INTERVAL_DISTANCE_METRES, this);
                locMgr.RequestLocationUpdates(LocationManager.NetworkProvider, Constants.GPS_LOCATION_UPDATE_INTERVAL_TIME, Constants.GPS_LOCATION_UPDATE_INTERVAL_DISTANCE_METRES, this);
                locMgr.RequestLocationUpdates(LocationManager.PassiveProvider, Constants.GPS_LOCATION_UPDATE_INTERVAL_TIME, Constants.GPS_LOCATION_UPDATE_INTERVAL_DISTANCE_METRES, this);

            }
            #endregion
        }

        public void StopGettingLocationPoints()
        {
            locMgr.RemoveUpdates(this);
        }

        public void OnLocationChanged(Location location)
        {
            if (currentLat != location.Latitude || currentLong != location.Longitude)
            {
                currentLat = location.Latitude;
                currentLong = location.Longitude;
                if (location.HasAccuracy)
                    currentAccuracy = location.Accuracy;

                repo.SaveLocationPoint(new GpsPosition(currentLong, currentLat, currentAccuracy));

                Log.Info(tag, "Longitude: " + currentLong + "; Latitude: " + currentLat + "; Accuracy: " + currentAccuracy);
            }
        }

        public void OnProviderDisabled(string provider)
        {
            StopGettingLocationPoints();
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