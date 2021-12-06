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
    public class usersController : Controller
    {
        private dbContect db = new dbContect();
        public ActionResult Register()
        {
            return View();
        }
        //GET Login
        public ActionResult Login()
        {
            return View();
        }

        // GET: users
        public ActionResult home()
        {
            return View();
        }
        public ActionResult Index(string searchStr, int? page)
        {
            var user = from us in db.users select us;
            if (!String.IsNullOrEmpty(searchStr))
            {
                user = user.Where(e => e.username.Contains(searchStr));
            }
            //Sắp xếp trước khi phân trang
            user = user.OrderBy(e => e.user_id);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(user.ToPagedList(pageNumber, pageSize));
        }

        // GET: users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            users users = db.users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // GET: users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "email, username, fullname, password, birthaday")] users user)
        {
            
            try
            {
                if(ModelState.IsValid)
                {
                    
                    var check = db.users.FirstOrDefault(p => p.email == user.email);
                    if (check!=null)
                    {
                        ViewBag.IsExist = "Email nay da ton tai";
                        return View(user);
                    }
                    else
                    {
                        user.avatar = "";
                        var f = Request.Files["ImageFile"];
                        if (f != null && f.ContentLength > 0)
                        {
                            string FileName = System.IO.Path.GetFileName(f.FileName);
                            string UploadPath = Server.MapPath("~/images/user/" + FileName);
                            f.SaveAs(UploadPath);
                            user.avatar = FileName;
                        }
                        
                        user.created_at = DateTime.Now;
                        db.users.Add(user);
                        db.SaveChanges();
                    }
                        
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu!" + ex.Message;             
                return View(user);
            }
        }

        // GET: users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            users user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }


        // POST: users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "email,username,fullname,password,birthaday")] users users)
        {
            try
            {
                var f = Request.Files["ImageFile"];
                if (f != null && f.ContentLength > 0)
                {
                    string FileName = System.IO.Path.GetFileName(f.FileName);
                    string UploadPath = Server.MapPath("~/images/user/" + FileName);
                    f.SaveAs(UploadPath);
                    users.avatar = FileName;
                }
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu!";
                return View(users);
            }

        }

        // GET: users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            users users = db.users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            users users = db.users.Find(id);
            db.users.Remove(users);
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
