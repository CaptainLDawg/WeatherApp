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
using Geolocator;
using Geolocator.Plugin;
using Geolocator.Plugin.Abstractions;
using AndroidHUD;

namespace WeatherApp
{
    [Activity(Label = "OptionsActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class OptionsActivity : Activity
    {
        List<ListOOptions> listDefaultCity;
        List<ListOTempertures> listDefaultTemp;
        DatabaseManager objDb;
        RESThandler objRest;

        Button btnBack;
        Button btnCelsius;
        Button btnFarenheit;

        Button btnTrackGPS;
        Button btnAuckland;
        Button btnHamilton;
        Button btnChrist;
        Button btnWellington;

        string CityName;
        string TempName;

        double longitude;
        double latitude;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Options);

            btnCelsius = FindViewById<Button>(Resource.Id.btnCelsius);
            btnFarenheit = FindViewById<Button>(Resource.Id.btnFaren);

            btnTrackGPS = FindViewById<Button>(Resource.Id.btnTrackGPS);
            btnAuckland = FindViewById<Button>(Resource.Id.btnAuckland);
            btnHamilton = FindViewById<Button>(Resource.Id.btnHamilton);
            btnChrist = FindViewById<Button>(Resource.Id.btnChrist);
            btnWellington = FindViewById<Button>(Resource.Id.btnWellington);

            objDb = new DatabaseManager();
            listDefaultCity = objDb.ViewDefaultCity();
            CityName = listDefaultCity[0].CityName;

            listDefaultTemp = objDb.ViewDefaultTemp();
            TempName = listDefaultTemp[0].TempName;



            SetButtonColors();      //Change button colours

            btnTrackGPS.Click += BtnTrackGPS_Click;

            btnAuckland.Click += BtnCity_Click;
            btnHamilton.Click += BtnCity_Click;
            btnChrist.Click += BtnCity_Click;
            btnWellington.Click += BtnCity_Click;

            btnCelsius.Click += BtnTemp_Click;
            btnFarenheit.Click += BtnTemp_Click;

        }


        private void UpdateCity (string newCityName)
        {
            objDb = new DatabaseManager();
            objDb.ChangeCity(newCityName);
        }
        private void UpdateTemp(string Temp)
        {
            objDb = new DatabaseManager();
            objDb.ChangeTemp(Temp);
        }

        private void BtnTemp_Click(object sender, EventArgs e)
        {
            Android.Graphics.Drawables.Drawable backDrawable = Resources.GetDrawable(Resource.Drawable.back);

            btnCelsius.SetBackgroundColor(Android.Graphics.Color.ParseColor("#0099cc"));
            btnFarenheit.SetBackgroundColor(Android.Graphics.Color.ParseColor("#0099cc"));

            btnCelsius.SetTextColor(Android.Graphics.Color.White);
            btnFarenheit.SetTextColor(Android.Graphics.Color.White);

            var button = sender as Button;
            button.SetBackgroundDrawable(backDrawable);
            button.SetTextColor(Android.Graphics.Color.Black);

            string ButtonText = button.Text;
            UpdateTemp(ButtonText);     //Do AWAIT function
        }

        private async void BtnTrackGPS_Click(object sender, EventArgs e)
        {
            AndHUD.Shared.Show(this, "Getting Weather info. Please wait");

            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;
            var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000000);

            longitude = position.Longitude;

            latitude = position.Latitude;


            Android.Graphics.Drawables.Drawable backDrawable = Resources.GetDrawable(Resource.Drawable.back);

            //Change button background colour and text 
            btnAuckland.SetBackgroundColor(Android.Graphics.Color.ParseColor("#0099cc"));
            btnHamilton.SetBackgroundColor(Android.Graphics.Color.ParseColor("#0099cc"));
            btnChrist.SetBackgroundColor(Android.Graphics.Color.ParseColor("#0099cc"));
            btnWellington.SetBackgroundColor(Android.Graphics.Color.ParseColor("#0099cc"));

            btnAuckland.SetTextColor(Android.Graphics.Color.White);
            btnHamilton.SetTextColor(Android.Graphics.Color.White);
            btnChrist.SetTextColor(Android.Graphics.Color.White);
            btnWellington.SetTextColor(Android.Graphics.Color.White);

            objRest = new RESThandler(@"http://api.openweathermap.org/data/2.5/weather?APPID=192917e709caf9dd129be1355d0f820a&mode=xml&lat=" + latitude + "&lon=" + longitude);
            //objRest = new RESThandler(@"http://api.openweathermap.org/data/2.5/weather?APPID=192917e709caf9dd129be1355d0f820a&mode=xml&lat=" + 55.75 + "&lon=" + 37.62);      // MOTHER RUSSIA TEST
            var Response = await objRest.ExecuteRequestAsync();

            objDb.ChangeCityGPS(Response.City.Name, Response.City.Country);

            AndHUD.Shared.Dismiss();
        }

