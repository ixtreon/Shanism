using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace ScenarioLib.Tests
{
    [TestClass]
    public class ScenarioSerializeTests
    {
        [TestMethod]
        public void testUnwantedProperties()
        {
            ScenarioConfig scenario = new ScenarioStub("lolol") { DontSerializePlz = 666 };

            var datas = JsonConvert.SerializeObject(scenario);

            var outScenario = JsonConvert.DeserializeObject<ScenarioConfig>(datas);

            Assert.AreEqual("lolol", outScenario.Name);
        }

        class ScenarioStub : ScenarioConfig
        {

            public ScenarioStub(string name)
            {
                this.Name = name;
            }

            public int DontSerializePlz { get; set; } = 42;
        }
    }
}
