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
                Beer newBeer = new Beer(breweryId, beerName, beerStyle, beerABV, beerIBU, beerId)

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


    }
}
