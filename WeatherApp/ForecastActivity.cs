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
    [Activity(Label = "ForecastActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class ForecastActivity : Activity
    {
        List<ListOOptions> listDefaultCity;
        List<ListOTempertures> listDefaultTemp;

        static string dbName = "dbOptions.sqlite";
        string dbPath = System.IO.Path.Combine(Android.OS.Environment.ExternalStorageDirectory.ToString(), dbName);

        LinearLayout lnBackground;

        DatabaseManager objDb;
        RESThandler objRest;

        TextView lblCurrDate;
        TextView lblCurrTime;
        TextView lblCurrCity;
        ImageView imgCurrIcon;

        TextView lblDay1;
        TextView lblDay2;
        TextView lblDay3;
        TextView lblDay4;
        TextView lblDay5;

        TextView lblDate1;
        TextView lblDate2;
        TextView lblDate3;
        TextView lblDate4;
        TextView lblDate5;

        ImageView imgIcon1;
        ImageView imgIcon2;
        ImageView imgIcon3;
        ImageView imgIcon4;
        ImageView imgIcon5;

        TextView lblHighTemp1;
        TextView lblHighTemp2;
        TextView lblHighTemp3;
        TextView lblHighTemp4;
        TextView lblHighTemp5;

        TextView lblLowTemp1;
        TextView lblLowTemp2;
        TextView lblLowTemp3;
        TextView lblLowTemp4;
        TextView lblLowTemp5;

        string CityName;
        string CountryName;
        string TempName;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Forecast);

            lnBackground = FindViewById<LinearLayout>(Resource.Id.lnBackground);

            lblCurrDate = FindViewById<TextView>(Resource.Id.lblCurrDate);
            lblCurrTime = FindViewById<TextView>(Resource.Id.lblCurrTime);
            lblCurrCity = FindViewById<TextView>(Resource.Id.lblCurrCity);
            imgCurrIcon = FindViewById<ImageView>(Resource.Id.imgCurrIcon);

            lblDay1 = FindViewById<TextView>(Resource.Id.lblNameDay1);
            lblDay2 = FindViewById<TextView>(Resource.Id.lblNameDay2);
            lblDay3 = FindViewById<TextView>(Resource.Id.lblNameDay3);
            lblDay4 = FindViewById<TextView>(Resource.Id.lblNameDay4);
            lblDay5 = FindViewById<TextView>(Resource.Id.lblNameDay5);

            lblDate1 = FindViewById<TextView>(Resource.Id.lblDate1);
            lblDate2 = FindViewById<TextView>(Resource.Id.lblDate2);
            lblDate3 = FindViewById<TextView>(Resource.Id.lblDate3);
            lblDate4 = FindViewById<TextView>(Resource.Id.lblDate4);
            lblDate5 = FindViewById<TextView>(Resource.Id.lblDate5);

            imgIcon1 = FindViewById<ImageView>(Resource.Id.imgWeather1);
            imgIcon2 = FindViewById<ImageView>(Resource.Id.imgWeather2);
            imgIcon3 = FindViewById<ImageView>(Resource.Id.imgWeather3);
            imgIcon4 = FindViewById<ImageView>(Resource.Id.imgWeather4);
            imgIcon5 = FindViewById<ImageView>(Resource.Id.imgWeather5);

            lblLowTemp1 = FindViewById<TextView>(Resource.Id.lblLowTemp1);
            lblLowTemp2 = FindViewById<TextView>(Resource.Id.lblLowTemp2);
            lblLowTemp3 = FindViewById<TextView>(Resource.Id.lblLowTemp3);
            lblLowTemp4 = FindViewById<TextView>(Resource.Id.lblLowTemp4);
            lblLowTemp5 = FindViewById<TextView>(Resource.Id.lblLowTemp5);

            lblHighTemp1 = FindViewById<TextView>(Resource.Id.lblHighTemp1);
            lblHighTemp2 = FindViewById<TextView>(Resource.Id.lblHighTemp2);
            lblHighTemp3 = FindViewById<TextView>(Resource.Id.lblHighTemp3);
            lblHighTemp4 = FindViewById<TextView>(Resource.Id.lblHighTemp4);
            lblHighTemp5 = FindViewById<TextView>(Resource.Id.lblHighTemp5);

            objDb = new DatabaseManager();
            listDefaultCity = objDb.ViewDefaultCity();
            CityName = listDefaultCity[0].CityName;
            CountryName = listDefaultCity[0].CountryName;

            listDefaultTemp = objDb.ViewDefaultTemp();
            TempName = listDefaultTemp[0].TempName;

            getWeatherInfo(CityName, CountryName);

        }

        public async void getWeatherInfo(string defaultCityName, string defaultCountryName)
        {
            AndHUD.Shared.Show(this, "Getting Forecast info");

            objRest = new RESThandler(@"http://api.openweathermap.org/data/2.5/forecast/daily?APPID=192917e709caf9dd129be1355d0f820a&mode=xml&q=" + defaultCityName + ",NZ");

            var ResponseFore = await objRest.ForeExecuteRequestAsync();


            string time = DateTime.Now.ToString("h:mm tt");
            lblCurrTime.Text = time;
            lblCurrDate.Text = DateTime.Today.DayOfWeek.ToString() + ", " + System.DateTime.Today.ToShortDateString();
            lblCurrCity.Text = defaultCityName + "," + defaultCountryName;

            lblDate1.Text = ResponseFore.Forecast.Time[1].Day;
            lblDate2.Text = ResponseFore.Forecast.Time[2].Day;
            lblDate3.Text = ResponseFore.Forecast.Time[3].Day;
            lblDate4.Text = ResponseFore.Forecast.Time[4].Day;
            lblDate5.Text = ResponseFore.Forecast.Time[5].Day;

            DateTime dt1 = DateTime.ParseExact(ResponseFore.Forecast.Time[1].Day, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            lblDay1.Text = dt1.DayOfWeek.ToString();
            DateTime dt2 = DateTime.ParseExact(ResponseFore.Forecast.Time[2].Day, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            lblDay2.Text = dt2.DayOfWeek.ToString();
            DateTime dt3 = DateTime.ParseExact(ResponseFore.Forecast.Time[3].Day, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            lblDay3.Text = dt3.DayOfWeek.ToString();
            DateTime dt4 = DateTime.ParseExact(ResponseFore.Forecast.Time[4].Day, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            lblDay4.Text = dt4.DayOfWeek.ToString();
            DateTime dt5 = DateTime.ParseExact(ResponseFore.Forecast.Time[5].Day, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            lblDay5.Text = dt5.DayOfWeek.ToString();

            var imageIcon = GetImageBitmapFromUrl("http://openweathermap.org/img/w/" + ResponseFore.Forecast.Time[0].Symbol.Var + ".png");
            imgCurrIcon.SetImageBitmap(imageIcon);

            imageIcon = GetImageBitmapFromUrl("http://openweathermap.org/img/w/" + ResponseFore.Forecast.Time[1].Symbol.Var + ".png");
            imgIcon1.SetImageBitmap(imageIcon);

            imageIcon = GetImageBitmapFromUrl("http://openweathermap.org/img/w/" + ResponseFore.Forecast.Time[2].Symbol.Var + ".png");
            imgIcon2.SetImageBitmap(imageIcon);

            imageIcon = GetImageBitmapFromUrl("http://openweathermap.org/img/w/" + ResponseFore.Forecast.Time[3].Symbol.Var + ".png");
            imgIcon3.SetImageBitmap(imageIcon);

            imageIcon = GetImageBitmapFromUrl("http://openweathermap.org/img/w/" + ResponseFore.Forecast.Time[4].Symbol.Var + ".png");
            imgIcon4.SetImageBitmap(imageIcon);

            imageIcon = GetImageBitmapFromUrl("http://openweathermap.org/img/w/" + ResponseFore.Forecast.Time[5].Symbol.Var + ".png");
            imgIcon5.SetImageBitmap(imageIcon);

            if (TempName == "Celsius")
            {
                lblLowTemp1.Text = Convert.ToInt32(Convert.ToDouble(ResponseFore.Forecast.Time[1].Temperature.Min.ToString()) - 273.15) + "°";
                lblLowTemp2.Text = Convert.ToInt32(Convert.ToDouble(ResponseFore.Forecast.Time[2].Temperature.Min.ToString()) - 273.15) + "°";
                lblLowTemp3.Text = Convert.ToInt32(Convert.ToDouble(ResponseFore.Forecast.Time[3].Temperature.Min.ToString()) - 273.15) + "°";
                lblLowTemp4.Text = Convert.ToInt32(Convert.ToDouble(ResponseFore.Forecast.Time[4].Temperature.Min.ToString()) - 273.15) + "°";
                lblLowTemp5.Text = Convert.ToInt32(Convert.ToDouble(ResponseFore.Forecast.Time[5].Temperature.Min.ToString()) - 273.15) + "°";

                lblHighTemp1.Text = Convert.ToInt32(Convert.ToDouble(ResponseFore.Forecast.Time[1].Temperature.Max.ToString()) - 273.15) + "°";
                lblHighTemp2.Text = Convert.ToInt32(Convert.ToDouble(ResponseFore.Forecast.Time[2].Temperature.Max.ToString()) - 273.15) + "°";
                lblHighTemp3.Text = Convert.ToInt32(Convert.ToDouble(ResponseFore.Forecast.Time[3].Temperature.Max.ToString()) - 273.15) + "°";
                lblHighTemp4.Text = Convert.ToInt32(Convert.ToDouble(ResponseFore.Forecast.Time[4].Temperature.Max.ToString()) - 273.15) + "°";
                lblHighTemp5.Text = Convert.ToInt32(Convert.ToDouble(ResponseFore.Forecast.Time[5].Temperature.Max.ToString()) - 273.15) + "°";
            }
            else
            {
                //F = 1.8 x (K - 273) + 32.
                lblLowTemp1.Text = Convert.ToInt32((1.8 * (Convert.ToDouble(ResponseFore.Forecast.Time[1].Temperature.Min.ToString()) - 273.15)) + 32) + "°";
                lblLowTemp2.Text = Convert.ToInt32((1.8 * (Convert.ToDouble(ResponseFore.Forecast.Time[2].Temperature.Min.ToString()) - 273.15)) + 32) + "°";
                lblLowTemp3.Text = Convert.ToInt32((1.8 * (Convert.ToDouble(ResponseFore.Forecast.Time[3].Temperature.Min.ToString()) - 273.15)) + 32) + "°";
                lblLowTemp4.Text = Convert.ToInt32((1.8 * (Convert.ToDouble(ResponseFore.Forecast.Time[4].Temperature.Min.ToString()) - 273.15)) + 32) + "°";
                lblLowTemp5.Text = Convert.ToInt32((1.8 * (Convert.ToDouble(ResponseFore.Forecast.Time[5].Temperature.Min.ToString()) - 273.15)) + 32) + "°";

                lblHighTemp1.Text = Convert.ToInt32((1.8 * (Convert.ToDouble(ResponseFore.Forecast.Time[1].Temperature.Max.ToString()) - 273.15)) + 32) + "°";
                lblHighTemp2.Text = Convert.ToInt32((1.8 * (Convert.ToDouble(ResponseFore.Forecast.Time[2].Temperature.Max.ToString()) - 273.15)) + 32) + "°";
                lblHighTemp3.Text = Convert.ToInt32((1.8 * (Convert.ToDouble(ResponseFore.Forecast.Time[3].Temperature.Max.ToString()) - 273.15)) + 32) + "°";
                lblHighTemp4.Text = Convert.ToInt32((1.8 * (Convert.ToDouble(ResponseFore.Forecast.Time[4].Temperature.Max.ToString()) - 273.15)) + 32) + "°";
                lblHighTemp5.Text = Convert.ToInt32((1.8 * (Convert.ToDouble(ResponseFore.Forecast.Time[5].Temperature.Max.ToString()) - 273.15)) + 32) + "°";
            }

            //if (ResponseFore.Forecast.Time[5].Symbol.Var == "01d" || ResponseFore.Forecast.Time[5].Symbol.Var == "01n")
            //{
            //    lnBackground.SetBackgroundResource(Resource.Drawable.ClearDay);
            //}
            //else if (ResponseFore.Forecast.Time[5].Symbol.Var == "02d" || ResponseFore.Forecast.Time[5].Symbol.Var == "02n")
            //{
            //    lnBackground.SetBackgroundResource(Resource.Drawable.few_clouds);
            //}
            //else if (ResponseFore.Forecast.Time[5].Symbol.Var == "03d" || ResponseFore.Forecast.Time[5].Symbol.Var == "03n")
            //{
            //    lnBackground.SetBackgroundResource(Resource.Drawable.scattered_clouds);
            //}
            //else if (ResponseFore.Forecast.Time[5].Symbol.Var == "04d" || ResponseFore.Forecast.Time[5].Symbol.Var == "04n")
            //{
            //    lnBackground.SetBackgroundResource(Resource.Drawable.scattered_clouds);
            //}
            //else if (ResponseFore.Forecast.Time[5].Symbol.Var == "09d" || ResponseFore.Forecast.Time[5].Symbol.Var == "09n")
            //{
            //    lnBackground.SetBackgroundResource(Resource.Drawable.rain);
            //}
            //else if (ResponseFore.Forecast.Time[5].Symbol.Var == "10d" || ResponseFore.Forecast.Time[5].Symbol.Var == "10n")
            //{
            //    lnBackground.SetBackgroundResource(Resource.Drawable.rain);
            //}
            //else if (ResponseFore.Forecast.Time[5].Symbol.Var == "11d" || ResponseFore.Forecast.Time[5].Symbol.Var == "11n")
            //{
            //    lnBackground.SetBackgroundResource(Resource.Drawable.Storm);
            //}
            //else if (ResponseFore.Forecast.Time[5].Symbol.Var == "13d" || ResponseFore.Forecast.Time[5].Symbol.Var == "13n")
            //{
            //    lnBackground.SetBackgroundResource(Resource.Drawable.Mist);
            //}
            //else if (ResponseFore.Forecast.Time[5].Symbol.Var == "50d" || ResponseFore.Forecast.Time[5].Symbol.Var == "50n")
            //{
            //    lnBackground.SetBackgroundResource(Resource.Drawable.Snow);
            //}

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
    }
}