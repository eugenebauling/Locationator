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
using Locationator.LocationProvider;
using Locationator.Models;

namespace Locationator.Fragments
{
    public class CoordinateLogFragment : Fragment, ILocationSubscriber
    {
        #region Controls

        TextView gpsText;
        Button gpsShowBtn;
        private string tag;

        #endregion

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            
            View v = inflater.Inflate(Resource.Layout.CoordinateLog, null, false);

            gpsText = v.FindViewById<TextView>(Resource.Id.gpsText);
            gpsShowBtn = v.FindViewById<Button>(Resource.Id.gpsRequestButton);
            tag = this.Context.GetText(Resource.String.TAG_COORD_LOG);
            LinkBtnEvents();
            LocationUpdates.Subscribe(this);

            return v;
        }

        private void LinkBtnEvents()
        {
            gpsShowBtn.Click += GpsShowBtn_Click;
        }

        private void GpsShowBtn_Click(object sender, EventArgs e)
        {
            //gpsText.Text += GetGpsPointText();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        //private string GetGpsPointText()
        //{
        //    StringBuilder builder = new StringBuilder();
        //    string msg = Application.Context.Resources.GetString(Resource.String.TAG_POSITION);

        //    return builder.AppendFormat(msg, gpsPoints.CurrentLongitude, gpsPoints.CurrentLatitude, gpsPoints.CurrentAccuracy).ToString() + "\r\n";
        //}

        public override void OnResume()
        {
            base.OnResume();

            //Log.Info(tag, GetGpsPointText());
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
    }
}