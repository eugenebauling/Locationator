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
using Locationator.Screens.Fragments;
using System.Threading.Tasks;
using Android;
using Android.Content.PM;
using Android.Support.V4.App;
using Locationator.Screens.Activities;

namespace Locationator
{
    [Activity(Label = "Locationator", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : AppCompatActivity
    {

        private string tag;
        private const int LocationPermissionsId = 1;
        DrawerLayout drawer;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            drawer = (DrawerLayout)FindViewById(Resource.Id.drawer_layout);

            Settings.SetDefaults();
            LocationUpdates.Init(this, (LocationManager)GetSystemService(Context.LocationService));

            tag = this.BaseContext.GetText(Resource.String.TAG_MAIN_ACTIVITY);

            //if (!IsTaskRoot && Intent.Categories.Contains(Intent.CategoryLauncher) && Intent.Action != null && Intent.Action == Intent.ActionMain)
            //{
            //    Intent resumeIntent = PackageManager.GetLaunchIntentForPackage(PackageName);
            //    resumeIntent.SetAction(Intent.ActionMain);
            //    resumeIntent.AddCategory(Intent.CategoryLauncher);
            //    StartActivity(resumeIntent);
            //}

            SetupMenuItems();
            ChangeMainContent();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public void ChangeMainContent()
        {
            LinearLayout main = FindViewById<LinearLayout>(Resource.Id.main_layout);
            var frag = new CoordinateLogFragment();
            var trans = FragmentManager.BeginTransaction();
            trans.Replace(Resource.Id.main_layout, frag);
            trans.Commit();
        }

        private void SetupMenuItems()
        {
            NavigationView navigationView = new NavigationView(this);
            DrawerLayout.LayoutParams navLayout = new DrawerLayout.LayoutParams(DrawerLayout.LayoutParams.WrapContent, DrawerLayout.LayoutParams.MatchParent);
            navLayout.Gravity = (int)GravityFlags.Start;
            navigationView.LayoutParameters = navLayout;
            navigationView.SetFitsSystemWindows(true);
            navigationView.ItemIconTintList = null;
            navigationView.InflateHeaderView(Resource.Layout.NavHeader);
            navigationView.InflateMenu(Resource.Menu.NavMenu);
            navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;

            DrawerLayout layout = (DrawerLayout)FindViewById(Resource.Id.drawer_layout);
            layout.AddView(navigationView);

            Toolbar toolbar = (Toolbar)FindViewById(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayShowTitleEnabled(true);
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.menu);
            SupportActionBar.SetTitle(Resource.String.ApplicationName);
        }

        private void NavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            // set item as selected to persist highlight
            e.MenuItem.SetChecked(true);
            // close drawer when item is tapped
            drawer.CloseDrawers();
            switch (e.MenuItem.ItemId)
            {
                case Resource.Id.nav_exit:
                    System.Environment.Exit(0);
                    break;
                case Resource.Id.nav_settings:
                    var intent = new Intent(this, typeof(SettingsActivity));
                    StartActivity(intent);
                    break;
                default:
                    break;
            }

            if (e.MenuItem.ItemId == Resource.Id.nav_exit)
            {
                System.Environment.Exit(0);
            }
 
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    drawer.OpenDrawer(Android.Support.V4.View.GravityCompat.Start);
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        public override void OnBackPressed()
        {
            MoveTaskToBack(false);
        }
    }
}

