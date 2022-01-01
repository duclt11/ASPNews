using BTL_LT_UD_WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTL_LT_UD_WEB.Areas.Poster.Controllers
{
    public class LoginController : Controller
    {
        private dbContect db = new dbContect();
        // GET: Poster/Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(Login login)
        {
            var admin = db.posters.Where(u => u.email == login.Email && u.password == login.Password).FirstOrDefault();
            if (admin != null)
            {
                Session["posterId"] = admin.poster_id;
                // return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                Session["avatar"] = admin.avatar;
                Session["posterName"] = admin.username;
                return Redirect("/Poster/Poster/index");
                
            }
            ViewBag.Error = "Tài khoản hoặc mật khẩu không chính xác";
            //return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            return View(login);
            
        }
    }
}