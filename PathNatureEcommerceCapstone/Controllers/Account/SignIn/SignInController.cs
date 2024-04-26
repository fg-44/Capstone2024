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
using PathNatureEcommerceCapstone.Models.Filtri;

namespace PathNatureEcommerceCapstone.Controllers.Account.SignIn
{
    public class SignInController : Controller
    {
        private PathNatureDbEcommerce db = new PathNatureDbEcommerce();

        // GET: SignIn
        public ActionResult Index()
        {
            var members = db.Members.Include(m => m.Roles);
            return View(members.ToList());
        }

        // GET: SignIn/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Members members = db.Members.Find(id);
            if (members == null)
            {
                return HttpNotFound();
            }
            return View(members);
        }

        // GET: SignIn/Create
        public ActionResult Create()
        {
            ViewBag.FK_Member_RoleID = new SelectList(db.Roles, "RoleID", "RoleName");
            return View();
        }

        // POST: SignIn/Create
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MemberID,FK_Member_RoleID,FirstName,LastName,Username,Email,Password,IsActive,IsDeleted,CreatedAt,LastLogin")] Members members)
        {
            if (ModelState.IsValid)
            {
                db.Members.Add(members);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FK_Member_RoleID = new SelectList(db.Roles, "RoleID", "RoleName", members.FK_Member_RoleID);
            return View(members);
        }

        //--- SIGN IN CONTROLLER-------------------------------------------------------------------------------------
        //ROUTE[]
        public ActionResult SignIn()
        {
            return View("SignIn");
        }

        [HttpPost]
        [AllowAnonymous]
        [SetDateFilterAttribute]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(Members usersite)
        {
            var authenticatedUser = db.Members.FirstOrDefault(u => u.Username == usersite.Username && u.Password == usersite.Password);

            if (authenticatedUser != null)
            {
                try
                {
                    authenticatedUser.LastLogin = DateTime.Now;
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    // Gestisci eventuali eccezioni
                    ModelState.AddModelError("", "Si è verificato un errore durante l'aggiornamento di LastLoginDate.");
                    // Registra l'eccezione o esegui ulteriori gestioni degli errori
                }

                // Setta l'authentication cookie con il nome utente e il ruolo
                FormsAuthentication.SetAuthCookie(usersite.Username, false);

                // Redirect l'utente alla pagina del profilo appropriata in base al suo ruolo
                if (authenticatedUser.Roles != null && authenticatedUser.Roles.RoleName == "Client")
                {
                    // Ottieni l'ID dell'utente corrente
                    var userId = authenticatedUser.MemberID;
                    // Redirect all'azione Index del controller Home e passa l'ID dell'utente come parametro
                    return RedirectToAction("Index", "Home", new { MemberID = userId });
                }
                else if (authenticatedUser.Roles != null && authenticatedUser.Roles.RoleName == "Admin")
                {
                    // Se l'utente ha il ruolo di amministratore, reindirizzalo al dashboard dell'amministratore
                    return RedirectToAction("Dashboard", "LayoutPageAdminController", new { area = "", layout = "~/Views/Shared/_LayoutPageAdmin.cshtml" });
                }
            }

            // Se l'autenticazione fallisce o l'utente non viene trovato, torna alla pagina di accesso con un messaggio di errore
            ModelState.AddModelError("", "Username o Password errati");
            return RedirectToAction("Index", "Home");
        }



        //-----------------------------------------------------------------------------------------------------------

        // GET: SignIn/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Members members = db.Members.Find(id);
            if (members == null)
            {
                return HttpNotFound();
            }
            ViewBag.FK_Member_RoleID = new SelectList(db.Roles, "RoleID", "RoleName", members.FK_Member_RoleID);
            return View(members);
        }

        // POST: SignIn/Edit/5
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MemberID,FK_Member_RoleID,FirstName,LastName,Username,Email,Password,IsActive,IsDeleted,CreatedAt,LastLogin")] Members members)
        {
            if (ModelState.IsValid)
            {
                db.Entry(members).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FK_Member_RoleID = new SelectList(db.Roles, "RoleID", "RoleName", members.FK_Member_RoleID);
            return View(members);
        }

        // GET: SignIn/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Members members = db.Members.Find(id);
            if (members == null)
            {
                return HttpNotFound();
            }
            return View(members);
        }

        // POST: SignIn/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Members members = db.Members.Find(id);
            db.Members.Remove(members);
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
