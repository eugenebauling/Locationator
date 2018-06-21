using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Locationator.Models;
using Locationator.Objects;

namespace Locationator.LocationProvider
{
    public interface ILocationSubscriber
    {
        int GetSubscriberId();
        void OnPositionChanged(GpsPosition position);
        void OnPositionError(PointProviderStatus position);
    }
}