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
using OxyPlot.Xamarin.Android;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace DATC
{
    [Activity(Label = "Date senzor")]
    public class SenzorActivity : Activity
    {
        ToggleButton btnTempUmid;
        PlotView pltSenzor;
        public static List<int> dateSenzor = new List<int>();
        string axisTitle;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DateSenzor);
            btnTempUmid = FindViewById<ToggleButton>(Resource.Id.btnTempUmid);
            btnTempUmid.Click += BtnTempUmid_Click;
            if (Helper.vizualizareaCurenta == Helper.Vizualizare.Temperatura)
            {
                btnTempUmid.Checked = false;
                axisTitle = "Temperatura (C)";
            }
            else
            {
                btnTempUmid.Checked = true;
                axisTitle = "Umiditatea (%)";
            }
            btnTempUmid.Text += " " + Helper.senzorCurent;
            pltSenzor = FindViewById<PlotView>(Resource.Id.pltSenzor);
            dateSenzor.Add(34);
            dateSenzor.Add(32);
            dateSenzor.Add(36);
            ActualizareGrafic();
        }

        void ActualizareGrafic()
        {
                pltSenzor.Model = CreatePlotModel();
        }

        private PlotModel CreatePlotModel()
        {
            LineSeries series1 = new LineSeries();
            PlotModel plotModel = new PlotModel { Title = "Date" };
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Maximum = 50, Minimum = -20, MajorGridlineColor = OxyColors.White,Title= axisTitle, TitleColor = OxyColor.FromRgb(255, 255, 255) });
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, MajorGridlineColor = OxyColors.White, Title ="Timp (s)", TitleColor= OxyColor.FromRgb(255,255,255)});
            for (int i = 0; i < dateSenzor.Count; i++)
                series1.Points.Add(new DataPoint(0.1 * i, dateSenzor[i]));
            plotModel.DefaultColors = new List<OxyColor> { OxyColors.White };
            plotModel.Series.Add(series1);
            return plotModel;
        }

        private void BtnTempUmid_Click(object sender, EventArgs e)
        {
            btnTempUmid.Text += " " + Helper.senzorCurent;
            if (btnTempUmid.Checked)
            {
                //Umiditate
                Helper.vizualizareaCurenta=Helper.Vizualizare.Umiditate;
                axisTitle = "Umiditate (%)";
                ActualizareGrafic();
            }
            else
            {
                //Temperatura
                Helper.vizualizareaCurenta = Helper.Vizualizare.Temperatura;
                axisTitle = "Temperatura (C)";
                ActualizareGrafic();
            }
        }
    }
}