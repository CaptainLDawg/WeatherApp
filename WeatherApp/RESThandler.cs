using System;
using RestSharp;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WeatherApp
{
    class RESThandler
    {
        private string url;
        private IRestResponse response;

        public RESThandler()
        {
            url = "";
        }
        public RESThandler(string lurl)
        {
            url = lurl;
        }
        public async Task<Current> ExecuteRequestAsync()
        {

            var client = new RestClient(url);
            var request = new RestRequest();

            response = await client.ExecuteTaskAsync(request);

            XmlSerializer serializer = new XmlSerializer(typeof(Current));
            Current objCurrent;

            TextReader sr = new StringReader(response.Content);
            objCurrent = (Current)serializer.Deserialize(sr);
            return objCurrent;
        }
        public async Task<Weatherdata> ForeExecuteRequestAsync()
        {

            var client = new RestClient(url);
            var request = new RestRequest();

            response = await client.ExecuteTaskAsync(request);

            XmlSerializer serializer = new XmlSerializer(typeof(Weatherdata));
            Weatherdata objWeatherData;

            TextReader sr = new StringReader(response.Content);
            objWeatherData = (Weatherdata)serializer.Deserialize(sr);
            return objWeatherData;
        }
    }
}