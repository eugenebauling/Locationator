using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Locationator.Models;

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
using Newtonsoft.Json;

namespace Locationator.DAL
{
    public class WebService: IPositionRepo
    {
        private string tag;
        private Context _context;

        public WebService(Context context)
        {
            _context = context;
            tag = _context.GetText(Resource.String.TAG_DAL);
        }

        public void SaveLocationPointAsync(GpsPosition pos)
        {
            var client = new WebClient();
            client.UploadStringCompleted +=
                new UploadStringCompletedEventHandler(client_UploadStringCompleted);
            client.Headers[HttpRequestHeader.ContentType] = "application/json";
            client.Headers[HttpRequestHeader.Accept] = "application/json";
            var sourceUrl = new Uri(Constants.URL_SAVE_LOCATION_POINT);
            var data = JsonConvert.SerializeObject(pos);
            client.UploadStringAsync(sourceUrl, "POST", data);
        }

        private void client_UploadStringCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
                Log.Info(tag, _context.GetText(Resource.String.DAL_ERROR) + " " + e.Error.Message);
            else
                Log.Info(tag, _context.GetText(Resource.String.DAL_SUCCESS));
        }

        public void SaveLocationPoint(GpsPosition pos)
        {
            var data = JsonConvert.SerializeObject(pos);

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
                    Log.Info(tag, _context.GetText(Resource.String.DAL_ERROR_STATUS_CODE) + " " + response.StatusCode);
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var content = reader.ReadToEnd();
                    if (string.IsNullOrWhiteSpace(content))
                    {
                        Log.Info(tag, _context.GetText(Resource.String.DAL_EMPTY_BODY) + " " + response.StatusCode);
                    }
                    else
                    {
                        Log.Info(tag, _context.GetText(Resource.String.DAL_RESPONSE_BODY) + " \r\n" + content);
                    }
                }
            }
        }
    }
}