using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Locationator.Models;

namespace Locationator.DAL
{
    public interface IPositionRepo
    {
        void SaveLocationPointAsync(GpsPosition pos);
        void SaveLocationPoint(GpsPosition pos);
    }
}