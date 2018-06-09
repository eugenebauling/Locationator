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

namespace Locationator.DAL
{
    class NoDal : IPositionRepo
    {
        public void SaveLocationPoint(GpsPosition pos)
        {
            throw new NotImplementedException(); //intentional
        }

        public void SaveLocationPointAsync(GpsPosition pos)
        {
            throw new NotImplementedException(); //intentional
        }
    }
}