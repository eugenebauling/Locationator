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
using Android.Content.Res;

namespace Locationator
{
    public class DAL: Java.Lang.Object
    {
        private string tag;
        private Context context;

        public DAL(Context _context)
        {
            tag = _context.GetText(Resource.String.TAG_DAL);
            context = _context;
        }

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
                Log.Info(tag, context.GetText(Resource.String.DAL_SUCCESS) + " " + e.Error.Message);
            else
                Log.Info(tag, context.GetText(Resource.String.DAL_SUCCESS));
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
                    Log.Info(tag, context.GetText(Resource.String.DAL_ERROR_STATUS_CODE) + " " + response.StatusCode);
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var content = reader.ReadToEnd();
                    if (string.IsNullOrWhiteSpace(content))
                    {
                        Log.Info(tag, context.GetText(Resource.String.DAL_EMPTY_BODY) + " " + response.StatusCode);
                    }
                    else
                    {
                        Log.Info(tag, context.GetText(Resource.String.DAL_RESPONSE_BODY) + " \r\n" + content);
                    }
                }
            }
        }
    }
}