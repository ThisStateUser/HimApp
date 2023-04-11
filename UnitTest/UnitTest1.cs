using HimApp.BD;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TrueAuth()
        {
            Users result = HimApp.Controllers.UserObj.FindUser("admin","admin");
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void FalseAuth()
        {
            Users result = HimApp.Controllers.UserObj.FindUser("admin", "1hey");
            Assert.IsNull(result);
        }
    }
}
