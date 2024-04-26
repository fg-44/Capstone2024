using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using PathNatureEcommerceCapstone.Models.DatabaseCapstone;
using PathNatureEcommerceCapstone.Models.Filtri;
using PathNatureEcommerceCapstone.Models.ViewModel;

namespace PathNatureEcommerceCapstone.Controllers.Account.SignUp
{
    public class SignupController : Controller
    {
        private PathNatureDbEcommerce db = new PathNatureDbEcommerce();

        // GET: Signup
        public ActionResult Index()
        {
            var members = db.Members.Include(m => m.Roles);
            return View(members.ToList());
        }

        // Sign up -------------------------------------------------------------------
        public ActionResult SignUp()
        {
            return View();
        }


        // POST SIGN UP       
        [HttpPost]
        [AllowAnonymous]
        [SetDateFilter]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SignUpClient(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Ottieni l'ID del ruolo "Client"
                    var roleId = GetRoleIdForRoleName("Client", db);

                    // Crea un nuovo membro e assegna l'ID del ruolo "Client"
                    var newMember = new Members
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Username = model.Username,
                        Email = model.Email,
                        Password = model.Password,
                        IsActive = true,
                        IsDeleted = false,
                        CreatedAt = DateTime.Now,
                        LastLogin = DateTime.Now,
                        FK_Member_RoleID = roleId // Assegna l'ID del ruolo "Client"
                    };

                    db.Members.Add(newMember);
                    await db.SaveChangesAsync();

                    // Aggiungi le informazioni di spedizione
                    var shippingDetails = new ShippingDetails
                    {
                        FK_ShippingDetails_MembersID = newMember.MemberID,
                        Address = model.Address,
                        City = model.City,
                        State = model.State,
                        Country = model.Country,
                        ZipCode = model.ZipCode,
                    };

                    db.ShippingDetails.Add(shippingDetails);
                    await db.SaveChangesAsync();

                    // Redirect to sign in page after successful sign up
                    return RedirectToAction("SignIn", "Account");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while signing up. Please try again.");
                    // Log the exception or perform additional error handling
                }
            }

            // If sign up fails or model validation fails, return to sign up page with error messages
            return View(model);
        }

        private int GetRoleIdForRoleName(string roleName, PathNatureDbEcommerce context)
        {
            var role = context.Roles.FirstOrDefault(r => r.RoleName == roleName);
            if (role != null)
            {
                return role.RoleID;
            }
            else
            {
                return -1; // Ritorna un valore negativo se il ruolo non viene trovato
            }
        }


        // GET: Signup/Details/5
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

        // GET: Signup/Create
        public ActionResult Create()
        {
            ViewBag.FK_Member_RoleID = new SelectList(db.Roles, "RoleID", "RoleName");
            return View();
        }

        // POST: Signup/Create
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

        // GET: Signup/Edit/5
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

        // POST: Signup/Edit/5
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

        // GET: Signup/Delete/5
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

        // POST: Signup/Delete/5
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
