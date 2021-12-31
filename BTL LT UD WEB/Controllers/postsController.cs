using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BTL_LT_UD_WEB.Models;
using BTL_LT_UD_WEB.Security;
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
            var posts = db.posts.OrderByDescending(e=>e.created_at);
          
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
        public ActionResult PostByCategory(int? id_catagory, int? page)
        {
            var posts = db.posts.Where(e=>e.category_id==id_catagory).ToList();

            int pageSize = 7;
            int pageNumber = (page ?? 1);
            ViewBag.Name = db.categories.Where(e => e.category_id == id_catagory).FirstOrDefault().category_name;
            

            return View(posts.ToPagedList(pageNumber, pageSize));
           
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

        public ActionResult GetComment(int id, int? page)
        {
            var allCmt = db.comments.Where(d => d.post_id == id).OrderByDescending(e=>e.datecomment).ToList();
            ViewBag.post_id = id;
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return PartialView("GetComment", allCmt.ToPagedList(pageNumber, pageSize));
        }
      

        [AuthenticateUser]
        [HttpPost]
        public ActionResult CreateComment(int? user_id, int? id, string content)
        {

            comment cmt = new comment
            {
                user_id = user_id,
                post_id = id,
                content = content,
                datecomment = DateTime.Now

            };
            db.comments.Add(cmt);
            db.SaveChanges();
            //return PartialView("GetComment");
            return RedirectToAction("Details", "posts", new { id = id });
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult Contact()
        {
            return View();
        }

    }
}
