using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PathNatureEcommerceCapstone.Models.DatabaseCapstone;

namespace PathNatureEcommerceCapstone.Controllers.Account.SignOut
{
    public class SignOutController : Controller
    {
        private PathNatureDbEcommerce db = new PathNatureDbEcommerce();


        //--- SIGN OUT ------------------------------------------------------------------------------------

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            TempData["Logout"] = "You are Disconnected!";
            Session.Clear();
            return RedirectToAction("SignIn", "SignIn");
        }

        //-------------------------------------------------------------------------------------------------

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
