using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaskCrawler.Models.Dto;

using System;
using System.Collections.Generic;
using System.Text;

namespace MaskCrawler.Models.Dto.Tests
{
    [TestClass()]
    public class BackResultTests
    {
        [TestMethod()]
        public void JudgeTest()
        {
            var obj = BackResult.Judge<Testmodel>(1, "ok", new
            {
                id = 10,
                name = "hello",
                age = "15"
            });
            Assert.IsNotNull(obj);
        }

        public class Testmodel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
        }
    }
}