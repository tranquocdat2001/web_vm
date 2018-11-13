using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VeganMart.Controllers
{
    public class ShoppingCartController : Controller
    {
        public ActionResult Cart()
        {
            return View();
        }
        public ActionResult CheckOut()
        {
            return View();
        }
        public ActionResult CheckOrder()
        {
            return View();
        }

        public ActionResult PaymentEnd()
        {
            return View();
        }
    }
}