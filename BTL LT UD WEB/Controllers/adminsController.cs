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

namespace BTL_LT_UD_WEB.Controllers
{
    public class adminsController : Controller
    {
        private dbContect db = new dbContect();
        //GET login
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult home()
        {
            return View();
        }
        // GET: admins
        public ActionResult Index(string searchStr, int? page)
        {
            var admins = from ad in db.admins select ad;
            if (!String.IsNullOrEmpty(searchStr))
            {
                admins = admins.Where(e => e.username.Contains(searchStr));
            }
            //Sắp xếp trước khi phân trang
            admins = admins.OrderBy(e => e.admin_id);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(admins.ToPagedList(pageNumber, pageSize));
        }

        // GET: admins/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            admin admins = db.admins.Find(id);
            if (admins == null)
            {
                return HttpNotFound();
            }
            return View(admins);
        }
        // GET: admins/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: admins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "admin_id,email,username,fullname,password,reset_password,avatar,birthaday,created_at")] admin admins)
        {
            if (ModelState.IsValid)
            {
                db.admins.Add(admins);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(admins);
        }

        // GET: admins/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            admin admins = db.admins.Find(id);
            if (admins == null)
            {
                return HttpNotFound();
            }
            return View(admins);
        }

        // POST: admins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "admin_id,email,username,fullname,password,reset_password,avatar,birthaday,created_at")] admin admins)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admins).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(admins);
        }

        // GET: admins/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            admin admins = db.admins.Find(id);
            if (admins == null)
            {
                return HttpNotFound();
            }
            return View(admins);
        }

        // POST: admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            admin admins = db.admins.Find(id);
            db.admins.Remove(admins);
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

