using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BTL_LT_UD_WEB.Models;
using HtmlAgilityPack;
using PagedList;

namespace BTL_LT_UD_WEB.Controllers
{
    public class postsController : Controller
    {
        private dbContect db = new dbContect();

        // GET: posts
        public ActionResult Index(int? page)
        {
            var posts = db.posts.OrderBy(e=>e.created_at);
          
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            ViewBag.Hot = db.posts.Take(2).ToList();


            return View("Index", posts.ToPagedList(pageNumber, pageSize));
        }
        [ChildActionOnly]
        public ActionResult Catagory()
        {
            var cata = db.categories.ToList().Take(5);
            return PartialView("_Catagory", cata);
        }

       [ChildActionOnly]
       public ActionResult Newest()
        {
            var news = db.posts.ToList().OrderByDescending(p => p.created_at).Take(3);
            return PartialView("Newest", news);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            post post = db.posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        public ActionResult SideNews()
        {
            ViewBag.HotNews = db.posts.OrderBy(p => p.title).Take(4).ToList();
            ViewBag.PopularNews = db.posts.OrderBy(p => p.created_at).Take(4).ToList();
            ViewBag.Catagory = db.categories.Take(5).ToList();
           
            return PartialView("SideNews");
        }
        
       

        public static string GetSrcImage(string html)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            string htmlBody = "";
            if (htmlDoc.DocumentNode.SelectSingleNode("//img") == null)
            {
                htmlBody = "~/images/download.png";
            }
            else
            {
                htmlBody = htmlDoc.DocumentNode.SelectSingleNode("//img").Attributes["src"].Value;
            }
            return htmlBody;
        }

        public ActionResult GetComment(int id)
        {
            var allCmt = db.comments.Where(d => d.post_id == id).ToList();
            return PartialView("GetComment", allCmt);
        }
        
        public ActionResult CreateComment()
        {
            return View();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        
    }
}
