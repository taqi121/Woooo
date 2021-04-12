using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Woooo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult studio()
        {
            return View();
        }
        public ActionResult showcases()
        {
            return View();
        }

        public ActionResult blog()
        {
            return View();
        }
        [HttpGet]
        public ActionResult contact()
        {
            return View();
        }
        [HttpPost]
        [ActionName("contact")]
        public ActionResult contact1()
        {
            try
            {
                var name = Request["name"].ToString();
                var email = Request["email"].ToString();
                var Subject = Request["subject"].ToString();
                var message = Request["message"].ToString();

                var SendEmail = "taqi.malik86@gmail.com";
                SendMail.SendEmail(message, Subject, new string[] { SendEmail });
                TempData["msgContact"] = "Email Sent Successfully";
            }
            catch(Exception ex)
            {
                ViewBag.error = ex.Message;
            }
            
            return View();
        }
        public ActionResult indexvideo()
        {
            return View();
        }

        public ActionResult indexcarousel()
        {
            return View();
        }
        public ActionResult casesingle()
        {
            return View();
        }
    }
}