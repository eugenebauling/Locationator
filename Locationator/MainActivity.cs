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

namespace Locationator
{
    [Activity(Label = "Locationator", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : AppCompatActivity
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
            SetupMenuItems();

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
            StringBuilder builder = new StringBuilder();
            string msg = Application.Context.Resources.GetString(Resource.String.TAG_POSITION);
   
            return builder.AppendFormat(msg, gpsPoints.CurrentLongitude, gpsPoints.CurrentLatitude, gpsPoints.CurrentAccuracy).ToString() + "\r\n";
        }

        private void LinkBtnEvents()
        {
            gpsShowBtn.Click += GpsShowBtn_Click;
        }

        private void GpsShowBtn_Click(object sender, EventArgs e)
        {
            gpsText.Text += GetGpsPointText();
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
    }
}

