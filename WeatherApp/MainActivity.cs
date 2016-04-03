using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;

using Android.Graphics.Drawables;
using System.IO;
using Android.Graphics;
using System.Net;
using System.Text;

using AndroidHUD;

namespace WeatherApp
{
    [Activity(Label = "WeatherApp", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
        List<ListOOptions> listDefaultCity;
        List<ListOTempertures> listDefaultTemp;

        static string dbName = "dbOptions.sqlite";
        string dbPath = System.IO.Path.Combine(Android.OS.Environment.ExternalStorageDirectory.ToString(), dbName);

        DatabaseManager objDb;
        RESThandler objRest;

        TextView lblCityName;
        TextView lblCityTemp;
        TextView lblHumidity;
        TextView lblCityFeels;
        TextView lblWeatherCondition;
        TextView lblDate;
        TextView lblTime;

        ImageView imgIcon;
        Button btnHelp;
        Button btnForecast;

        LinearLayout lnBackground;

        double HeatIndex;
        string CityName;
        string CountryName;
        string TempName;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            GetDatabase();

            lblDate = FindViewById<TextView>(Resource.Id.lblDate);
            lblTime = FindViewById<TextView>(Resource.Id.lblTime);
            lblCityName = FindViewById<TextView>(Resource.Id.lblCityName);
            lblCityTemp = FindViewById<TextView>(Resource.Id.lblTemp);
            lblCityFeels = FindViewById<TextView>(Resource.Id.lblFeels);
            lblHumidity = FindViewById<TextView>(Resource.Id.lblHumidity);
            lblWeatherCondition = FindViewById<TextView>(Resource.Id.lblWeatherCond);
            imgIcon = FindViewById<ImageView>(Resource.Id.imgIcon);
            btnHelp = FindViewById<Button>(Resource.Id.btnHelp);
            btnForecast = FindViewById<Button>(Resource.Id.btnForecast);

            lnBackground = FindViewById<LinearLayout>(Resource.Id.lnBackground);
            

            objDb = new DatabaseManager();
            listDefaultCity = objDb.ViewDefaultCity();
            CityName = listDefaultCity[0].CityName;
            CountryName = listDefaultCity[0].CountryName;

            listDefaultTemp = objDb.ViewDefaultTemp();
            TempName = listDefaultTemp[0].TempName;

            getWeatherInfo(CityName, CountryName);

            btnHelp.Click += BtnHelp_Click;
            btnForecast.Click += BtnForecast_Click;

        }

        private void BtnForecast_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(ForecastActivity));
        }

        private void BtnHelp_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(OptionsActivity));
            this.Finish();
        }

        public async void getWeatherInfo(string defaultCityName, string defaultCountryName)
        {
            AndHUD.Shared.Show(this, "Getting Weather info");

            objRest = new RESThandler(@"http://api.openweathermap.org/data/2.5/weather?APPID=192917e709caf9dd129be1355d0f820a&mode=xml&q=" + defaultCityName + "," + CountryName);
        
            var Response = await objRest.ExecuteRequestAsync();

            var imageIcon = GetImageBitmapFromUrl("http://openweathermap.org/img/w/" + Response.Weather.Icon + ".png");
            imgIcon.SetImageBitmap(imageIcon);

            CalculateHeatIndex(Convert.ToDouble(Response.Temperature.Value), Convert.ToDouble(Response.Humidity.Value));   //Calculate the Heat Index
            int farHeatIndex = Convert.ToInt32(HeatIndex);    //Convert HeatIndex to int, which removes all decimal places

            double kelTemp = Convert.ToDouble(Response.Temperature.Value);


            if (TempName == "Celsius")
                {
                    int celTemp = Convert.ToInt32(kelTemp - 273.15);
                    int celHeatIndex = ((farHeatIndex - 32) * 5) / 9;
                    lblCityTemp.Text = celTemp + "°";
                    lblCityFeels.Text = "Feels like \n" + celHeatIndex + "°";
                }
            else
            {
                int farTemp = Convert.ToInt32(1.8 * (kelTemp - 273) + 32);
                lblCityTemp.Text = farTemp + "°";
                lblCityFeels.Text = "Feels like \n" + farHeatIndex + "°";
            }

            //change background
            if (Response.Weather.Icon == "01d" || Response.Weather.Icon == "01n")
            {
                lnBackground.SetBackgroundResource(Resource.Drawable.ClearDay);
            }
            else if (Response.Weather.Icon == "02d" || Response.Weather.Icon == "02n")
            {
                lnBackground.SetBackgroundResource(Resource.Drawable.few_clouds);
            }
            else if (Response.Weather.Icon == "03d" || Response.Weather.Icon == "03n")
            {
                lnBackground.SetBackgroundResource(Resource.Drawable.scattered_clouds);
            }
            else if (Response.Weather.Icon == "04d" || Response.Weather.Icon == "04n")
            {
                lnBackground.SetBackgroundResource(Resource.Drawable.scattered_clouds);
            }
            else if (Response.Weather.Icon == "09d" || Response.Weather.Icon == "09n")
            {
                lnBackground.SetBackgroundResource(Resource.Drawable.rain);
            }
            else if (Response.Weather.Icon == "10d" || Response.Weather.Icon == "10n")
            {
                lnBackground.SetBackgroundResource(Resource.Drawable.rain);
            }
            else if (Response.Weather.Icon == "11d" || Response.Weather.Icon == "11n")
            {
                lnBackground.SetBackgroundResource(Resource.Drawable.Storm);
            }
            else if (Response.Weather.Icon == "13d" || Response.Weather.Icon == "13n")
            {
                lnBackground.SetBackgroundResource(Resource.Drawable.Snow);
            }
            else if (Response.Weather.Icon == "50d" || Response.Weather.Icon == "50n")
            {
                lnBackground.SetBackgroundResource(Resource.Drawable.Mist);
            }

            //Set Text
            lblCityName.Text = Response.City.Name + ", " + Response.City.Country;
            lblHumidity.Text = "Humidity \n" + Response.Humidity.Value + "%";
            lblWeatherCondition.Text = "Weather\n" + Response.Weather.Value;



            string time = DateTime.Now.ToString("h:mm tt");
            lblTime.Text = time;
            lblDate.Text = DateTime.Today.DayOfWeek.ToString() + ", " + System.DateTime.Today.ToShortDateString();

            AndHUD.Shared.Dismiss();

        }

        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }

        public void CalculateHeatIndex(double T, double R)
        {
            //T = Temperture, R = Humidity
            double Kelv = T;
            T = 1.8 * ( Kelv - 273) + 32;       //Convert Temperture to Farenheit from Kelvin as the HeatIndex formula uses Farenheit.
            HeatIndex = -42.379 + (2.04901523 * T) + (10.14333127 * R) - (0.22475541 * (T * R)) - (0.00683783 * (T * T)) - (0.05481717 * (R * R)) + (0.00122874 * ((T * T) * R)) + (0.00085282 * ((T * R) * R)) - (0.00000199 * (((T * T) * R) * R));
        }

        public void GetDatabase()
        {
            //DeleteDatabase(dbPath);       //If I change the database, use this to delete the database that's already on the phone
            // Check if your DB has already been extracted.
            if (!File.Exists(dbPath))
            {
                using (BinaryReader br = new BinaryReader(Assets.Open(dbName)))
                {
                    using (BinaryWriter bw = new BinaryWriter(new FileStream(dbPath, FileMode.Create)))
                    {
                        byte[] buffer = new byte[2048];
                        int len = 0;
                        while ((len = br.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            bw.Write(buffer, 0, len);
                        }
                    }
                }
            }
        }

    }
}

