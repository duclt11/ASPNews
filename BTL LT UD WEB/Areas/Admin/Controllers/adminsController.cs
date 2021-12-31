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
            ViewBag.Posts = db.posts.Count();
            ViewBag.Poster = db.posters.Count();
			ViewBag.Category = db.categories.Count();

			ViewBag.User = db.users.Count();
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
            ViewBag.Error = "Tài khoản hoặc mật khẩu không chính xác";
            //return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            return View(login);
        }
        public ActionResult Logout()
        {
            Session["adminId"] = null;
            return Redirect("/");
        }
		public ActionResult Posts(int? year, int? month)
		{
			
			//if (year.HasValue && month.HasValue)
			//{
				var dayCount = DateTime.DaysInMonth(year.Value, month.Value);
				var saleData = (from o in db.posts.AsEnumerable()
								where o.created_at.Year == year && o.created_at.Month == month
								group o by o.created_at.Day
								into day
								select new { day = day.Key, postCount = day.Count() })
					.ToList();
				var dailySaleData = Enumerable.Repeat(0, dayCount).ToArray();
				saleData.ForEach(sd => dailySaleData[sd.day - 1] = sd.postCount);
				return Json(new { dayCount, data = dailySaleData }, JsonRequestBehavior.AllowGet);
			//}
			//if (year.HasValue)
			//{
			//	var saleData = (from o in db.posts.AsEnumerable()
			//					where o.created_at.Year == year
			//					group o by o.created_at.Month
			//		into m
			//					select new { month = m.Key, sale = m.Sum(SumFn) }).ToList();
			//	var monthlySaleData = Enumerable.Repeat(0, 12).ToArray();
			//	saleData.ForEach(sd => monthlySaleData[sd.month - 1] = sd.sale);
			//	return Json(new { data = monthlySaleData }, JsonRequestBehavior.AllowGet);
			//}
			//else
			//{
			//	var saleData = (from o in db.posts.AsEnumerable()
			//					group o by o.created_at.Year
			//		into y
			//					select new
			//					{
			//						year = y.Key,
			//						sale = y.Sum(SumFn)
			//					}).ToList();
			//	var minY = saleData.Min(d => d.year);
			//	var maxY = saleData.Max(d => d.year);
			//	var yearCount = maxY - minY + 1;
			//	var yearlySaleData = Enumerable.Repeat(0, yearCount).ToArray();
			//	saleData.ForEach(sd => yearlySaleData[sd.year - minY] = sd.sale);
			//	return Json(new { years = Enumerable.Range(minY, yearCount).ToArray(), data = yearlySaleData }, JsonRequestBehavior.AllowGet);
			//}
		}

	}

    
}

