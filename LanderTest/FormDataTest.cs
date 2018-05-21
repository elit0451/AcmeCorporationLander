using ContentLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace LanderTest
{
    [TestClass]
    public class FormDataTest
    {
        List<int> serialNumbersList = new List<int>();

        [TestInitialize]
        public void Setup()
        {
            DataAccess data = new DataAccess();
            serialNumbersList = data.GetSerialNumbers();
        }

        [TestMethod]
        public void TestFormView()
        {
            var controller = new HomeController();
            var result = controller.Form() as ViewResult;
            Assert.AreEqual("Form", result.ViewName);
        }
    }
}