        private void BtnCity_Click(object sender, EventArgs e)
        {
            Android.Graphics.Drawables.Drawable backDrawable = Resources.GetDrawable(Resource.Drawable.back);

            //Change button background colour and text 
            btnAuckland.SetBackgroundColor(Android.Graphics.Color.ParseColor("#0099cc"));
            btnHamilton.SetBackgroundColor(Android.Graphics.Color.ParseColor("#0099cc"));
            btnChrist.SetBackgroundColor(Android.Graphics.Color.ParseColor("#0099cc"));
            btnWellington.SetBackgroundColor(Android.Graphics.Color.ParseColor("#0099cc"));

            btnAuckland.SetTextColor(Android.Graphics.Color.White);
            btnHamilton.SetTextColor(Android.Graphics.Color.White);
            btnChrist.SetTextColor(Android.Graphics.Color.White);
            btnWellington.SetTextColor(Android.Graphics.Color.White);

            var button = sender as Button;
            button.SetBackgroundDrawable(backDrawable);
            button.SetTextColor(Android.Graphics.Color.Black);

            //Database
            string ButtonText = button.Text;
            UpdateCity(ButtonText);     //Do AWAIT function


        }

        private void SetButtonColors()
        {
            Android.Graphics.Drawables.Drawable backDrawable = Resources.GetDrawable(Resource.Drawable.back);

            //Change button background colour and text 
            btnAuckland.SetBackgroundColor(Android.Graphics.Color.ParseColor("#0099cc"));
            btnHamilton.SetBackgroundColor(Android.Graphics.Color.ParseColor("#0099cc"));
            btnChrist.SetBackgroundColor(Android.Graphics.Color.ParseColor("#0099cc"));
            btnWellington.SetBackgroundColor(Android.Graphics.Color.ParseColor("#0099cc"));

            btnAuckland.SetTextColor(Android.Graphics.Color.White);
            btnHamilton.SetTextColor(Android.Graphics.Color.White);
            btnChrist.SetTextColor(Android.Graphics.Color.White);
            btnWellington.SetTextColor(Android.Graphics.Color.White);

            btnCelsius.SetBackgroundColor(Android.Graphics.Color.ParseColor("#0099cc"));
            btnFarenheit.SetBackgroundColor(Android.Graphics.Color.ParseColor("#0099cc"));

            btnCelsius.SetTextColor(Android.Graphics.Color.White);
            btnFarenheit.SetTextColor(Android.Graphics.Color.White);

            if (TempName == "Celsius")
            {
                btnCelsius.SetBackgroundDrawable(backDrawable);
                btnCelsius.SetTextColor(Android.Graphics.Color.Black);
            }
            else if (TempName == "Fahrenheit")
            {
                btnFarenheit.SetBackgroundDrawable(backDrawable);
                btnFarenheit.SetTextColor(Android.Graphics.Color.Black);
            }

            if (CityName == "Auckland")
            {
                btnAuckland.SetBackgroundDrawable(backDrawable);
                btnAuckland.SetTextColor(Android.Graphics.Color.Black);
            }
            else if (CityName == "Hamilton")
            {
                btnHamilton.SetBackgroundDrawable(backDrawable);
                btnHamilton.SetTextColor(Android.Graphics.Color.Black);
            }
            else if (CityName == "Christchurch")
            {
                btnChrist.SetBackgroundDrawable(backDrawable);
                btnChrist.SetTextColor(Android.Graphics.Color.Black);
            }
            else if (CityName == "Wellington")
            {
                btnWellington.SetBackgroundDrawable(backDrawable);
                btnWellington.SetTextColor(Android.Graphics.Color.Black);
            }
        }

        public override void OnBackPressed()
        {
            StartActivity(typeof(MainActivity));
        }
    }
}