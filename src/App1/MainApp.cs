using System;

using Android.App;
using Android.Content;
using Android.Runtime;

namespace App1
{
    [Application]
    class MainApp : Application
    {
        protected MainApp(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {

        }

        private void Current_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {

            var pos = e.Position;
           // throw new NotImplementedException();
        }

        public override void OnCreate()
        {
            base.OnCreate();

            StartLocationTrackerService();
           
        }

        private void StartLocationTrackerService()
        {
            Intent locationReporterService = new Intent(this, typeof(LocationReporterService));
            Context.StartService(locationReporterService);


        }
    }
}