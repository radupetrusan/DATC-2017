using Android.App;
using Android.Widget;
using Android.OS;
using Android.Gms.Maps;
using System;
using Android.Gms.Maps.Model;
using System.Drawing;

namespace DATC
{
    [Activity(Label = "DATC", MainLauncher = true)]
    public class MainActivity : Activity,IOnMapReadyCallback
    {
        GoogleMap mMap;

        public void OnMapReady(GoogleMap googleMap)
        {
            LatLng markerlatLng = new LatLng(45.740363, 21.244295);
            mMap = googleMap;
            CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(markerlatLng,17);
            mMap.MoveCamera(camera);
            MarkerOptions markerOptions = new MarkerOptions();
            markerOptions.SetPosition(markerlatLng);
            markerOptions.SetTitle("Senzor 1");
            mMap.AddMarker(markerOptions);
            CircleOptions circleOptions = new CircleOptions();
            LatLng mcirclelatLng = new LatLng(45.740363, 21.245455);
            circleOptions.InvokeCenter(mcirclelatLng);
            circleOptions.InvokeFillColor(Color.Green.ToArgb());
            circleOptions.InvokeRadius(10);
            mMap.AddCircle(circleOptions);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            SetupMap();
        }

        private void SetupMap()
        {
            if (mMap == null)
            {
                FragmentManager.FindFragmentById<MapFragment>(Resource.Id.map).GetMapAsync(this);
            }
        }
    }
}

