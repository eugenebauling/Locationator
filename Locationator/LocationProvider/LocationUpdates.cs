using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Locationator.Models;

namespace Locationator.LocationProvider
{

    public static class LocationUpdates
    {
        private static LocationManager locMgr;
        private static GpsPointProvider gpsPoints;
        private static Context context;
        private static List<ILocationSubscriber> subscribers = new List<ILocationSubscriber>();
        private static bool started = false;
        private static Guid publisherId;
        private static bool permission;

        public static void Init(Context _context, LocationManager _locMgr, bool permissionGranted)
        {
            context = _context;
            locMgr = _locMgr;
            gpsPoints = new GpsPointProvider(_context, _locMgr);
            publisherId = gpsPoints.PublisherId;
            permission = permissionGranted;
            if (permission && subscribers.Count > 0)
            {
                StartUpdates();
            }
        }

        private static void StartUpdates()
        {
            if (permission)
            {
                gpsPoints.StartGettingLocationPoints();
                started = true;
            }
        }

        private static void StopUpdates()
        {
            gpsPoints.StopGettingLocationPoints();
            started = false;
        }

        public static void Publish(GpsPosition position, Guid requestedPublisherId)
        {
            if (requestedPublisherId == publisherId)
            {
                foreach (ILocationSubscriber sub in subscribers)
                {
                    sub.OnPositionChanged(position);
                }
            }
        }

        public static void Subscribe(ILocationSubscriber subscriber)
        {
            subscribers.Add(subscriber);

            if (!started)
            {
                StartUpdates();
            }
        }

        public static void Unsubscribe(ILocationSubscriber subscriber)
        {
            int i = subscribers.IndexOf(subscriber);
            if (i > -1)
            {
                subscribers.RemoveAt(i);
            }
            if (subscribers.Count == 0)
            {
                StopUpdates();
            }
        }
    }
}