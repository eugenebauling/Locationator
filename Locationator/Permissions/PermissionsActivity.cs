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
    [Activity(Label = "PermissionsActivity")]
    public class PermissionsActivity : Activity
    {

        IPermissionsCheck permitModule;
        Context context;

        private const int LocationPermissionsId = 1;

        public PermissionsActivity()
        {

        }

        public PermissionsActivity(IPermissionsCheck requestor, Context baseContext)
        {
            permitModule = requestor;
            context = baseContext;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public void GetLocationPermissions()
        {
            //Check to see if any permission in our group is available, if one, then all are
            const string locationPermission = Manifest.Permission.AccessFineLocation;

            if (context.CheckSelfPermission(locationPermission) == (int)Permission.Granted)
            {
                permitModule.PermissionCheckResult(true);
                return;
            }

            string[] Permissions =
                {
                  Manifest.Permission.AccessCoarseLocation,
                  Manifest.Permission.AccessFineLocation
                };
            ActivityCompat.RequestPermissions(this, Permissions, LocationPermissionsId);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            switch (requestCode)
            {
                case LocationPermissionsId:
                    {
                        if (grantResults[0] == Permission.Granted)
                        {
                            permitModule.PermissionCheckResult(true);
                        }
                        else
                        {
                            permitModule.PermissionCheckResult(false);
                        }
                    }
                    break;
            }
        }

    }
}