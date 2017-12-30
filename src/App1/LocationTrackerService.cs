using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Plugin.Geolocator.Abstractions;

namespace App1
{

    public class LocationListener
    {
        private readonly Func<IGeolocator> _geoLocatorFactory;

        public LocationListener(Func<IGeolocator> geoLocatorFactory)
        {
            _geoLocatorFactory = geoLocatorFactory;
            StartListening(_geoLocatorFactory());
        }

        public void StartReporting()
        {

        }

        public void StopReporting()
        {

        }

        private void StartListening(IGeolocator geolocator)
        {
            geolocator.PositionChanged += Current_PositionChanged;
            geolocator.StartListeningAsync(new TimeSpan(0, 1, 0), 0, false, new ListenerSettings() { AllowBackgroundUpdates = true });
        }

        private void Current_PositionChanged(object sender, PositionEventArgs e)
        {
            ReportAsync(e.Position);
        }

        private async Task ReportAsync(Position pos)
        {
            // Create a request using a URL that can receive a post.   
            WebRequest request = WebRequest.Create("http://169.254.80.80:5002/api/LocationTracker");
            // Set the Method property of the request to POST.  
            request.Method = "POST";
            var posJson = JsonConvert.SerializeObject(pos);

            byte[] byteArray = Encoding.UTF8.GetBytes(posJson);
            //// Set the ContentType property of the WebRequest.  
            request.ContentType = "text/json";
            //// Set the ContentLength property of the WebRequest.  
            request.ContentLength = byteArray.Length;
            //// Get the request stream.

            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(byteArray, 0, byteArray.Length);
            }

            using (var response = request.GetResponse())
            {

                Console.WriteLine(((HttpWebResponse)response).StatusDescription);

                //using (var responseStream = response.GetResponseStream())
                //{
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    string responseFromServer = reader.ReadToEnd();
                    //// Display the content.  
                    Console.WriteLine(responseFromServer);
                }
                //// Read the content.  
                //string responseFromServer = reader.ReadToEnd();
                //// Display the content.  
                //Console.WriteLine(responseFromServer);
                //// }
            }

            //// Write the data to the request stream.  

            //// Close the Stream object.  
            //dataStream.Close();
            //// Get the response.  
            //WebResponse response = request.GetResponse();
            //// Display the status.  
            //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            //// Get the stream containing content returned by the server.  
            //dataStream = response.GetResponseStream();
            //// Open the stream using a StreamReader for easy access.  
            //StreamReader reader = new StreamReader(dataStream);
            //// Read the content.  
            //string responseFromServer = reader.ReadToEnd();
            //// Display the content.  
            //Console.WriteLine(responseFromServer);
            //// Clean up the streams.  
            //reader.Close();
            //dataStream.Close();
            //response.Close();
        }
    }

    // todo: split these out
    [Service]
    public class LocationReporterService : Service
    {

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        private LocationListener _reporter;

        //public LocationTrackerService() : base("LocationTrackerService")
        //{


        //}

        public override void OnCreate()
        {
            base.OnCreate();

            IGeolocator geoLocatorFactory() => global::Plugin.Geolocator.CrossGeolocator.Current;
            _reporter = new LocationListener(geoLocatorFactory);

        }


    }
}