using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using FavoriteBeers;

namespace FavoriteBeers.Models
{
    public class Brewery
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
    }
}