using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shanism.Common.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkTests
{
    [TestClass]
    public class SerializationTests
    {

        [TestMethod]
        public void TestStructKind()
        { 

            int a = -5;
            DiffWriter.IntStruct s = new DiffWriter.IntStruct { IntValue = a };


            Debug.WriteLine(s.ByteA);
            Debug.WriteLine(s.ByteB);
            Debug.WriteLine(s.ByteC);
            Debug.WriteLine(s.ByteD);
        }

    }
}
