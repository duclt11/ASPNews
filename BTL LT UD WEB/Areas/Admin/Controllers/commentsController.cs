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

namespace BTL_LT_UD_WEB.Areas.Admin.Controllers
{
    public class commentsController : Controller
    {
        private dbContect db = new dbContect();

        // GET: comments
        public ActionResult Index(string searchStr, int? page)
        {
            var comments = db.comments.Include(c => c.post).Include(c => c.user);

           
            //Sắp xếp trước khi phân trang
            comments = comments.OrderBy(e => e.comment_id);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(comments.ToPagedList(pageNumber, pageSize));
        }

        // GET: comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            comment comments = db.comments.Find(id);
            if (comments == null)
            {
                return HttpNotFound();
            }
            return View(comments);
        }

        // GET: comments/Create
        public ActionResult Create()
        {
            ViewBag.post_id = new SelectList(db.posts, "post_id", "title");
            ViewBag.user_id = new SelectList(db.users, "user_id", "email");
            return View();
        }

        // POST: comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "comment_id,content,status,user_id,datecomment,post_id")] comment comments)
        {
            if (ModelState.IsValid)
            {
                db.comments.Add(comments);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.post_id = new SelectList(db.posts, "post_id", "title", comments.post_id);
            ViewBag.user_id = new SelectList(db.users, "user_id", "email", comments.user_id);
            return View(comments);
        }

        // GET: comments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            comment comments = db.comments.Find(id);
            if (comments == null)
            {
                return HttpNotFound();
            }
            ViewBag.post_id = new SelectList(db.posts, "post_id", "title", comments.post_id);
            ViewBag.user_id = new SelectList(db.users, "user_id", "email", comments.user_id);
            return View(comments);
        }

        // POST: comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "comment_id,content,status,user_id,datecomment,post_id")] comment comments)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comments).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.post_id = new SelectList(db.posts, "post_id", "title", comments.post_id);
            ViewBag.user_id = new SelectList(db.users, "user_id", "email", comments.user_id);
            return View(comments);
        }

        // GET: comments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            comment comments = db.comments.Find(id);
            if (comments == null)
            {
                return HttpNotFound();
            }
            return View(comments);
        }

        // POST: comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            comment comments = db.comments.Find(id);
            db.comments.Remove(comments);
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

        public ActionResult CommentUser(int id)
        {
            return View();
        }
    }
}
