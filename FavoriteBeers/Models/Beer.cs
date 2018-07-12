using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using FavoriteBeers;

namespace FavoriteBeers.Models
{
    public class Beer
    {
        public int BreweryId { get; set; }
        public string Name { get; set; }
        public string Style { get; set; }
        public double ABV { get; set; }
        public int IBU { get; set; }
        public int Id { get; set; }

        public Beer(int newBreweryId, string newName, string newStyle, double newABV, int newIBU, int newId = 0)
        {
            BreweryId = newBreweryId;
            Name = newName;
            Style = newStyle;
            ABV = newABV;
            IBU = newIBU;
            Id = newId;
        }

        public override bool Equals(System.Object otherBeer)
        {
            if (!(otherBeer is Beer))
            {
                return false;
            }
            else
            {
                Beer newBeer = (Beer) otherBeer; //cast otherBeer as Beer object 
                bool breweryIdEquality = (this.BreweryId == newBeer.BreweryId);
                bool nameEquality = (this.Name == newBeer.Name);
                bool styleEquality = (this.Style == newBeer.Style);
                bool abvEquality = (this.ABV == newBeer.ABV);
                bool ibuEquality = (this.IBU == newBeer.IBU);
                bool IdEquality = (this.Id == newBeer.Id);
                return (breweryIdEquality && nameEquality && styleEquality && abvEquality && ibuEquality);
            }
        }

        // public override int GetHashCode()
        // {
        //     return this.Name.GetHashCode();
        // }

        public static List<Beer> GetAll()
        {
            //setting up empty list of all beers
            List<Beer> allBeers = new List<Beer> {};

            //establishes MySqlDataReader object (rdr) that executes a query (select all records (instances) from beers table)
            MySqlConnection conn = DB.Connection(); 
            conn.Open(); 
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand; 
            cmd.CommandText = @"SELECT * FROM beers;";
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            
            // to instantiate every beer object matching the above query and adds it to to the empty list established in the beginning
            //gets every beer record that matches the above query
            while(rdr.Read())
            {
                //matches column in beers table to correspondig variables
                int breweryId = rdr.GetInt32(0);
                string beerName = rdr.GetString(1);
                string beerStyle = rdr.GetString(2);
                double beerABV = rdr.GetDouble(3);
                int beerIBU = rdr.GetInt32(4);
                int beerId = rdr.GetInt32(5);

                //instantiates new Beer instance
                Beer newBeer = new Beer(breweryId, beerName, beerStyle, beerABV, beerIBU, beerId);

                //adds instance to list that contains every beer matching query
                allBeers.Add(newBeer);
            }

            //closes server connection
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allBeers;
        }

        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM beers;";

            //not searching, just deleting
            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO beers (brewery_id, name, style, abv, ibu) VALUES (@breweryId, @beerName, @beerStyle, @beerABV, @beerIBU);";

            //sets the instances property values as the values in the sql command
            cmd.Parameters.AddWithValue("@breweryId", this.BreweryId);
            cmd.Parameters.AddWithValue("@beerName", this.Name);
            cmd.Parameters.AddWithValue("@beerStyle", this.Style);
            cmd.Parameters.AddWithValue("@beerABV", this.ABV);
            cmd.Parameters.AddWithValue("@beerIBU", this.IBU);

            cmd.ExecuteNonQuery();
            Id = (int) cmd.LastInsertedId; //cast LastInsertedId (returns long) to int and auto-increments record's id

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static Beer FindBeer(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM beers WHERE id = @thisId;";
            cmd.Parameters.AddWithValue("@thisId", id);
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

            //unsure why this is here...ask tomorrow
            int breweryId = 0;
            string beerName = "";
            string beerStyle = "";
            double beerABV = 0.0;
            int beerIBU = 0;
            int beerId = 0;

            while(rdr.Read())
            {
                breweryId = rdr.GetInt32(0);
                beerName = rdr.GetString(1);
                beerStyle = rdr.GetString(2);
                beerABV = rdr.GetDouble(3);
                beerIBU = rdr.GetInt32(4);
                beerId = rdr.GetInt32(5);
            }

            Beer foundBeer = new Beer(breweryId, beerName, beerStyle, beerABV, beerIBU, beerId);

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return foundBeer;
        }
    }
}
