using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EasyEasy.Client;
using System.Collections.Generic;

namespace EasyEasy.Client.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var client = new Client("bc420813-17d9-47fa-9d29-8bd6a1207f8a");

            var cat = new Cat()
            {
                Name = "Sam",
                Age = 1.5,
                Interests = new string[] { "play", "eat" }
            };
            var id = client.AddAsync(cat).Result;
            Assert.IsNotNull(id);

            cat = client.GetOneAsync<Cat>(id).Result;

            Assert.IsNotNull(cat);
            Assert.IsNotNull(cat.Id);

            cat.Age = 1.7;
            client.UpdateAsync(cat).Wait();

            var cats = client.GetAsync<Cat>(new { }).Result;
            cats = client.GetAsync<Cat>(new { age = 1.5 }).Result;
            cats = client.GetAsync<Cat>(new { age_gte = 1.5 }).Result;
            cats = client.GetAsync<Cat>(new { name_like = "Sa*" }).Result;

            cats = client.GetAsync<Cat>(new { _start = 10, _count = 10 }).Result;

            client.DeleteAsync<Cat>(cat.Id).Wait();
        }
    }

    class Cat
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public double Age { get; set; }

        public IEnumerable<string> Interests { get; set; }
    }
}
