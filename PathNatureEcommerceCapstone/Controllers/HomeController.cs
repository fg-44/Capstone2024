using PathNatureEcommerceCapstone.Models.DatabaseCapstone;
using PathNatureEcommerceCapstone.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PathNatureEcommerceCapstone.Controllers
{
    public class HomeController : Controller
    {
        private PathNatureDbEcommerce db = new PathNatureDbEcommerce();
        public ActionResult Index()
        {
            var categories = db.Categories.ToList();
            var sidebarViewModel = new SidebarViewModel
            {
                CategoryNames = categories.Select(c => c.CategoryName).ToList(), // Modifica qui
                ListOfCategories = categories
            };

            var homeViewModel = new HomeViewModel
            {
                ListOfProducts = db.Products.ToList()
            };

            var combinedViewModel = new CombinedViewModel
            {
                SidebarViewModel = sidebarViewModel,
                HomeViewModel = homeViewModel
            };

            return View(combinedViewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //* CART ---------------------------------------------
        // FASE 1 DEL CHECKOUT
        [HttpPost]
        public ActionResult AddToCart(int productId)
        {
            // Verifica se l'utente è autenticato
            if (!User.Identity.IsAuthenticated)
            {
                // Se l'utente non è autenticato, restituisci un messaggio di errore
                return Json(new { success = false, message = "Per aggiungere prodotti al carrello, effettua prima il login." });
            }

            // Trova il prodotto nel database usando l'ID del prodotto
            var product = db.Products.Find(productId);
            if (product == null)
            {
                // Se il prodotto non è trovato, restituisci un errore 404 (Not Found)
                return HttpNotFound();
            }

            // Verifica se il prodotto è disponibile in magazzino
            if (product.Quantity < 1)
            {
                // Se il prodotto non è disponibile, restituisci un messaggio di errore
                return Json(new { success = false, message = "Il prodotto selezionato non è al momento disponibile." });
            }

            // Decrementa la quantità del prodotto disponibile in magazzino
            product.Quantity--;

            // Salva le modifiche nel database
            db.SaveChanges();

            // Ottieni il carrello dell'utente dalla sessione
            CartViewModel cart = Session["cart"] as CartViewModel;
            if (cart == null)
            {
                // Se il carrello non esiste ancora, crea un nuovo carrello vuoto
                cart = new CartViewModel
                {
                    CartItems = new List<ItemCartViewModel>()
                };
            }

            // Cerca se il prodotto è già presente nel carrello
            var existingItem = cart.CartItems.FirstOrDefault(item => item.ProductId == productId);
            if (existingItem != null)
            {
                // Se il prodotto è già presente nel carrello, incrementa la quantità
                existingItem.Quantity++;
            }
            else
            {
                // Se il prodotto non è presente nel carrello, aggiungilo al carrello
                cart.CartItems.Add(new ItemCartViewModel
                {
                    ProductId = product.ProductID,
                    ProductName = product.ProductName,
                    Price = (decimal)product.Price,
                    Quantity = 1
                });
            }

            // Salva il carrello aggiornato nella sessione
            Session["cart"] = cart;

            // Inizializza il numero totale di prodotti nel carrello
            var totalQuantity = cart.CartItems.Sum(item => item.Quantity);

            // Restituisci una risposta JSON con successo e il numero totale di prodotti nel carrello
            return Json(new { success = true, totalQuantity = totalQuantity });
        }

        [HttpPost]
        public ActionResult RemoveFromCart(int productId)
        {
            // Ottieni il carrello dalla sessione
            CartViewModel cart = Session["cart"] as CartViewModel;
            if (cart != null)
            {
                // Cerca l'elemento nel carrello
                var itemToRemove = cart.CartItems.FirstOrDefault(item => item.ProductId == productId);
                if (itemToRemove != null)
                {
                    // Aggiorna la quantità disponibile nel magazzino
                    var product = db.Products.Find(productId);
                    if (product != null)
                    {
                        product.Quantity += itemToRemove.Quantity;

                        // Verifica se la quantità disponibile supera un certo limite massimo
                        int maxStockQuantity = 100; // Esempio di limite massimo di magazzino
                        if (product.Quantity > maxStockQuantity)
                        {
                            product.Quantity = maxStockQuantity; // Limita la quantità disponibile al limite massimo consentito
                        }

                        // Salva le modifiche al database
                        db.SaveChanges();
                    }

                    // Rimuovi l'elemento dal carrello
                    cart.CartItems.Remove(itemToRemove);

                    // Salva il carrello aggiornato nella sessione
                    Session["cart"] = cart;
                }
            }
            else
            {
                // Se il carrello è null, crea un nuovo carrello
                cart = new CartViewModel
                {
                    CartItems = new List<ItemCartViewModel>()
                };
                Session["cart"] = cart;
            }

            // Calcola il totale della quantità nel carrello
            var totalQuantity = cart.CartItems.Sum(item => item.Quantity);

            // Restituisci i dati aggiornati del carrello come JSON
            return Json(new { success = true, totalQuantity });
        }

        public ActionResult GetCartItemCount()
        {
            CartViewModel cart = Session["cart"] as CartViewModel;
            int cartItemCount = 0;
            if (cart != null && cart.CartItems != null)
            {
                cartItemCount = cart.CartItems.Count();
            }
            return Json(new { cartItemCount });
        }

        //* CHECKOUT ------------------------------------------
        // FASE 2 DEL CHECKOUT
        [HttpGet]
        public ActionResult Checkout()
        {
            if (Session["cart"] == null)
            {
                // Se il carrello nella sessione non esiste, inizializzalo e assegnalo alla sessione
                var cart = new CartViewModel();
                var shipping = new ShippingDetailsViewModel();

                // Assegna i dettagli di spedizione al carrello
                cart.ShippingDetails = shipping;

                Session["cart"] = cart;
            }
            else
            {
                // Se il carrello esiste già ma non ha i dettagli di spedizione, inizializzali
                var cart = (CartViewModel)Session["cart"];
                if (cart.ShippingDetails == null)
                {
                    cart.ShippingDetails = new ShippingDetailsViewModel();
                }
            }


            // Continua con il checkout
            return View((CartViewModel)Session["cart"]); // Passa il modello al metodo View
        }


        [HttpPost]
        [Authorize]
        public ActionResult CheckoutID(CartViewModel model)
        {
            if (ModelState.IsValid)
            {
                string username = User.Identity.Name;
                var user = db.Members.FirstOrDefault(u => u.Username == username);
                if (user == null)
                {
                    Debug.WriteLine("L'utente non è stato trovato durante il checkout.");
                    return RedirectToAction("SignIn", "Account");
                }

                try
                {
                    Debug.WriteLine("Inizio del blocco try");

                    Debug.WriteLine("Member ID: " + model.ShippingDetails.MemberID);
                    Debug.WriteLine("Address: " + model.ShippingDetails.Address);

                    decimal totalAmount = CalculateTotalAmount(model);

                    var newOrder = new Carts
                    {
                        FK_Carts_MembersID = model.ShippingDetails.MemberID,
                        FK_Carts_CartStatusesID = GetCartStatusId("pagato"),
                    };
                    db.Carts.Add(newOrder);
                    db.SaveChanges();

                    Debug.WriteLine("Member ID del modello: " + model.ShippingDetails.MemberID);
                    Debug.WriteLine("Nuovo ID ordine creato: " + newOrder.CartID);

                    var newShippingDetails = new ShippingDetails
                    {
                        FK_ShippingDetails_MembersID = model.ShippingDetails.MemberID,
                        Address = model.ShippingDetails.Address,
                        City = model.ShippingDetails.City,
                        State = model.ShippingDetails.State,
                        Country = model.ShippingDetails.Country,
                        ZipCode = model.ShippingDetails.ZipCode,
                        OrderID = newOrder.CartID,
                        AmountPaid = totalAmount,
                        PaymentType = "Pagamento alla consegna",
                    };
                    db.ShippingDetails.Add(newShippingDetails);
                    db.SaveChanges();

                    Debug.WriteLine("Ordine salvato con successo. ID ordine: " + newOrder.CartID);
                    Debug.WriteLine("Dettagli di spedizione salvati con successo. ID ordine: " + newShippingDetails.OrderID);

                    Session["cart"] = null;

                    return RedirectToAction("CheckoutDetails", new { orderId = newOrder.CartID });
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Si è verificato un errore durante il processo di ordine: " + ex.Message);
                    ModelState.AddModelError("", "Si è verificato un errore durante il processo di ordine. Si prega di riprovare più tardi.");
                    return View("Checkout", model);
                }

            }

            return View("Checkout", model);
        }


        // Metodo per calcolare l'importo totale dell'ordine dal carrello
        private decimal CalculateTotalAmount(CartViewModel model)
        {
            decimal totalAmount = 0;
            foreach (var item in model.CartItems)
            {
                totalAmount += item.TotalPrice;
            }
            return totalAmount;
        }

        //OREDR DETAILS ------------------------------------------
        // FASE 3 DEL CHECKOUT

        public ActionResult CheckoutDetails()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CheckoutDetails(int orderId)
        {
            // Recuperare lo stato del carrello dal database utilizzando l'orderId
            var cartStatusId = db.Carts.FirstOrDefault(o => o.CartID == orderId)?.FK_Carts_CartStatusesID;

            if (cartStatusId.HasValue)
            {
                // Verifica se lo stato del carrello è "pagato" (assumendo che 1 sia l'ID dello stato "pagato")
                if (cartStatusId == 1)
                {
                    // L'ordine è stato completato, possiamo procedere con il recupero dei dettagli dell'ordine
                    var order = db.Carts.FirstOrDefault(o => o.CartID == orderId);
                    if (order == null)
                    {
                        // Gestire il caso in cui l'ordine non sia trovato
                        return RedirectToAction("Index", "Home"); // o qualunque altra azione adeguata
                    }

                    // Passiamo l'ordine alla vista
                    return View(order);
                }
            }

            // L'ordine non è stato completato, possiamo visualizzare un messaggio di errore o reindirizzare l'utente
            return RedirectToAction("Index", "Home"); // o visualizzare un messaggio di errore in una vista specifica
        }


        //  // METODO RECUPERO ID -----------------------------------------
        // Metodo per ottenere l'ID dello stato del carrello in base al nome dello stato
        public int GetCartStatusId(string statusName)
        {
            using (var db = new PathNatureDbEcommerce())
            {
                var cartStatus = db.CartStatuses.FirstOrDefault(s => s.StatusName == statusName);
                if (cartStatus != null)
                {
                    return cartStatus.CartStatusID;
                }
            }
            // Se lo stato del carrello non è trovato, gestire il caso appropriato
            return 0;
        }

        // -------------------------------------------------------

        [HttpPost]
        public ActionResult IncreaseQty(int productId)
        {
            // Recupera il prodotto dal database utilizzando l'ID del prodotto
            var product = db.Products.Find(productId);

            // Verifica se il prodotto esiste e se il carrello è stato inizializzato nella sessione
            if (product != null && Session["cart"] is CartViewModel cart)
            {
                // Verifica se il prodotto è già nel carrello
                var existingItem = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);

                if (existingItem != null)
                {
                    // Se il prodotto esiste già nel carrello, aumenta semplicemente la quantità
                    existingItem.Quantity++;

                    // Aggiorna il database: decrementa la quantità nel magazzino
                    product.Quantity--;
                    db.SaveChanges();

                    // Imposta il flag di successo su true
                    return Json(new { success = true });
                }
                else
                {
                    // Se il prodotto non esiste ancora nel carrello, crea un nuovo elemento di carrello
                    var newItem = new ItemCartViewModel
                    {
                        ProductId = product.ProductID,
                        ProductName = product.ProductName,
                        Quantity = 1,
                        Price = (decimal)product.Price
                    };
                    // Aggiungi il nuovo elemento di carrello al carrello
                    cart.CartItems.Add(newItem);

                    // Aggiorna il database: decrementa la quantità nel magazzino
                    product.Quantity--;
                    db.SaveChanges();

                    // Aggiorna il carrello nella sessione
                    Session["cart"] = cart;

                    // Imposta il flag di successo su true
                    return Json(new { success = true });
                }
            }

            // Se si è verificato un errore, restituisci un risultato JSON che indica che l'aggiunta al carrello non è riuscita
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult DecreaseQty(int productId)
        {
            // Recupera il carrello dalla sessione
            if (Session["cart"] is CartViewModel cart)
            {
                // Trova l'elemento nel carrello corrispondente all'ID del prodotto
                var item = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);
                if (item != null)
                {
                    // Riduci la quantità del prodotto nel carrello
                    if (item.Quantity > 1)
                    {
                        item.Quantity--;

                        // Aggiorna il database: incrementa la quantità nel magazzino
                        var product = db.Products.Find(productId);
                        if (product != null)
                        {
                            product.Quantity++;
                            db.SaveChanges();
                        }

                        // Aggiorna il carrello nella sessione
                        Session["cart"] = cart;

                        // Imposta il flag di successo su true
                        return Json(new { success = true });
                    }
                    else
                    {
                        // Se la quantità è 1, rimuovi completamente il prodotto dal carrello
                        cart.CartItems.Remove(item);

                        // Aggiorna il database: incrementa la quantità nel magazzino
                        var product = db.Products.Find(productId);
                        if (product != null)
                        {
                            product.Quantity++;
                            db.SaveChanges();
                        }

                        // Aggiorna il carrello nella sessione
                        Session["cart"] = cart;

                        // Imposta il flag di successo su true
                        return Json(new { success = true });
                    }
                }
            }

            // Se si è verificato un errore, restituisci un risultato JSON che indica che la rimozione dal carrello non è riuscita
            return Json(new { success = false });
        }


        [HttpPost]
        public ActionResult UpdateShippingDetails(ShippingDetailsViewModel shippingDetails)
        {
            // Verifica se l'utente è autenticato
            if (!User.Identity.IsAuthenticated)
            {
                // Se l'utente non è autenticato, reindirizzalo alla pagina di login
                return RedirectToAction("Login", "Account");
            }

            // Recupera il carrello dalla sessione
            CartViewModel cart = Session["cart"] as CartViewModel;

            // Verifica se il carrello esiste nella sessione
            if (cart == null)
            {
                // Se il carrello non esiste, reindirizza l'utente alla pagina del carrello
                return RedirectToAction("Index", "Cart");
            }

            // Aggiorna i dettagli di spedizione nel carrello
            cart.ShippingDetails = shippingDetails;

            // Salva il carrello aggiornato nella sessione
            Session["cart"] = cart;

            // Reindirizza l'utente alla pagina del carrello o alla fase successiva del checkout
            // In questo caso, reindirizza l'utente alla pagina successiva del checkout
            return RedirectToAction("CheckoutDetails", "Cart");
        }


        //* NAVBAR ---------------------------------------------

        private int GetCategoryId(string categoryName)
        {
            // Cerca nel database l'ID della categoria corrispondente al nome della categoria
            var category = db.Categories.FirstOrDefault(c => c.CategoryName == categoryName);

            // Se la categoria è stata trovata, restituisci il suo ID; altrimenti, restituisci un valore predefinito
            return category != null ? category.CategoryID : -1; // -1 è un valore di default per indicare che la categoria non è stata trovata
        }

        public ActionResult ProductsByCategory(string categoryName)
        {
            // Ottieni l'ID della categoria dal nome della categoria
            int categoryId = GetCategoryId(categoryName);

            // Se l'ID della categoria è valido, filtra i prodotti per quella categoria
            if (categoryId != -1)
            {
                var productsInCategory = db.Products.Where(p => p.FK_Products_CategoriesID == categoryId).ToList();
                return View(productsInCategory);
            }
            else
            {
                // Se l'ID della categoria non è valido, gestisci l'errore o reindirizza a una pagina di errore
                return RedirectToAction("Error");
            }
        }


    }

}