﻿using System;
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
using Android.Content.Res;
using Locationator.Permissions;

namespace Locationator.LocationProvider
{

    public class GpsPointProvider : Java.Lang.Object, ILocationListener, IPermissionsResult
    {
        private LocationManager locMgr;
        private Context context;
        private double currentLong = 0.0;
        private double currentLat = 0.0;
        private float currentAccuracy = 0;
        private readonly string tag;
        private Guid publisherId;

        public double CurrentLatitude { get { return currentLat; } }
        public double CurrentLongitude { get { return currentLong; } }
        public float CurrentAccuracy { get { return currentAccuracy; } }
        public Guid PublisherId { get { return publisherId; } }

        public GpsPointProvider(Context _context, LocationManager _locMgr)
        {
            tag = _context.GetText(Resource.String.TAG_GPS_POINT_PROVIDER);
            locMgr = _locMgr;
            context = _context;
            publisherId = Guid.NewGuid();
            Log.Info(tag, "Location Manager " + locMgr);

        }

        public void StartGettingLocationPoints()
        {
            CheckIfDeviceHasLocationTurnedOn();
        }

        private PointProviderStatus StartWithPermissionsGranted()
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
                    locMgr.RequestLocationUpdates(provider, Settings.GpsLocationUpdateIntervalMilliseconds, Settings.GpsLocationUpdateIntervalMetres, this);

                    status.Started = true;
                    status.Error = String.Empty;
                }
                else
                {
                    Log.Info(tag, Application.Context.Resources.GetString(Resource.String.ERR_GENERAL));
                    status.Started = false;
                    status.Error = Application.Context.Resources.GetString(Resource.String.ERR_GENERAL);
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
                locMgr.RequestLocationUpdates(LocationManager.GpsProvider, Settings.GpsLocationUpdateIntervalMilliseconds, Settings.GpsLocationUpdateIntervalMetres, this);

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
                locMgr.RequestLocationUpdates(LocationManager.NetworkProvider, Settings.GpsLocationUpdateIntervalMilliseconds, Settings.GpsLocationUpdateIntervalMetres, this);

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
                locMgr.RequestLocationUpdates(LocationManager.PassiveProvider, Settings.GpsLocationUpdateIntervalMilliseconds, Settings.GpsLocationUpdateIntervalMetres, this);

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

                LocationUpdates.Publish(new GpsPosition(currentLong, currentLat, currentAccuracy), publisherId);

                StringBuilder builder = new StringBuilder();
                string msg = Application.Context.Resources.GetString(Resource.String.TAG_POSITION);
                Log.Info(tag, builder.AppendFormat(msg, currentLong, currentLat, currentAccuracy).ToString());
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
            if (status == Availability.Available || status == Availability.TemporarilyUnavailable)
                StartGettingLocationPoints();
            else
                StopGettingLocationPoints();
        }

        public void CheckPermissions()
        {
            PermissionsCommunicator.GetLocationPermissions(this, context);
        }

        public void PermissionCheckResult(bool permissionGranted)
        {
            if (permissionGranted)
            {
                PointProviderStatus result = StartWithPermissionsGranted();
                if (!result.Started)
                {
                    LocationUpdates.PublishError(result, publisherId);
                }
            }
        }

        private void CheckIfDeviceHasLocationTurnedOn()
        {
            if (!locMgr.IsProviderEnabled(LocationManager.GpsProvider) && !locMgr.IsProviderEnabled(LocationManager.NetworkProvider))
            {
                // Build the alert dialog
                AlertDialog.Builder builder = new AlertDialog.Builder(context);
                builder.SetTitle(Application.Context.Resources.GetString(Resource.String.TEXT_NO_LOCATION_SERVICE));
                builder.SetMessage(Application.Context.Resources.GetString(Resource.String.TEXT_ENABLE_LOCATION_SERVICE));
                builder.SetPositiveButton(Application.Context.Resources.GetString(Resource.String.BTN_OK), Ack);
                Dialog alertDialog = builder.Create();
                alertDialog.SetCanceledOnTouchOutside(false);
                alertDialog.Show();
            }
            else
            {
                CheckPermissions();
            }
        }
        void Ack(object sender, DialogClickEventArgs e)
        {
            Intent intent = new Intent(Android.Provider.Settings.ActionLocationSourceSettings);
            context.StartActivity(intent);
        }
    }
}