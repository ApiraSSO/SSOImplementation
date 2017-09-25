using System;
using System.Linq;
using System.IO;
using Kernel.Serialisation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serialisation.JSON.SettingsProviders;
using Newtonsoft.Json.Linq;

namespace Serialisation.JSON.Test
{
    [TestClass]
    public class UnitTest1
    {
        internal class Test
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        internal class Test1
        {
            public Test Test { get; }
        }

        [TestMethod]
        public void TestMethod1()
        {
            var setting = new DefaultSettingsProvider();
            var ser = new NSJsonSerializer(setting);
            var o = ser.Serialize(new Test { Id = 1, Name = "Test" });
            //var o = ser.Serialize(new { Id = 1, Name = "Test" });
            var sererialised = ser.Deserialize<object>(o);
            var t = sererialised.GetType();
        }

        [TestMethod]
        public void TestMethod2()
        {
            var setting = new DefaultSettingsProvider();
            var ser = new NSJsonSerializer(setting);
            var o = new Test { Id = 1, Name = "Test" };
            byte[] serialsed = null;
            using (var ms = new MemoryStream())
            {
                ((ISerializer)ser).Serialize(ms, new object[] { o });
                serialsed = ms.ToArray();
                ms.Position = 0;
                var r = ((ISerializer)ser).Deserialize(ms, null).First();
            }
        }
    }
}