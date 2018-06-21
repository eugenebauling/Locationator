using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Locationator.LocationProvider;

namespace Locationator.Permissions
{
    public static class PermissionsCommunicator
    {

        private static IPermissionsResult request;
        private static Context context;

        public static void GetLocationPermissions(IPermissionsResult requestor, Context cont)
        {
            request = requestor;
            context = cont;

            //Check to see if any permission in our group is available, if one, then all are
            const string locationPermission = Manifest.Permission.AccessFineLocation;

            if (ActivityCompat.CheckSelfPermission(context, locationPermission) == (int)Permission.Granted)
            {
                request.PermissionCheckResult(true);
                return;
            }

            string[] Permissions =
                {
                  Manifest.Permission.AccessCoarseLocation,
                  Manifest.Permission.AccessFineLocation
                };


            var intent = new Intent(context, typeof(LocationPermissionActivity));
            intent.PutExtra("permissions", Permissions);
            intent.AddFlags(ActivityFlags.NewTask);
            context.StartActivity(intent);
        }

        public static void NotifyRequestor(bool granted)
        {
            if (request != null)
            {
                request.PermissionCheckResult(granted);
            }
        }
    }
}