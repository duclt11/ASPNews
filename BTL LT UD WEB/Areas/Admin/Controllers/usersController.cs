using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using BTL_LT_UD_WEB.Models;
using BTL_LT_UD_WEB.Security;
using PagedList;
using System.Net.Mail;
namespace BTL_LT_UD_WEB.Areas.Admin.Controllers
{
    [Authenticate]
    public class usersController : Controller
    {
        private dbContect db = new dbContect();
       
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
            user users = db.users.Find(id);
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
        #region Createcu
        //public ActionResult Create([Bind(Include = "email, username, fullname, password")] user user)
        //{

        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {

        //            var check1 = db.users.FirstOrDefault(p => p.email == user.email);
        //            var check2 = db.posters.FirstOrDefault(p => p.email == user.email);
        //            if (check1 != null || check2 != null)
        //            {
        //                ViewBag.IsExist = "Email này đã tồn tại trong User hoặc Poster";
        //                return View(user);
        //            }
        //            else
        //            {

        //                var f = Request.Files["ImageFile"];
        //                if (f != null && f.ContentLength > 0)
        //                {
        //                    string FileName = System.IO.Path.GetFileName(f.FileName);
        //                    string UploadPath = Server.MapPath("~/images/user/" + FileName);
        //                    f.SaveAs(UploadPath);
        //                    user.avatar = FileName;
        //                }

        //                user.created_at = DateTime.Now;
        //                db.users.Add(user);
        //                db.SaveChanges();
        //            }

        //        }

        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.Error = "Lỗi nhập dữ liệu!" + ex.Message;
        //        return View(user);
        //    }
        //}
        #endregion

        #region Create moi
        public ActionResult Create([Bind(Include = "email, username, fullname,phone,ProvinceID,DistrictID,address,avatar,birthaday, password")] user user)
        {

            try
            {
                if (ModelState.IsValid)
                {

                    var check1 = db.users.FirstOrDefault(p => p.email == user.email);
                    var check2 = db.posters.FirstOrDefault(p => p.email == user.email);

                    if (check1 != null || check2 != null)
                    {
                        ViewBag.IsExist = "Email này đã tồn tại trong User hoặc Poster";
                        return View(user);
                    }

                    else
                    {
                        var f = Request.Files["ImageFile"];
                        if (f != null && f.ContentLength > 0)
                        {
                            string FileName = System.IO.Path.GetFileName(f.FileName);
                            string UploadPath = Server.MapPath("~/images/user/" + FileName);
                            f.SaveAs(UploadPath);
                            user.avatar = FileName;
                        }
                        var xmlDoc = XDocument.Load(Server.MapPath(@"~/assets/client/data/Provinces_Data.xml"));

                        var province = xmlDoc.Element("Root").Elements("Item")
                        .Single(x => x.Attribute("type").Value == "province" && int.Parse(x.Attribute("id").Value) == Convert.ToInt32(user.ProvinceID));

                        var district = province.Elements("Item")
                        .Single(x => x.Attribute("type").Value == "district" && int.Parse(x.Attribute("id").Value) == Convert.ToInt32(user.DistrictID));



                        user.ProvinceID = Convert.ToString(province.Attribute("value").Value);
                        user.DistrictID = Convert.ToString(district.Attribute("value").Value);
                        user.password = Crypto.Hash(user.password);
                        user.created_at = DateTime.Now;
                        db.users.Add(user);
                        db.SaveChanges();

                        string message = "";
                      
                        //Send email for reset password
                        string resetCode = Guid.NewGuid().ToString();
                        SendVerificationLinkEmail(user.email, resetCode, user.password);
                        user.ResetPasswordCode = resetCode;
                        //This line I have added here to avoid confirm password not match issue , as we had added a confirm password property 
                        //in our model class in part 1
                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.SaveChanges();
                        message = "Your Account Created";

                        ViewBag.Message = message;
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
        #endregion


        [NonAction]
        public void SendVerificationLinkEmail(string Email, string activationCode, string emailFor)
        {
            //var verifyUrl = "/users/" + emailFor + "/" + activationCode;
            //var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("lethanh231120@gmail.com", "Dotnet Awesome");
            var toEmail = new MailAddress(Email);
            var fromEmailPassword = "dwvrcntiuzwxajsv";

            string subject = "";
            string body = "";
            subject = "Your account is successfully created!";
            body = "<br/>We are excited to tell you that your Dotnet Awesome account is" +
                " successfully created. Please click on the below link to verify your account" +
                " <br/> <p>info Account : <br/>email: " + Email + "<br/>" + " password:" + emailFor + "</p>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }



        // GET: users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        #region Edit cu

        //public ActionResult Edit([Bind(Include = "user_id, email,username,fullname,password,avatar,birthaday,created_at")] user users)
        //{
        //    //var old = db.users.FirstOrDefault(u => u.user_id == users.user_id);
        //    //users.avatar = old.avatar;
        //    //users.created_at = old.created_at;
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {

        //            var f = Request.Files["ImageFile"];
        //            if (f != null && f.ContentLength > 0)
        //            {
        //                string FileName = System.IO.Path.GetFileName(f.FileName);

        //                string UploadPath = Server.MapPath("~/images/user/" + FileName);
        //                f.SaveAs(UploadPath);
        //                users.avatar = FileName;

        //            }
        //            db.Entry(users).State = EntityState.Modified;
        //            db.SaveChanges();
        //        }

        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.Error = "Lỗi nhập dữ liệu!" + ex.ToString();
        //        return View(users);
        //    }

        //}
        #endregion

        #region Edit moi
        public ActionResult Edit([Bind(Include = "user_id,email, username, fullname,phone,ProvinceID,DistrictID,address, password, created_at")] user users)
        {
            //var old = db.users.FirstOrDefault(u => u.user_id == users.user_id);
            //users.avatar = old.avatar;
            //users.created_at = old.created_at;
            try
            {
                if (ModelState.IsValid)
                {
                    var xmlDoc = XDocument.Load(Server.MapPath(@"~/assets/client/data/Provinces_Data.xml"));

                    var province = xmlDoc.Element("Root").Elements("Item")
                    .Single(x => x.Attribute("type").Value == "province" && int.Parse(x.Attribute("id").Value) == Convert.ToInt32(users.ProvinceID));

                    var district = province.Elements("Item")
                    .Single(x => x.Attribute("type").Value == "district" && int.Parse(x.Attribute("id").Value) == Convert.ToInt32(users.DistrictID));

                   
                    users.ProvinceID = Convert.ToString(province.Attribute("value").Value);
                    users.DistrictID = Convert.ToString(district.Attribute("value").Value);
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
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu!" + ex.ToString();
                return View(users);
            }

        }
        #endregion

        // GET: users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user users = db.users.Find(id);
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
            user users = db.users.Find(id);
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
  
        public ActionResult CommentUser(int? id)
        {
            
            var allCmt = db.comments.Where(e => e.user_id == id).ToList();
            
            return View(allCmt);
        }

        public JsonResult LoadProvince()
        {
            var xmlDoc = XDocument.Load(Server.MapPath(@"~/assets/client/data/Provinces_Data.xml"));
            var xElements = xmlDoc.Element("Root").Elements("Item").Where(x => x.Attribute("type").Value == "province");
            var list = new List<ProvinceModel>();
            ProvinceModel province = null;
            foreach (var item in xElements)
            {
                province = new ProvinceModel();
                province.ID = int.Parse(item.Attribute("id").Value);
                province.Name = item.Attribute("value").Value;
                list.Add(province);

            }
            return Json(new
            {
                data = list,
                status = true
            });
        }
        public JsonResult LoadDistrict(int provinceID)
        {
            var xmlDoc = XDocument.Load(Server.MapPath(@"~/assets/client/data/Provinces_Data.xml"));

            var xElement = xmlDoc.Element("Root").Elements("Item")
                .Single(x => x.Attribute("type").Value == "province" && int.Parse(x.Attribute("id").Value) == provinceID);

            var list = new List<DistrictModel>();
            DistrictModel district = null;
            foreach (var item in xElement.Elements("Item").Where(x => x.Attribute("type").Value == "district"))
            {
                district = new DistrictModel();
                district.ID = int.Parse(item.Attribute("id").Value);
                district.Name = item.Attribute("value").Value;
                district.ProvinceID = int.Parse(xElement.Attribute("id").Value);
                list.Add(district);

            }
            return Json(new
            {
                data = list,
                status = true
            });
        }
    }

   
}
