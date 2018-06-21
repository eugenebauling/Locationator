using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V7;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace Locationator.Permissions
{
    [Activity(Label = "LocationPermissionActivity")]
    public class LocationPermissionActivity : AppCompatActivity
    {
        const int requestorID = 1;
        string[] permissions;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            permissions = Intent.Extras.GetStringArray("permissions");

            ActivityCompat.RequestPermissions(this, permissions, requestorID);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            bool granted = (grantResults[0] == Permission.Granted);

            if (requestCode == requestorID)
            {
                PermissionsCommunicator.NotifyRequestor(granted);
            }

            Finish();
        }
    }
}