using System;
using System.IO;
using System.Collections.Generic;

namespace WeatherApp
{
    public class DatabaseManager
    {
        static string dbName = "dbOptions.sqlite";
        string dbPath = System.IO.Path.Combine(Android.OS.Environment.ExternalStorageDirectory.ToString(), dbName);

        public DatabaseManager()
        {

        }
        public void ChangeCity(string CityName)
        {
            try
            {
                using (var conn = new SQLite.SQLiteConnection(dbPath))
                {
                    var cmd = new SQLite.SQLiteCommand(conn);
                    cmd.CommandText = "UPDATE tblCity SET isDefault = 'No'";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "UPDATE tblCity SET isDefault = 'Default' WHERE CityName = '" + CityName + "'";
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:" + e.Message);
            }
        }
        public void ChangeCityGPS(string CityName, string CountryName)
        {
            try
            {
                using (var conn = new SQLite.SQLiteConnection(dbPath))
                {
                    var cmd = new SQLite.SQLiteCommand(conn);
                    cmd.CommandText = "UPDATE tblCity SET isDefault = 'No'";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "UPDATE tblCity SET CityName = '" + CityName + "' WHERE isGPS = 'Default'";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "UPDATE tblCity SET CountryName = '" + CountryName + "' WHERE isGPS = 'Default'";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "UPDATE tblCity SET isDefault = 'Default' WHERE CityName = '" + CityName + "'";
                    cmd.ExecuteNonQuery();

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:" + e.Message);
            }
        }
        public void ChangeTemp(string TempName)
        {
            try
            {
                using (var conn = new SQLite.SQLiteConnection(dbPath))
                {
                    var cmd = new SQLite.SQLiteCommand(conn);
                    cmd.CommandText = "UPDATE tblTemp SET isDefault = 'No'";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "UPDATE tblTemp SET isDefault = 'Default' WHERE TempName = '" + TempName + "'";
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:" + e.Message);
            }
        }
        public List<ListOOptions> ViewDefaultCity()
        {
            try
            {
                using (var conn = new SQLite.SQLiteConnection(dbPath))
                {
                    var cmd = new SQLite.SQLiteCommand(conn);
                    cmd.CommandText = "Select * from tblCity Where isDefault = 'Default'";
                    var OptionsInfo = cmd.ExecuteQuery<ListOOptions>();
                    return OptionsInfo;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Error:" + e.Message);
                return null;
            }
        }
        public List<ListOTempertures> ViewDefaultTemp()
        {
            try
            {
                using (var conn = new SQLite.SQLiteConnection(dbPath))
                {
                    var cmd = new SQLite.SQLiteCommand(conn);
                    cmd.CommandText = "Select * from tblTemp Where isDefault = 'Default'";
                    var TempInfo = cmd.ExecuteQuery<ListOTempertures>();
                    return TempInfo;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Error:" + e.Message);
                return null;
            }
        }
    }
}