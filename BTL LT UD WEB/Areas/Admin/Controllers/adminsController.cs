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
    
    public class adminsController : Controller
    {
        
        // GET: admins
        
        public ActionResult Index()
        {

            return View();
        }

      
        
    }
}

