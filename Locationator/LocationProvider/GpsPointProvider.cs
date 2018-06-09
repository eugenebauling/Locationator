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
using Locationator.Objects;

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

        public PointProviderStatus StartGettingLocationPoints()
        {
            if (Settings.GpsLocationMode == GpsPointCollectionMode.SystemSpecified)
            {
                return StartAutomaticSystemSelectedProvider();
            }
            else if (Settings.GpsLocationMode == GpsPointCollectionMode.Manual)
            {
                List<PointProviderStatus> statuses = new List<PointProviderStatus>();
                PointProviderStatus finalStatus = new PointProviderStatus();
                statuses = StartManualSystemSelectedProvider();

                foreach (var status in statuses)
                {
                    if (status.Started)
                    {
                        finalStatus.Started = true;
                    }

                    if (!String.IsNullOrEmpty(status.Error))
                    {
                        finalStatus.Error += status.Error + "/r/n";
                    }
                }

                return finalStatus;
            }

            PointProviderStatus incorrectSettingStatus = new PointProviderStatus();
            incorrectSettingStatus.Error = "Incorrect GPS Location Mode setting.";
            return incorrectSettingStatus;
        }

        private PointProviderStatus StartAutomaticSystemSelectedProvider()
        {
            PointProviderStatus status = new PointProviderStatus();
            try
            {
                string provider;
                Criteria locationCriteria = new Criteria();

                locationCriteria.Accuracy = Settings.GPSAutoAccuracy;
                locationCriteria.PowerRequirement = Settings.GPSAutoPower;

                provider = locMgr.GetBestProvider(locationCriteria, true);
                Log.Info(tag, "Provider: " + provider);
                if (provider != null)
                {
                    locMgr.RequestLocationUpdates(provider, Constants.GPS_LOCATION_UPDATE_INTERVAL_TIME, Constants.GPS_LOCATION_UPDATE_INTERVAL_DISTANCE_METRES, this);

                    status.Started = true;
                    status.Error = String.Empty;
                }
                else
                {
                    Log.Info(tag, "No location providers available");
                    status.Started = false;
                    status.Error = "No location providers available";
                }
            }
            catch (Exception ex)
            {
                status.Started = false;
                status.Error = ex.Message;
                return status;
            }
            return status;
        }

        private List<PointProviderStatus> StartManualSystemSelectedProvider()
        {
            List<PointProviderStatus> statuses = new List<PointProviderStatus>();
            try
            {
                if (Settings.UseGPS)
                {
                    PointProviderStatus gpsStatus = StartGPSProvider();
                    statuses.Add(gpsStatus);
                }

                if (Settings.UseNetwork)
                {
                    PointProviderStatus networkStatus = StartNetworkProvider();
                    statuses.Add(networkStatus);
                }

                if (Settings.UsePassive)
                {
                    PointProviderStatus passiveStatus = StartPassiveProvider();
                    statuses.Add(passiveStatus);
                }
            }
            catch (Exception ex)
            {
                PointProviderStatus manualProviderStatus = new PointProviderStatus();
                manualProviderStatus.Started = false;
                manualProviderStatus.Error = ex.Message;
                statuses.Add(manualProviderStatus);
                return statuses;
            }
            return statuses;
        }

        private PointProviderStatus StartGPSProvider()
        {
            PointProviderStatus status = new PointProviderStatus();
            try
            {
                locMgr.RequestLocationUpdates(LocationManager.GpsProvider, Constants.GPS_LOCATION_UPDATE_INTERVAL_TIME, Constants.GPS_LOCATION_UPDATE_INTERVAL_DISTANCE_METRES, this);

                status.Started = true;
                status.Error = String.Empty;
            }
            catch (Exception ex)
            {
                status.Started = false;
                status.Error = ex.Message;
                return status;
            }
            return status;
        }

        private PointProviderStatus StartNetworkProvider()
        {
            PointProviderStatus status = new PointProviderStatus();
            try
            {
                locMgr.RequestLocationUpdates(LocationManager.NetworkProvider, Constants.GPS_LOCATION_UPDATE_INTERVAL_TIME, Constants.GPS_LOCATION_UPDATE_INTERVAL_DISTANCE_METRES, this);

                status.Started = true;
                status.Error = String.Empty;
            }
            catch (Exception ex)
            {
                status.Started = false;
                status.Error = ex.Message;
                return status;
            }
            return status;
        }

        private PointProviderStatus StartPassiveProvider()
        {
            PointProviderStatus status = new PointProviderStatus();
            try
            {
                locMgr.RequestLocationUpdates(LocationManager.PassiveProvider, Constants.GPS_LOCATION_UPDATE_INTERVAL_TIME, Constants.GPS_LOCATION_UPDATE_INTERVAL_DISTANCE_METRES, this);

                status.Started = true;
                status.Error = String.Empty;
            }
            catch (Exception ex)
            {
                status.Started = false;
                status.Error = ex.Message;
                return status;
            }
            return status;
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