using System;
using SQLite;

namespace WeatherApp
{
    public class ListOTempertures
    {
        [PrimaryKey, AutoIncrement]
        public string TempName { get; set; }
        public string isDefault { get; set; }

        public ListOTempertures()
        {

        }
    }
}