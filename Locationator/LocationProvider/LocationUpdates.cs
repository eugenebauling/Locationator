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
using Locationator.DAL;
using Locationator.Models;
using Locationator.Objects;

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

        public static void Init(Context _context, LocationManager _locMgr)
        {
            context = _context;
            locMgr = _locMgr;
            gpsPoints = new GpsPointProvider(_context, _locMgr);
            publisherId = gpsPoints.PublisherId;
        }

        private static void StartUpdates()
        {
                gpsPoints.StartGettingLocationPoints();
                started = true;
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

        public static void PublishError(PointProviderStatus status, Guid requestedPublisherId)
        {
            if (requestedPublisherId == publisherId)
            {
                foreach (ILocationSubscriber sub in subscribers)
                {
                    sub.OnPositionError(status);
                }
            }
        }

        public static void Subscribe(ILocationSubscriber subscriber)
        {
            if (subscribers.Where(x => x.GetSubscriberId() == subscriber.GetSubscriberId()).Any() == false)
            {
                subscribers.Add(subscriber);

                if (!started)
                {
                    StartUpdates();
                }
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

        public static void Restart()
        {
            StopUpdates();
            StartUpdates();
        }
    }
}