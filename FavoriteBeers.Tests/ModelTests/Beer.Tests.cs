using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using FavoriteBeers.Models;

namespace FavoriteBeers.Tests
{
    [TestClass]
    public class BeerTests : IDisposable
    {
        //teardown method
        public void Dispose()
        {
            Beer.DeleteAll();
        }

        //sets up connection to test database
        public BeerTests()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=favorite_beer_test;";
        }

        [TestMethod]
        public void GetAll_DbStartsEmpty_0()
        {
            int result = Beer.GetAll().Count;
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Equals_ReturnTrueIfBeersAreSame_Beers()
        {
            Beer firstBeer = new Beer(1, "Tecate", "Lager", 4.5, 15);
            Beer secondBeer = new Beer(1, "Tecate", "Lager", 4.5, 15);
            Assert.AreEqual(firstBeer, secondBeer);
        }

        [TestMethod]
        public void Save_SavesToDatabase_BeerList()
        {
            Beer firstBeer = new Beer(1, "Tecate", "Lager", 4.5, 15);
            firstBeer.Save();
            List<Beer> testList = new List<Beer> { firstBeer }; 
            List<Beer> result = Beer.GetAll();

            CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void Save_AssignsIdToObject_Id()
        {
            Beer testBeer = new Beer(1, "Tecate", "Lager", 4.5, 15);
            testBeer.Save();
            Beer savedBeer = Beer.GetAll()[0];
            int result = savedBeer.Id;
            int testId = testBeer.Id;
            Assert.AreEqual(testId, result);
        }

        [TestMethod]
        public void Find_FindsBeerInDatabase_Beer()
        {
            Beer testBeer = new Beer(1, "Tecate", "Lager", 4.5, 15);
            testBeer.Save();
            Beer foundBeer = Beer.FindBeer(testBeer.Id);
            Assert.AreEqual(testBeer, foundBeer);
        }

        [TestMethod]
        public void Update_UpdatesBeerInDatabase_String()
        {
            Beer testBeer = new Beer(1, "Tecate", "Lager", 4.5, 15);
            testBeer.Save();
            testBeer.Update(1, "Coors", "Lager", 4.5, 15);
            string result = Beer.FindBeer(testBeer.Id).Name;
            Console.WriteLine(result);
            Assert.AreEqual("Coors", result);
        }

        [TestMethod]
        public void Delete_DeletesBeerInDatabase_BeerList()
        {
            Beer testBeer = new Beer(1, "Tecate", "Lager", 4.5, 15);
            testBeer.Save();
            testBeer.Delete();
            List<Beer> resultList = Beer.GetAll();
            List<Beer> testList = new List<Beer> { };
            CollectionAssert.AreEqual(testList, resultList);
        }
    }
}
