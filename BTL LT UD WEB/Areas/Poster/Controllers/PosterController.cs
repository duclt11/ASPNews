using BTL_LT_UD_WEB.Models;
using BTL_LT_UD_WEB.Security;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BTL_LT_UD_WEB.Areas.Poster.Controllers
{
    [AuthenticatePoster]
    public class PosterController : Controller
    {
        private dbContect db = new dbContect();
        // GET: Poster/Poster
        public ActionResult Index(string searchStr, int? page)
        {
            int posterId = int.Parse(Session["posterId"].ToString());
                var posts = db.posts.Where(e=>e.poster_id==posterId);
                //Search theo tieu de
                if (!String.IsNullOrEmpty(searchStr))
                {
                    posts = posts.Where(e => e.title.Contains(searchStr));
                }
                //Sắp xếp trước khi phân trang
                posts = posts.OrderByDescending(e => e.post_id);
                int pageSize = 3;
                int pageNumber = (page ?? 1);
                return View(posts.ToPagedList(pageNumber, pageSize));
            

            
        }
        public ActionResult Logout()
        {
            Session["posterId"] = null;
            return Redirect("/");
        }
        
        public ActionResult Info()
        {
            int posterId = int.Parse(Session["posterId"].ToString());
            var res = db.posters.Find(posterId);
            return View(res);
        }

        // POST: posters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Info([Bind(Include = "poster_id,username,fullname,password,avatar,phone,birthaday,created_at, email")] poster poster)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var f = Request.Files["ImageFile"];
                    if (f != null && f.ContentLength > 0)
                    {
                        string FileName = System.IO.Path.GetFileName(f.FileName);

                        string UploadPath = Server.MapPath("~/images/poster/" + FileName);
                        f.SaveAs(UploadPath);
                        poster.avatar = FileName;

                    }
                    Session["posterName"] = poster.username;
                    Session["avatar"] = poster.avatar;
                    db.Entry(poster).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu!" + ex.ToString();
                return View(poster);
            }
        }
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
            var cmt = db.comments.Where(e => e.post_id == id);
            try
            {
                db.comments.RemoveRange(cmt);
                db.posts.Remove(posts);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Error = "Bài viết này có comment, không thể xóa";
                return View("Delete");
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult PostComment(int id)
        {
            var res = db.comments.Where(e => e.post_id == id).ToList();
            TempData["post_id"] = id;
            return View(res);

        }
        
        public ActionResult DeleteComment(int id)
        {
            var del = db.comments.Find(id);

            db.Entry(del).State = EntityState.Deleted;
            db.SaveChanges();
            return Redirect("Poster/PostComment/" + TempData["post_id"]);
        }
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
        //[ValidateInput(false)]
        public ActionResult Create([Bind(Include = "post_id,title,description,content,created_at,category_id,poster_id")] post posts)
        {
            try
            {


                if (ModelState.IsValid)
                {

                    posts.created_at = DateTime.Now;
                    db.posts.Add(posts);
                    db.SaveChanges();
                }
                else
                {
                    ViewBag.Error = "Lỗi nhập dữ liệu, kiểm tra lại!";
                    return View(posts);
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
        public ActionResult Edit([Bind(Include = "post_id,title,description,created_at,category_id,poster_id")] post posts)
        {

            try
            {
                
                // posts.content = Request.Form["content"];
                db.Entry(posts).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");

                //posts.content = Request.Form["content"];
                // return View(posts);
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                {
                    ViewBag.Error += eve.Entry.Entity.GetType().Name + " " + eve.Entry.State + '\n';
                    foreach (var e in eve.ValidationErrors)
                    {
                        ViewBag.Error += e.PropertyName + " " + e.ErrorMessage + '\n';
                    }
                }

                ViewBag.category_id = new SelectList(db.categories, "category_id", "category_name", posts.category_id);
                ViewBag.poster_id = new SelectList(db.posters, "poster_id", "email", posts.poster_id);
                return View(posts);
            }
        }

    }
}