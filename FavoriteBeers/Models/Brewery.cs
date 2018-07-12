using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using FavoriteBeers;

namespace FavoriteBeers.Models
{
    public class Brewery
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int Id { get; set; }

        public Brewery(string newName, string newCity, string newState, int newId = 0)
        {
            Name = newName;
            City = newCity;
            State = newState;
            Id = newId;
        }

        public override bool Equals(System.Object otherBrewery)
        {
            if (!(otherBrewery is Brewery))
            {
                return false;
            }
            else
            {
                Brewery newBrewery = (Brewery) otherBrewery;
                bool nameEquality = (this.Name == newBrewery.Name);
                bool cityEquality = (this.City == newBrewery.City);
                bool stateEquality = (this.State == newBrewery.State);
                bool idEquality = (this.Id == newBrewery.Id);
                return (nameEquality && cityEquality && stateEquality && idEquality);
            }
        }

        public static List<Brewery> GetAll()
        {
            List<Brewery> allBreweries = new List<Brewery> {};
            
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM breweries;";
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            while (rdr.Read())
            {
                string breweryName = rdr.GetString(0);
                string breweryCity = rdr.GetString(1);
                string breweryState = rdr.GetString(2);
                int breweryId = rdr.GetInt32(3);
                Brewery newBrewery = new Brewery (breweryName, breweryCity, breweryState, breweryId);
                allBreweries.Add(newBrewery);
            }

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allBreweries;
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO breweries (name, city, state) VALUES (@breweryName, @breweryCity, @breweryState);";

            cmd.Parameters.AddWithValue("@breweryName", this.Name);
            cmd.Parameters.AddWithValue("@breweryCity", this.City);
            cmd.Parameters.AddWithValue("@breweryState", this.State);

            cmd.ExecuteNonQuery();
            Id = (int) cmd.LastInsertedId;

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM breweries;";

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static Brewery FindBrewery(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM breweries WHERE id = @thisId;";
            cmd.Parameters.AddWithValue("@thisId", id);
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

            string breweryName = "";
            string breweryCity = "";
            string breweryState = "";
            int breweryId = 0;

            while (rdr.Read())
            {
                breweryName = rdr.GetString(0);
                breweryCity = rdr.GetString(1);
                breweryState = rdr.GetString(2);
                breweryId = rdr.GetInt32(3);
            }

            Brewery foundBrewery = new Brewery(breweryName, breweryCity, breweryState, breweryId);

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return foundBrewery;
        }
    }
}