using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PathNatureEcommerceCapstone.Controllers.Admin
{
    [Authorize(Roles = "AdminAccess")] // Autorizza solo gli utenti con il ruolo "Admin"
    public class LayoutPageAdminControllerController : Controller
    {
        // GET: LayoutPageAdminController
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "AdminAccess")] // Autorizza solo gli utenti con il ruolo "Admin"
        public ActionResult Dashboard()
        {
            return View();
        }
    }
}