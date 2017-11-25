using Android.App;
using Android.Widget;
using Android.OS;
using Android.Gms.Maps;
using System;
using Android.Gms.Maps.Model;
using System.Drawing;
using System.Net.Http;
using System.Text;

namespace DATC
{
    [Activity(Label = "DATC", MainLauncher = true)]
    public class MainActivity : Activity,IOnMapReadyCallback
    {
        static HttpClient client = new HttpClient();
        GoogleMap mMap;
        ToggleButton btnTempOrUmid;
        public void OnMapReady(GoogleMap googleMap)
        {
            LatLng markerlatLng = new LatLng(45.740363, 21.244295);
            mMap = googleMap;
            CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(markerlatLng,17);
            mMap.MoveCamera(camera);
            Helper.AdaugareMarker(mMap, markerlatLng, "Senzor 1");
            Helper.AdaugareMarker(mMap, new LatLng(45.740303, 21.244295), "Senzor 2");
            Helper.AdaugareMarker(mMap, new LatLng(45.740493, 21.244295), "Senzor 3");
            mMap.MarkerClick += MMap_MarkerClick;
        }

        private void MMap_MarkerClick(object sender, GoogleMap.MarkerClickEventArgs e)
        {
            Helper.senzorCurent = e.Marker.Title;
            StartActivity(typeof(SenzorActivity));
        }

        protected override void OnResume()
        {
            base.OnResume();
            if(Helper.vizualizareaCurenta==Helper.Vizualizare.Temperatura)
            {
                btnTempOrUmid.Checked = false;
            }
            else
            {
                btnTempOrUmid.Checked = true;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            btnTempOrUmid = FindViewById<ToggleButton>(Resource.Id.btnTempOrUmid);
            SetupMap();
            btnTempOrUmid.Click += BtnTempOrUmid_Click;
        }
        
        private void BtnTempOrUmid_Click(object sender, EventArgs e)
        {
            if(btnTempOrUmid.Checked)
            {
                //Umiditate
                Helper.vizualizareaCurenta = Helper.Vizualizare.Umiditate;
                /*GET from API
                 * client.DefaultRequestHeaders.Add("Accept", "application/hal+json");
                var Home = "http://localhost:50922/api/values";
                var response = client.GetAsync(Home).Result;
                string data = response.Content.ReadAsStringAsync().Result;
                */
            }
            else
            {
                //Temperatura
                Helper.vizualizareaCurenta = Helper.Vizualizare.Temperatura;
            }
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

