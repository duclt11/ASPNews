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
using PagedList;

namespace BTL_LT_UD_WEB.Areas.Admin.Controllers
{
    [Authenticate]
    public class postersController : Controller
    {
        private dbContect db = new dbContect();

        // GET: posters
        public ActionResult Index(string searchStr, int? page)
        {
            var poster = from po in db.posters select po;
            if (!String.IsNullOrEmpty(searchStr))
            {
                poster = poster.Where(e => e.username.Contains(searchStr));
            }
            //Sắp xếp trước khi phân trang
            poster = poster.OrderBy(e => e.poster_id);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(poster.ToPagedList(pageNumber, pageSize));
        }

        // GET: posters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            poster poster = db.posters.Find(id);
            if (poster == null)
            {
                return HttpNotFound();
            }
            return View(poster);
        }

        // GET: posters/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: posters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "poster_id,email,username,fullname,avatar,password,phone,birthaday")] poster poster)
        {
            try
            {
                var check1 = db.users.FirstOrDefault(p => p.email == poster.email);
                var check2 = db.posters.FirstOrDefault(p => p.email == poster.email);
                if (check1 != null || check2!=null)
                {
                    ViewBag.IsExist = "Email này đã tồn tại trong User hoặc Poster";
                    return View(poster);
                }
                if (ModelState.IsValid)
                {
                    poster.avatar = "";
                    var f = Request.Files["ImageFile"];
                    if (f != null && f.ContentLength > 0)
                    {
                        string FileName = System.IO.Path.GetFileName(f.FileName);
                        string UploadPath = Server.MapPath("~/images/poster/" + FileName);
                        f.SaveAs(UploadPath);
                        poster.avatar = FileName;
                    }
                    
                    poster.created_at = DateTime.Now;

                }
                db.posters.Add(poster);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi nhập dữ liệu!" + ex.Message;
                return View(poster);
            }
        }

        // GET: posters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            poster poster = db.posters.Find(id);
            if (poster == null)
            {
                return HttpNotFound();
            }
            return View(poster);
        }

        // POST: posters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "poster_id,email,username,fullname,password,avatar,phone,birthaday,created_at")] poster poster)
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

        // GET: posters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            poster poster = db.posters.Find(id);
            if (poster == null)
            {
                return HttpNotFound();
            }
            return View(poster);
        }

        // POST: posters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            poster poster = db.posters.Find(id);
            db.posters.Remove(poster);
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
        
        public ActionResult Posted(int id)
        {
            var posted = db.posts.Where(e => e.poster_id == id).ToList();
            ViewBag.Name = db.posters.Find(id).username;
            return View(posted);
        }
    }
}
