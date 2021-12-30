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
using System.Xml.Linq;
using System.Net.Mail;
using System.Web.Security;
using System.IO;

namespace BTL_LT_UD_WEB.Controllers
{
    
    public class HomeController : Controller
    {
        // GET: Home
        private dbContect db = new dbContect();

        public ActionResult login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Login login)
        {
            login.Password = Crypto.Hash(login.Password);
            var user = db.users.Where(u => u.email == login.Email && u.password == login.Password).FirstOrDefault();
            if (user != null)
            {
                Session["userid"] = user.user_id;
                Session["avatar"] = user.avatar;
                Session["Email"] = user.email;
                Session["UserName"] = user.username;
                Session["FullName"] = user.fullname;
                Session["Phone"] = user.phone;
                Session["Address"] = user.address;
                Session["Password"] = user.password;
                Session["ProvinceName"] = user.ProvinceID;
                Session["DistrictName"] = user.DistrictID;

                return RedirectToAction("index", "posts");
            }
            else if (user == null)
            {
                ViewBag.Error = "Đăng nhập thất bại. Vui lòng kiểm tra lại! ";
            }
            //return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            return View(login);


        }
        public ActionResult Register()
        {
            return View();
        }

        //POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(user user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var check1 = db.users.FirstOrDefault(p => p.email == user.email);
                    var check2 = db.posters.FirstOrDefault(p => p.email == user.email);
                    var check3 = db.users.FirstOrDefault(p => p.username == user.username);
                    if (check1 != null || check2 != null)
                    {
                        ViewBag.IsExist = "Email này đã được sử dụng";
                        return View(user);
                    }
                   
                    else
                    {
                        var xmlDoc = XDocument.Load(Server.MapPath(@"~/assets/client/data/Provinces_Data.xml"));

                        var province = xmlDoc.Element("Root").Elements("Item")
                        .Single(x => x.Attribute("type").Value == "province" && int.Parse(x.Attribute("id").Value) == Convert.ToInt32(user.ProvinceID));

                        var district = province.Elements("Item")
                        .Single(x => x.Attribute("type").Value == "district" && int.Parse(x.Attribute("id").Value) == Convert.ToInt32(user.DistrictID));

                        user.ProvinceID = Convert.ToString(province.Attribute("value").Value);
                        user.DistrictID = Convert.ToString(district.Attribute("value").Value);
                        var PassWord = user.password;
                        user.password = Crypto.Hash(user.password);
                        user.created_at = DateTime.Now;
                        db.users.Add(user);
                        db.SaveChanges();


                        
                        //Send email for reset password
                        string resetCode = Guid.NewGuid().ToString();
                        SendVerificationLinkEmail(user.email, resetCode, PassWord);
                        user.ResetPasswordCode = resetCode;
                        //This line I have added here to avoid confirm password not match issue , as we had added a confirm password property 
                        //in our model class in part 1
                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.SaveChanges();
                       

                        ViewBag.Message = "Tạo tài khoản thành công";


                        return RedirectToAction("Index", "posts");
                    }
                }
            }
            catch
            {
                ViewBag.Error = "Có lỗi xảy ra";
                
            }
            return View();

        }
            

        
        public void SendVerificationLinkEmail(string Email, string activationCode, string emailFor)
        {
            //var verifyUrl = "/users/" + emailFor + "/" + activationCode;
            //var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("lethanh231120@gmail.com", "Dotnet Awesome");
            var toEmail = new MailAddress(Email);
            var fromEmailPassword = "dwvrcntiuzwxajsv";

            string subject = "";
            string body = "";
            subject = "Xác minh Email!";
            body = "<br/Tài khoản của bạn vừa được tạo thành công" +
                ". Hãy xác minh tài khoản của bạn" +
                " <br/> <p>Thông tin tài khoản : <br/>email: " + Email + "<br/>" + " Mật khẩu:" + emailFor + "</p>";

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

        public ActionResult userInfo(int? id)
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
        [AuthenticateUser]
        public ActionResult EditAccount(int? id)
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
        [AuthenticateUser]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAccount([Bind(Include = "user_id,email, username, fullname,phone,ProvinceID,DistrictID,address,avatar, password")] user user)
        {
            if (ModelState.IsValid)
            {
                var xmlDoc = XDocument.Load(Server.MapPath(@"~/assets/client/data/Provinces_Data.xml"));

                var province = xmlDoc.Element("Root").Elements("Item")
                .Single(x => x.Attribute("type").Value == "province" && int.Parse(x.Attribute("id").Value) == Convert.ToInt32(user.ProvinceID));

                var district = province.Elements("Item")
                .Single(x => x.Attribute("type").Value == "district" && int.Parse(x.Attribute("id").Value) == Convert.ToInt32(user.DistrictID));

                user.ProvinceID = Convert.ToString(province.Attribute("value").Value);
                user.DistrictID = Convert.ToString(district.Attribute("value").Value);

                user.created_at = DateTime.Now;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                Session["HoTen"] = user.username;
                return RedirectToAction("userInfo", new { id = Session["userid"] });
            }
            return View(user);
        }
        [AuthenticateUser]
        public ActionResult ChangePass(int? id)
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
        [AuthenticateUser]
        public ActionResult ChangePass([Bind(Include = "user_id,email, username, fullname,phone,ProvinceID,DistrictID,address,password")] user user)
        {
            string email = Request["email"];
            string cu = Request["MatKhauCu"];
            string moi = Request["MatKhauMoi"];
            string xacnhan = Request["MatKhauXacNhan"];
            string password = Convert.ToString(Session["Password"]);
            if (cu != null && moi != null && xacnhan != null && email != null)
            {
                cu = Crypto.Hash(cu.Trim());
                moi = moi.Trim();
                xacnhan = xacnhan.Trim();
                password = password.Trim();
                if (cu != "" && moi != "" && xacnhan != "" && email != "")
                {
                    if (email == Session["Email"].ToString().Trim())
                    {

                        if (moi == xacnhan)
                        {
                            if (cu == password + "")
                            {
                                user.password = Crypto.Hash(moi);
                                Session["Password"] = Crypto.Hash(moi);
                                db.Entry(user).State = EntityState.Modified;
                                db.SaveChanges();
                                return RedirectToAction("userInfo", new { id = Session["userid"] });
                            }
                            else
                            {
                                ViewBag.LoiMatKhau = "Mật khẩu cũ không khớp!" + Session["Password"] + "      " + Crypto.Hash(cu);
                                return View(user);
                            }
                        }
                        else
                        {
                            ViewBag.LoiMatKhau = "Nhập lại mật khẩu không khớp!";
                            return View(user);
                        }
                    }
                    else
                    {
                        ViewBag.LoiMatKhau = "Email không đúng!";
                        return View(user);
                    }
                }
                else
                {
                    ViewBag.LoiMatKhau = "Không được để trống trường dữ liệu nào!";
                    return View(user);
                }

            }
            else
            {
                ViewBag.LoiMatKhau = "Không được để trống trường dữ liệu nào!";
                return View(user);
            }
        }
        public ActionResult listPost(int? id)
        {
            var cmt = db.posts.Where(u => u.category_id == id).ToList();
            return View(cmt);
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
        
        public ActionResult Logout()
        {
            Session["userid"] = null;
            return RedirectToAction("Index", "posts");
        }
    }
        
}