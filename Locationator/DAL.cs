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
using System.Net;
using System.ComponentModel;
using Android.Util;
using System.IO;

namespace Locationator
{
    public class DAL: Java.Lang.Object
    {
        private const string tag = "DAL";

        public void SaveLocationPointAsync(string latitude, string longitude, int accuracy)
        {
            var client = new WebClient();
            client.UploadStringCompleted +=
                new UploadStringCompletedEventHandler(client_UploadStringCompleted);
            client.Headers[HttpRequestHeader.ContentType] = "application/json";
            client.Headers[HttpRequestHeader.Accept] = "application/json";
            var sourceUrl = new Uri(Constants.URL_SAVE_LOCATION_POINT);
            var data = "{ 'RecordedTime': '" + DateTime.UtcNow + "', 'Description': '" + Android.OS.Build.Model + " - " + Android.OS.Build.Serial + "', 'Longitude': '" + longitude + "', 'Latitude': '" + latitude + "', 'Accuracy': '" + accuracy + "' }";
            client.UploadStringAsync(sourceUrl, "POST", data);
        }

        private void client_UploadStringCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
                Log.Info(tag, "DAL Error: " + e.Error.Message);
            else
                Log.Info(tag, "DAL Success");
        }

        public void SaveLocationPoint(string latitude, string longitude, int accuracy)
        {
            var data = "{ 'RecordedTime': '" + DateTime.UtcNow + "', 'Description': '" + Android.OS.Build.Model + " - " + Android.OS.Build.Serial + "', 'Longitude': '" + longitude + "', 'Latitude': '" + latitude + "', 'Accuracy': '" + accuracy + "' }";
            var request = HttpWebRequest.Create(Constants.URL_SAVE_LOCATION_POINT);
            request.ContentType = "application/json";
            request.Method = "POST";
            byte[] byteArray = Encoding.UTF8.GetBytes(data);
            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/json";
            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    Log.Info(tag, "Error fetching data. Server returned status code: " + response.StatusCode);
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var content = reader.ReadToEnd();
                    if (string.IsNullOrWhiteSpace(content))
                    {
                        Log.Info(tag, "Response Contained empty body: " + response.StatusCode);
                    }
                    else
                    {
                        Log.Info(tag, "Response Body: \r\n" + content);
                    }
                }
            }
        }
    }
}