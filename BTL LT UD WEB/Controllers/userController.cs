using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.Net;
using BTL_LT_UD_WEB.Models;
using PagedList;
using BTL_LT_UD_WEB.Security;
using System.Xml.Linq;
using System.Net.Mail;
using System.Web.Security;
using System.IO;

namespace BTL_LT_UD_WEB.Controllers
{
    public class userController : Controller
    {
        // GET: user
        private dbContect db = new dbContect();
        [NonAction]
        public bool IsEmailExist(string Email)
        {
            var v = db.users.Where(a => a.email == Email).FirstOrDefault();
            return v != null;
        }
        [NonAction]
        public void SendVerificationLinkEmail(string Email, string activationCode, string emailFor = "VerifyAccount")
        {
            var verifyUrl = "/user/" + emailFor + "/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("lethanh231120@gmail.com", "Dotnet Awesome");
            var toEmail = new MailAddress(Email);
            var fromEmailPassword = "dwvrcntiuzwxajsv"; 

            string subject = "";
            string body = "";
             if (emailFor == "ResetPassword")
            {
                subject = "Reset Password";
                body = "<h1>Hi,</h1><h3>We got request for reset your account password. Please click on the below link to reset your password</h3>" +
                    "<br/><a href=" + link + ">Reset Password link</a>";
            }
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
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(string Email)
        {
            //Verify Email ID
            //Generate Reset password link 
            //Send Email 
            string message = "";
            
                var user = db.users.Where(a => a.email == Email).FirstOrDefault();
                if (user != null)
                {
                    //Send email for reset password
                    string resetCode = Guid.NewGuid().ToString();
                    SendVerificationLinkEmail(user.email, resetCode, "ResetPassword");
                user.ResetPasswordCode = resetCode;
                    //This line I have added here to avoid confirm password not match issue , as we had added a confirm password property 
                    //in our model class in part 1
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                    message = "Reset password link has been sent to your email id ! Please check your email! ";
                }
                else
                {
                    message = "Account not found";
                }
            ViewBag.Message = message;
            return View();
        }

        public ActionResult ResetPassword(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return HttpNotFound();
            }
            var user = db.users.Where(a => a.ResetPasswordCode == id).FirstOrDefault();
            if (user != null)
            {
                ResetPasswordModel model = new ResetPasswordModel();
                model.ResetCode = id;
                return View(model);
            }
            else
            {
                return HttpNotFound();
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            var message = "";
            if (ModelState.IsValid)
            {              
                var user = db.users.Where(a => a.ResetPasswordCode == model.ResetCode).FirstOrDefault();
                if (user != null)
                {
                    user.password = Crypto.Hash(model.NewPassword);
                    user.ResetPasswordCode = "";
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                    message = "New password updated successfully";
                }                
            }
            else
            {
                message = "Something invalid";
            }
            ViewBag.Message = message;
            return Redirect("/Home/Login"); ;
        }

    }
}