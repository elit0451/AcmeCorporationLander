using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AcmeCorporationLander.Models;
using ContentLibrary;
using ReflectionIT.Mvc.Paging;

namespace AcmeCorporationLander.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(int page = 1)
        {
            var qry = DataAccess.GetSubmissions();
            var model = PagingList.Create(qry, 10, page);
            return View(model);
        }

        public IActionResult Submission()
        {
            return View("Submission");
        }

        [HttpPost]
        public IActionResult Submission(Submission submission)
        {
            if (!ModelState.IsValid)
            {
                return View("Submission");
            }
            else
            {
                switch (DataAccess.InsertSubmission(submission))
                {
                    case InsertResult.WRONG_AGE: ViewData["Message"] = "You are not allowed to draw a prize because you are below 18 years old.";
                        return View("Submission");
                    case InsertResult.DRAW_LIMIT_REACHED: ViewData["Message"] = "You reached your limit for drawing this prize.";
                        return View("Submission");
                }

            }
            return View("Index", DataAccess.GetSubmissions());
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
