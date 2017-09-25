using System;
using System.Linq;
using CircuitBreakerInfrastructure;
using CircuitBreakerTests.MockData;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CircuitBreakerTests
{
    internal class TestClass
    {
        public string StringProperty { get; set; }
        public int IntProperty { get; set; }
    }

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var strategies = new FailedOperationStrategy[] { new PingFailedOperationStrategy(), new TimeoutFailedOperationStrategy() };
            Action<FailedOperationContext> seed = (c) => 
            {
            };
            var chain = strategies.Aggregate(seed, (f, next) => new Action<FailedOperationContext>(c =>  next.Apply(c, f)));
            var context = new FailedOperationContext();
            chain(context);
        }

        [TestMethod]
        public void TestRefMethod1()
        {
            var testClass = new TestClass
            {
                StringProperty = "TestString",
                IntProperty = 1
            };
            var testClass1 = testClass;
            this.RefMethod(ref testClass);
            var b = Object.ReferenceEquals(testClass1, testClass);
            Assert.AreSame(testClass1, testClass);
            Assert.IsTrue(b);
        }

        [TestMethod]
        public void TestRefMethod2()
        {
            var i = 1;
            var testClass = new TestClass
            {
                StringProperty = "TestString",
                IntProperty = 1
            };
            var testClass1 = testClass;
            this.RefMethod1(ref testClass);
            this.RefMethod(ref i);
            var b = Object.ReferenceEquals(testClass1, testClass);
            Assert.AreNotSame(testClass1, testClass);
            Assert.IsFalse(b);
            Assert.AreEqual(2, i);
        }

        [TestMethod]
        public void TestRefMethod3()
        {
            var i = 1;
            var testClass = new TestClass
            {
                StringProperty = "TestString",
                IntProperty = 1
            };
            var testClass1 = testClass;
            this.Method(testClass);
            this.Method(i);
            var b = Object.ReferenceEquals(testClass1, testClass);
            Assert.AreSame(testClass1, testClass);
            Assert.IsTrue(b);
            Assert.AreEqual(1, i);
        }

        [TestMethod]
        public void TestRefMethod4()
        {
            var testClass = new TestClass
            {
                StringProperty = "TestString",
                IntProperty = 1
            };
            TestClass testClass1 = null;
            this.RefMethod1(ref testClass1);

            var b = Object.ReferenceEquals(testClass1, testClass);
            Assert.AreNotSame(testClass1, testClass);
            Assert.IsFalse(b);
        }

        [TestMethod]
        public void OutMethodTest1()
        {
            var testClass = new TestClass
            {
                StringProperty = "TestString",
                IntProperty = 1
            };
            var testClass1 = testClass;
            this.OutMethod(out testClass);
            var b = Object.ReferenceEquals(testClass1, testClass);
            Assert.AreNotSame(testClass1, testClass);
            Assert.IsFalse(b);
        }

        [TestMethod]
        public void OutMethodTest2()
        {
            var testClass = new TestClass
            {
                StringProperty = "TestString",
                IntProperty = 1
            };
            var testClass1 = testClass;
            this.OutMethod1(out testClass);

            var b = Object.ReferenceEquals(testClass1, testClass);
            Assert.AreNotSame(testClass1, testClass);
            Assert.IsFalse(b);
        }

        [TestMethod]
        public void OutMethodTest3()
        {
            var testClass = new TestClass
            {
                StringProperty = "TestString",
                IntProperty = 1
            };
            TestClass testClass1;
            this.OutMethod1(out testClass1);

            var b = Object.ReferenceEquals(testClass1, testClass);
            Assert.AreNotSame(testClass1, testClass);
            Assert.IsFalse(b);
        }

        #region ref methods
        private void RefMethod(ref TestClass o)
        {
            o.IntProperty = 2;
        }

        private void RefMethod(ref int o)
        {
            o = 2;
        }
        private void RefMethod1(ref TestClass o)
        {
            o = new TestClass
            {
                StringProperty = "TestString",
                IntProperty = 1
            };
        }
        #endregion

        private void OutMethod(out TestClass o)
        {
            o = null;
        }

        private void OutMethod1(out TestClass o)
        {
            o = new TestClass
            {
                StringProperty = "TestString",
                IntProperty = 1
            };
        }
        private void Method(TestClass o)
        {
            o = new TestClass
            {
                StringProperty = "TestString",
                IntProperty = 1
            };
        }
        private void Method(int o)
        {
            o = 2;
        }
    }
}
