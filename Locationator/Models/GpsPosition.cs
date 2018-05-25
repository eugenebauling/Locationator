using Android.Locations;
using System;
using Android.OS;

namespace Locationator.Models
{
    public class GpsPosition : Java.Lang.Object
    {
        public string Long { get; }
        public string Lat { get; }
        public string Description { get; }
        public string Accuracy { get; }
        public string RecordedTime { get; }

        public GpsPosition(double longitude, double latitude, float accuracy)
        {
            Long = FormatCoordinate(longitude);
            Lat = FormatCoordinate(latitude);
            Accuracy = accuracy.ToString("0.00");
            Description = Build.Model + " - " + Build.Serial;
            RecordedTime = DateTime.UtcNow.ToString();
        }

        private string FormatCoordinate(double coord)
        {
            return Location.Convert(coord, Format.Degrees).Replace(',', '.');
        }
    }
}