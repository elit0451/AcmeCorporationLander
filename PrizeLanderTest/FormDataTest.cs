using AcmeCorporationLander.Controllers;
using ContentLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrizeLanderTest
{
    [TestClass]
    public class FormDataTest
    {
        List<int> serialNumbersList = new List<int>();

        [TestInitialize]
        public void Setup()
        {
            serialNumbersList = DataAccess.GetSerialNumbers();
            DataAccess.GetStarted();
        }

        [TestMethod]
        public void IsTheRightViewReturned()
        {
            HomeController controller = new HomeController();
            ViewResult result = controller.Submission() as ViewResult;
            Assert.AreEqual("Submission", result.ViewName);
        }

        [TestMethod]
        public void IsTheSubmissionNotSucessfulBecauseUserAgeIsBelow18()
        {
            Submission s = new Submission("Emily", "Bensen", "emilybensen@gmail.com", 52732222);
            int submissionsCount = DataAccess.GetSubmissions().Count;

            HomeController controller = new HomeController();
            ViewResult result = controller.Submission(s) as ViewResult;
            ViewDataDictionary viewData = result.ViewData;
            Assert.AreEqual("You are not allowed to draw a prize because you are below 18 years old.", viewData["Message"]);
            Assert.AreEqual(submissionsCount, DataAccess.GetSubmissions().Count);
        }

        [TestMethod]
        public void IsTheSubmissionSucessfulIfAgeIsAbove18()
        {

            int submissionsCount = DataAccess.GetSubmissions().Count;
            Submission s = new Submission("Louise", "Grill", "l.grill@gmail.com", 52732222);

            HomeController controller = new HomeController();
            controller.Submission(s);
            Assert.AreEqual(submissionsCount + 1, DataAccess.GetSubmissions().Count);
        }

        [TestMethod]
        public void IsTheSubmissionSucessfulIfCustomerHasValidSerialNr()
        {

            int submissionsCount = DataAccess.GetSubmissions().Count;
            Submission s = new Submission("Louise", "Grill", "l.grill@gmail.com", 52732222);

            HomeController controller = new HomeController();
            controller.Submission(s);
            controller.Submission(s);
            Assert.AreEqual(submissionsCount + 2, DataAccess.GetSubmissions().Count);
        }

        [TestMethod]
        public void IsTheSubmissionUnsucessfulIfCustomerHasReachedTheLimitForThisProduct()
        {
            Submission s = new Submission("Louise", "Grill", "l.grill@gmail.com", 52732222);

            HomeController controller = new HomeController();
            controller.Submission(s);
            controller.Submission(s);
            int submissionsCount = DataAccess.GetSubmissions().Count;
            controller.Submission(s);
            ViewResult result = controller.Submission(s) as ViewResult;
            ViewDataDictionary viewData = result.ViewData;
            Assert.AreEqual("You reached your limit for drawing this prize.", viewData["Message"]);
            Assert.AreEqual(submissionsCount, DataAccess.GetSubmissions().Count);
        }

        [TestMethod]
        public void IsTheSubmissionUnsucessfulIfFormIsWronglyField()
        {
            Submission s = new Submission(null, "Andersen", "j.andersen@gmail.com", 37295254);

            int submissionsCount = DataAccess.GetSubmissions().Count;
            HomeController controller = new HomeController();
            controller.Submission(s);
            ViewResult result = controller.Submission() as ViewResult;
            Assert.AreEqual("Submission", result.ViewName);

            Assert.AreEqual(submissionsCount, DataAccess.GetSubmissions().Count);
        }
    }
}
