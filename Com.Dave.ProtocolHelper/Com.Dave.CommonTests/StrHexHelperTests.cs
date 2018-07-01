using Microsoft.VisualStudio.TestTools.UnitTesting;
using Com.Dave.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Dave.Common.Tests
{
    [TestClass()]
    public class StrHexHelperTests
    {
        [TestMethod()]
        public void StringToHexTest()
        {
            var tt = StrHexHelper.StringToHex("68 04 00 04 08");
            byte[] byteBuffer = new byte[] { 0x68, 0x04, 0, 4, 8 };
            Assert.AreEqual(tt[0], byteBuffer[0]);
        }
    }
}