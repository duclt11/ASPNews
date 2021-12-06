using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using BTL_LT_UD_WEB.Models;

namespace BTL_LT_UD_WEB.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        //The login form is posted to this method.
        [HttpPost]
        public ActionResult Register()
        {
            return View();
        }
    }
}
