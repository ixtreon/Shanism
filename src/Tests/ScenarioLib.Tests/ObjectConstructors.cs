//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ScenarioLib.Tests
//{
//    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
//    public class ObjectConstructors
//    {
//        class Foo
//        {
//            [JsonProperty(ItemTypeNameHandling = TypeNameHandling.Objects)]
//            public List<ObjectConstructor> Bar { get; set; }
//        }

//        //[TestMethod]
//        //public void TestListSerialize()
//        //{
//        //    var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
//        //    var list = new List<ObjectConstructor>
//        //    {
//        //        new DoodadConstructor { TypeName = "lala" },
//        //        new UnitConstructor { TypeName = "duduu", Owner = "Goshko" },
//        //    };

//        //    var listString = JsonConvert.SerializeObject(list);
//        //    var listResult = JsonConvert.DeserializeObject<List<ObjectConstructor>>(listString);        // FAILS: unable to create abstract class

//        //    Assert.IsTrue(listResult.First() is DoodadConstructor);
//        //    Assert.IsTrue(listResult.Skip(1).First() is UnitConstructor);
//        //}

//        [TestMethod]
//        public void TestContainerSerialize()
//        {
//            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
//            var list = new List<ObjectConstructor>
//            {
//                new DoodadConstructor { Animation = "lala" },
//                new UnitConstructor { TypeName = "duduu", Owner = "Goshko" },
//            };

//            var fooString = JsonConvert.SerializeObject(new Foo { Bar = list });
//            var fooResult = JsonConvert.DeserializeObject<Foo>(fooString);

//            Assert.IsTrue(fooResult.Bar.First() is DoodadConstructor);
//            Assert.IsTrue(fooResult.Bar.Skip(1).First() is UnitConstructor);
//        }
//    }
//}
