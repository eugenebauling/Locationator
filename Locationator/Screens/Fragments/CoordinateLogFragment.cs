using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Locationator.DAL;
using Locationator.LocationProvider;
using Locationator.Models;
using Locationator.Objects;

namespace Locationator.Screens.Fragments
{
    public class CoordinateLogFragment : Fragment, ILocationSubscriber
    {
        #region Controls

        TextView gpsText;
        private string tag;
        const int subscriberId = 1;

        #endregion

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ILocationSubscriber repo = (PositionWebService)RepoManager.GetPositionRepo().Instance(this.Activity);

            LocationUpdates.Subscribe(this);
            LocationUpdates.Subscribe(repo);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            
            View v = inflater.Inflate(Resource.Layout.CoordinateLog, null, false);
            gpsText = v.FindViewById<TextView>(Resource.Id.gpsText);
            tag = this.Activity.GetText(Resource.String.TAG_COORD_LOG);
            return v;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public override void OnResume()
        {
            base.OnResume();
        }

        public override void OnPause()
        {
            base.OnPause();
        }

        public void OnPositionChanged(GpsPosition position)
        {
            StringBuilder builder = new StringBuilder();
            string msg = Application.Context.Resources.GetString(Resource.String.TAG_POSITION);
            gpsText.Text += builder.AppendFormat(msg, position.Long, position.Lat, position.Accuracy).ToString() + "\r\n";
        }

        public void OnPositionError(PointProviderStatus position)
        {
            string msg = Application.Context.Resources.GetString(Resource.String.ERR_GENERAL) + " : " + position.Error;
            gpsText.Text += msg + "\r\n";
        }

        public int GetSubscriberId()
        {
            return subscriberId;
        }
    }
}