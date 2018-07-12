using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using FavoriteBeers.Models;

namespace FavoriteBeers.Tests
{
    [TestClass]
    public class BreweryTests : IDisposable
    {
        //teardown method
        public void Dispose()
        {
            Beer.DeleteAll();
            Brewery.DeleteAll();
        }

        //sets up connection to test database
        public BreweryTests()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=favorite_beer_test;";
        }

        [TestMethod]
        public void Equals_ReturnsTrueIfBreweriesAreTheSame_Brewery()
        {
            Brewery firstBrewery = new Brewery("Enegren", "Moorpark", "CA");
            Brewery secondBrewery = new Brewery("Enegren", "Moorpark", "CA");
            Assert.AreEqual(firstBrewery, secondBrewery);
        }

        [TestMethod]
        public void GetAll_DatabaseStartsEmpty_0()
        {
            int result = Brewery.GetAll().Count;
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Save_SavesBreweryToDatabase_BreweryList()
        {
            Brewery newBrewery = new Brewery("Enegren", "Moorpark", "CA");
            newBrewery.Save();
            List<Brewery> testList = new List<Brewery> { newBrewery };
            List<Brewery> result = Brewery.GetAll();
            CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void Save_AssignsIdToObject_Id()
        {
            Brewery testBrewery = new Brewery("Enegren", "Moorpark", "CA");
            testBrewery.Save();
            Brewery savedBrewery = Brewery.GetAll()[0];
            int testId = testBrewery.Id;
            int result = savedBrewery.Id;
            Assert.AreEqual(testId, result);
        }

        [TestMethod]
        public void DeleteAll_DeletesAllBreweriesFromDatabase_BreweryList()
        {
            Brewery newBrewery1 = new Brewery("Enegren", "Moorpark", "CA");
            Brewery newBrewery2 = new Brewery("Elysian", "Seattle", "WA");
            newBrewery1.Save();
            newBrewery2.Save();
            Brewery.DeleteAll();
            List<Brewery> testList = new List<Brewery> { };
            List<Brewery> result = Brewery.GetAll();
            CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void FindBrewery_FindsBreweryInDatabase_Brewery()
        {
            Brewery newBrewery = new Brewery("Enegren", "Moorpark", "CA");
            newBrewery.Save();
            Brewery foundBrewery = Brewery.FindBrewery(newBrewery.Id);
            Assert.AreEqual(newBrewery, foundBrewery);
        }


    }
}
