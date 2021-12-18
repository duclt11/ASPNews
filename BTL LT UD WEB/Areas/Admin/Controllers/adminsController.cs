using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BTL_LT_UD_WEB.Models;
using PagedList;
using BTL_LT_UD_WEB.Security;
namespace BTL_LT_UD_WEB.Areas.Admin.Controllers
{
    
    public class adminsController : Controller
    {

        // GET: admins
        private dbContect db = new dbContect();

        [Authenticate]
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Login login)
        {
            var admin = db.admins.Where(u => u.email == login.Email && u.password == login.Password).FirstOrDefault();
            if (admin != null)
            {
                Session["adminId"] = admin.admin_id;
               // return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                return Redirect("/Admin/admins/index");
            }
            ViewBag.Error = "Khong the dang nhap";
            //return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            return View(login);
        }
        public ActionResult Logout()
        {
            Session["adminId"] = null;
            return Redirect("/");
        }
        
    }

    
}

