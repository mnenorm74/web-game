using System;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class SampleTest
    {
        [Test]
        public void Test()
        {
            Assert.AreEqual(4, 2 + 2);
        }
    }
}
