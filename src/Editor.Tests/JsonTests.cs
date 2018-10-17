//using Client.Common.Json;
//using Newtonsoft.Json;
//using NUnit.Framework;
//using Shanism.Common;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;

//namespace Client.Editor.Tests
//{
//    [TestFixture]
//    public class JsonTests
//    {
//        [SetUp]
//        public void Setup()
//        {
//            JsonConfig.Initialize();
//        }

//        [Test]
//        public void SerializeRectF()
//        {
//            var inObj = new Container<RectangleF>
//            {
//                Foo = "test",
//                Item = new RectangleF(1, 2, 3, 4),
//            };

//            var text = JsonConvert.SerializeObject(inObj);

//            var outObj = JsonConvert.DeserializeObject<Container<RectangleF>>(text);

//            Assert.AreEqual(inObj.Item, outObj.Item);
//            Assert.AreEqual(inObj.Foo, outObj.Foo);
//        }

//        [Test]
//        public void SerializeRect()
//        {
//            var inObj = new Container<Rectangle>
//            {
//                Foo = "test",
//                Item = new Rectangle(1, 2, 3, 4),
//            };

//            var text = JsonConvert.SerializeObject(inObj);

//            var outObj = JsonConvert.DeserializeObject<Container<Rectangle>>(text);

//            Assert.AreEqual(inObj.Item, outObj.Item);
//            Assert.AreEqual(inObj.Foo, outObj.Foo);
//        }

//        class Container<T>
//        {
//            public T Item { get; set; }

//            public Container() { }
//            public Container(T item) { Item = item; }


//            public string Foo { get; set; }
//        }

//    }
//}
