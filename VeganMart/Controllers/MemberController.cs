using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VeganMart.Controllers
{
    public class MemberController : Controller
    {        
        public ActionResult AccountInformation()
        {
            return View();
        }
        public ActionResult ChangePassword()
        {
            return View();
        }
        public ActionResult MyOrder()
        {
            return View();
        }
        public ActionResult MyOrderDetail()
        {
            return View();
        }
    }
}