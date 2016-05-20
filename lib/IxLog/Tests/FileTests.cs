using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IxLog.Tests
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class LogFileTests
    {
        [TestMethod]
        public void TestWriteMsgs()
        {
            var log = new Log("ixlog");

            if (File.Exists(log.FilePath))
                File.Delete(log.FilePath);

            log.Debug("Some event just happened!");
            log.Info("Some event just happened!");
            log.Warning("Some event just happened!");
            log.Error("Some event just happened!");

            var lines = File.ReadAllText(log.FilePath);
            Assert.IsFalse(string.IsNullOrWhiteSpace(lines));
        }


        [TestMethod]
        public void TestConcurrentWWrites()
        {
            var log = new Log("ixlog");

            if (File.Exists(log.FilePath))
                File.Delete(log.FilePath);

            Parallel.For(0, 10000, (i) => log.Warning("lala "+ i));

            Assert.IsTrue(File.ReadAllLines(log.FilePath).All(l => l.StartsWith("[")));
        }
    }
}
