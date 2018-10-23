using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EasyEasy.Client;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace EasyEasy.Client.Test
{
    [TestClass]
    public class UnitTest1
    {
        private readonly Client _client = new Client("http://localhost:3333", "e241daea-cb57-4a39-95bc-30de055dda7d");

        [TestMethod]
        public void TestCrud()
        {
            var cat = new Cat()
            {
                Name = "Sam",
                Age = 1.5,
                Interests = new string[] { "play", "eat" }
            };
            var id = _client.AddAsync(cat).Result;
            Assert.IsNotNull(id);

            cat = _client.GetOneAsync<Cat>(id).Result;

            Assert.IsNotNull(cat);
            Assert.IsNotNull(cat.Id);

            cat.Age = 1.7;
            _client.UpdateAsync(cat).Wait();

            var cats = _client.GetAsync<Cat>(new { }).Result;
            cats = _client.GetAsync<Cat>(new { age = 1.5 }).Result;
            cats = _client.GetAsync<Cat>(new { age_gte = 1.5 }).Result;
            cats = _client.GetAsync<Cat>(new { name_like = "Sa*" }).Result;

            cats = _client.GetAsync<Cat>(new { _start = 10, _count = 10 }).Result;

            _client.DeleteAsync<Cat>(cat.Id).Wait();
        }

        [TestMethod]
        public void TestMethod2()
        {
            foreach (var i in Enumerable.Range(0, 100))
            {
                var cat = new Cat()
                {
                    Name = "Sam #" + i.ToString(),
                    Age = 1.5,
                    Interests = new string[] { "play", "eat" }
                };
                var id = _client.AddAsync(cat).Result;
                cat = _client.GetOneAsync<Cat>(id).Result;

                Debug.WriteLine(id + " - " + cat.Id);

            }
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
