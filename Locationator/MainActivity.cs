using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Widget;
using Android.OS;
using Android.Locations;
using Android.Util;
using Locationator.LocationProvider;
using System.Text;
using Android.Support.V4.Widget;
using Android.Graphics;
using Android.Views;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;
using Locationator.Fragments;
using System.Threading.Tasks;
using Android;
using Android.Content.PM;
using Android.Support.V4.App;

namespace Locationator
{
    [Activity(Label = "Locationator", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : AppCompatActivity
    {

        private string tag;
        private const int RequestLocationId = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            Settings.SetDefaults();
            GetLocationPermissionAsync();
            

            tag = this.BaseContext.GetText(Resource.String.TAG_MAIN_ACTIVITY);

            SetupMenuItems();
            ChangeMainContent();

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        private void SetupMenuItems()
        {
            NavigationView navigationView = new NavigationView(this);
            DrawerLayout.LayoutParams navLayout = new DrawerLayout.LayoutParams(DrawerLayout.LayoutParams.WrapContent, DrawerLayout.LayoutParams.MatchParent);
            navLayout.Gravity = (int)GravityFlags.Start;
            navigationView.LayoutParameters = navLayout;
            navigationView.SetFitsSystemWindows(true);
            navigationView.InflateHeaderView(Resource.Layout.nav_header);
            navigationView.InflateMenu(Resource.Menu.drawer_view);

            DrawerLayout layout = (DrawerLayout)FindViewById(Resource.Id.drawer_layout);
            layout.AddView(navigationView);

            Toolbar toolbar = (Toolbar)FindViewById(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayShowTitleEnabled(true);
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);
            SupportActionBar.SetTitle(Resource.String.ApplicationName);
        }

        public void ChangeMainContent()
        {
            LinearLayout main = FindViewById<LinearLayout>(Resource.Id.main_layout);
            var frag = new CoordinateLogFragment();
            var trans = FragmentManager.BeginTransaction();
            trans.Replace(Resource.Id.main_layout, frag);
            trans.Commit();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    DrawerLayout layout = (DrawerLayout)FindViewById(Resource.Id.drawer_layout);
                    layout.OpenDrawer(Android.Support.V4.View.GravityCompat.Start);
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        private void GetLocationPermissionAsync()
        {
            //Check to see if any permission in our group is available, if one, then all are
            const string permission = Manifest.Permission.AccessFineLocation;
            if (CheckSelfPermission(permission) == (int)Permission.Granted)
            {
                LocationUpdates.Init(this.BaseContext, (LocationManager)GetSystemService(Context.LocationService), true);
                return;
            }

            string[] PermissionsLocation =
                {
                  Manifest.Permission.AccessCoarseLocation,
                  Manifest.Permission.AccessFineLocation
                };
            
            //Finally request permissions with the list of permissions and Id
            ActivityCompat.RequestPermissions(this, PermissionsLocation, RequestLocationId);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            switch (requestCode)
            {
                case RequestLocationId:
                    {
                        if (grantResults[0] == Permission.Granted)
                        {
                            LocationUpdates.Init(this.BaseContext, (LocationManager)GetSystemService(Context.LocationService), true);
                        }
                        else
                        {
                            LocationUpdates.Init(this.BaseContext, (LocationManager)GetSystemService(Context.LocationService), false);
                        }
                    }
                    break;
            }
        }
    }
}

