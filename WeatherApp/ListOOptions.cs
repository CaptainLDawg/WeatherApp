using System;
using SQLite;

namespace WeatherApp
{
    public class ListOOptions
    {
        [PrimaryKey, AutoIncrement]
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public string isDefault { get; set; }

        public ListOOptions()
        {

        }
    }
}

