using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PathNatureEcommerceCapstone.Models.DatabaseCapstone;
using PathNatureEcommerceCapstone.Models.Filtri;

namespace PathNatureEcommerceCapstone.Controllers.Admin
{
    public class ProductsController : Controller
    {
        private PathNatureDbEcommerce db = new PathNatureDbEcommerce();

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Categories);
            return View(products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.FK_Products_CategoriesID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View();
        }

        [HttpPost]
        [SetDateFilterAttribute]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,FK_Products_CategoriesID,ProductName,IsActive,IsDeleted,CreatedDate,ModifiedDate,Description,ProductImage,Altdescription,IsFeatured,Quantity,Price")] Products products, HttpPostedFileBase ProductImageFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ProductImageFile != null && ProductImageFile.ContentLength > 0)
                    {
                        // Ottieni il nome del file
                        var fileName = Path.GetFileName(ProductImageFile.FileName);
                        // Salva il file nel percorso desiderato
                        var path = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                        ProductImageFile.SaveAs(path);
                        // Aggiorna il modello con il percorso dell'immagine salvata
                        products.ProductImage = fileName; // Memorizza solo il nome del file

                    }
                    else
                    {
                        // Se non è stata fornita una nuova immagine, mantieni l'immagine esistente nel database
                        products.ProductImage = "~/Uploads/default.jpg";
                    }

                    // Manipolazione manuale della data prima dell'inserimento nel database
                    products.CreatedDate = DateTime.Now; // Imposta la data di creazione sulla data e ora correnti
                    products.ModifiedDate = DateTime.Now; // Imposta la data di modifica sulla data e ora correnti

                    db.Products.Add(products);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (SqlException ex)
                {
                    // Gestione dell'eccezione per la violazione del vincolo UNIQUE KEY
                    if (ex.Number == 2601 || ex.Number == 2627) // Codici di errore per la violazione del vincolo di chiave unica
                    {
                        ModelState.AddModelError(string.Empty, "Il nome del prodotto è già presente nel database. Inserisci un nome unico.");
                        ViewBag.FK_Products_CategoriesID = new SelectList(db.Categories, "CategoryID", "CategoryName", products.FK_Products_CategoriesID);
                        return View(products);
                    }
                    else
                    {
                        throw; // Se l'eccezione non è relativa alla violazione del vincolo UNIQUE KEY, rilancia l'eccezione
                    }
                }
            }

            // Se si è verificato un errore durante la validazione del modello o il caricamento del file, impostiamo una ViewBag
            ViewBag.ErrorMessage = "Si è verificato un errore durante la creazione del prodotto. Assicurati di compilare tutti i campi obbligatori e di selezionare un file per l'immagine.";

            ViewBag.FK_Products_CategoriesID = new SelectList(db.Categories, "CategoryID", "CategoryName", products.FK_Products_CategoriesID);
            return View(products);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            ViewBag.FK_Products_CategoriesID = new SelectList(db.Categories, "CategoryID", "CategoryName", products.FK_Products_CategoriesID);
            return View(products);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,FK_Products_CategoriesID,ProductName,IsActive,IsDeleted,CreatedDate,ModifiedDate,Description,ProductImage,Altdescription,IsFeatured,Quantity,Price")] Products products, HttpPostedFileBase ProductImageFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Se è stata fornita una nuova immagine, caricala e aggiorna il percorso dell'immagine nel modello
                    if (ProductImageFile != null && ProductImageFile.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(ProductImageFile.FileName);
                        var path = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                        ProductImageFile.SaveAs(path);
                        products.ProductImage = fileName; // Aggiorna il percorso dell'immagine nel modello
                    }

                    // Recupera il prodotto esistente dal database utilizzando l'ID
                    var existingProduct = db.Products.Find(products.ProductID);

                    if (existingProduct != null)
                    {
                        // Sovrascrivi solo le proprietà necessarie
                        existingProduct.FK_Products_CategoriesID = products.FK_Products_CategoriesID;
                        existingProduct.ProductName = products.ProductName;
                        existingProduct.IsActive = products.IsActive;
                        existingProduct.IsDeleted = products.IsDeleted;
                        existingProduct.Description = products.Description;
                        existingProduct.Altdescription = products.Altdescription;
                        existingProduct.IsFeatured = products.IsFeatured;
                        existingProduct.Quantity = products.Quantity;
                        existingProduct.Price = products.Price;

                        // Se è stata fornita una nuova immagine, aggiorna anche il percorso dell'immagine
                        if (ProductImageFile != null && ProductImageFile.ContentLength > 0)
                        {
                            existingProduct.ProductImage = products.ProductImage; // Aggiorna il percorso dell'immagine nel prodotto esistente
                        }

                        // Imposta la data di modifica
                        existingProduct.ModifiedDate = DateTime.Now;

                        // Salva le modifiche nel database
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Prodotto non trovato.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Gestione dell'eccezione
                ModelState.AddModelError(string.Empty, "Si è verificato un errore durante il salvataggio delle modifiche. Riprova più tardi.");
                // Log dell'eccezione per il debug
                // Logger.LogException(ex);
            }

            // Aggiungi il SelectList per le categorie
            ViewBag.FK_Products_CategoriesID = new SelectList(db.Categories, "CategoryID", "CategoryName", products.FK_Products_CategoriesID);

            // Se si verificano errori, torna alla vista con il modello e gli errori di convalida
            return View(products);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Products products = db.Products.Find(id);
            db.Products.Remove(products);
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
