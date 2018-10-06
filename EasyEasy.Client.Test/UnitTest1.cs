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
            var client = new Client("http://localhost:3333", "9013c822-ca97-45ee-8724-4535e959149c"
                );

            var cat = new Cat()
            {
                Name = "Sam",
                Age = 1.5,
                Interests = new string[] { "play", "eat" }
            };
            var id = client.AddAsync(cat).Result;
            Assert.IsNotNull(id);

            cat = client.GetOne<Cat>(id).Result;

            Assert.IsNotNull(cat);
            Assert.IsNotNull(cat.Id);

            cat.Age = 1.7;
            client.UpdateAsync(cat).Wait();

            var cats = client.Get<Cat>(new { }).Result;
            cats = client.Get<Cat>(new { age=1.5 }).Result;
            cats = client.Get<Cat>(new { age_gte=1.5 }).Result;
            cats = client.Get<Cat>(new { name_like="Sa*" }).Result;

            client.Delete<Cat>(cat.Id).Wait();
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
