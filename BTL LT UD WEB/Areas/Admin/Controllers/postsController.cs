using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BTL_LT_UD_WEB.Models;
using BTL_LT_UD_WEB.Security;
using PagedList;

namespace BTL_LT_UD_WEB.Areas.Admin.Controllers
{
    [Authenticate]
    public class postsController : Controller
    {
        private dbContect db = new dbContect();

        // GET: posts
        public ActionResult Index(string searchStr, int? page)
        {
            var posts = db.posts.Include(p => p.category).Include(p => p.poster);

            if (!String.IsNullOrEmpty(searchStr))
            {
                posts = posts.Where(e => e.poster.username.Contains(searchStr));
            }
            //Sắp xếp trước khi phân trang
            posts = posts.OrderBy(e => e.post_id);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(posts.ToPagedList(pageNumber, pageSize));
        }

        // GET: posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            post posts = db.posts.Find(id);
            if (posts == null)
            {
                return HttpNotFound();
            }
            return View(posts);
        }

        // GET: posts/Create
        public ActionResult Create()
        {
            ViewBag.category_id = new SelectList(db.categories, "category_id", "category_name");
            ViewBag.poster_id = new SelectList(db.posters, "poster_id", "username");
            return View();
        }

        // POST: posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "post_id,title,description,content,avatar,created_at,category_id,poster_id")] post posts)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    
                    posts.created_at = DateTime.Now;
                    db.posts.Add(posts);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu!" + ex.Message;
                ViewBag.category_id = new SelectList(db.categories, "category_id", "category_name", posts.category_id);
                ViewBag.poster_id = new SelectList(db.posters, "poster_id", "username", posts.poster_id);
                return View(posts);
            }        
        }
        // GET: posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            post posts = db.posts.Find(id);
            if (posts == null)
            {
                return HttpNotFound();
            }
            ViewBag.category_id = new SelectList(db.categories, "category_id", "category_name", posts.category_id);
            ViewBag.poster_id = new SelectList(db.posters, "poster_id", "username", posts.poster_id);
            return View(posts);
        }

        // POST: posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "post_id,title,description,avatar,created_at,category_id,poster_id, content")] post posts)
        {

            try
            {
                db.Entry(posts).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                {
                    ViewBag.Error += eve.Entry.Entity.GetType().Name + " " + eve.Entry.State+'\n';
                    foreach(var e in eve.ValidationErrors)
                    {
                        ViewBag.Error += e.PropertyName + " " + e.ErrorMessage + '\n';
                    }
                }
                
                ViewBag.category_id = new SelectList(db.categories, "category_id", "category_name", posts.category_id);
                ViewBag.poster_id = new SelectList(db.posters, "poster_id", "email", posts.poster_id);
                return View(posts);
            }
        }

        // GET: posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            post posts = db.posts.Find(id);
            if (posts == null)
            {
                return HttpNotFound();
            }
            return View(posts);
        }

        // POST: posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            post posts = db.posts.Find(id);
            db.posts.Remove(posts);
            db.SaveChanges();
            return RedirectToAction("Index");
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
