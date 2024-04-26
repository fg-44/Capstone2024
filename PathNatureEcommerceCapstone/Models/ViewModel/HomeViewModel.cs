using PathNatureEcommerceCapstone.Models.DatabaseCapstone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PathNatureEcommerceCapstone.Models.ViewModel
{
    public class HomeViewModel
    {
        private PathNatureDbEcommerce db = new PathNatureDbEcommerce();
        public IEnumerable<Products> ListOfProducts { get; set; }
        public HomeViewModel CreateModel()
        {
            ListOfProducts = db.Products.ToList();
            return this;
        }

        //public HomeViewModel CreateModel(string search, int? page)
        //{
        //    // Impostiamo il numero di elementi per pagina
        //    int pageSize = 3;

        //    // Calcoliamo l'indice del primo elemento sulla pagina corrente
        //    int pageNumber = (page ?? 1);

        //    // Calcoliamo l'indice del primo elemento sulla pagina corrente
        //    int skip = (pageNumber - 1) * pageSize;

        //    // Query per ottenere i prodotti filtrati per la ricerca
        //    IQueryable<Products> query = db.Products;

        //    if (!string.IsNullOrEmpty(search))
        //    {
        //        // Modifica la logica di ricerca per cercare il nome del prodotto che contiene la stringa di ricerca
        //        query = query.Where(p => p.ProductName.Contains(search));
        //    }

        //    // Eseguiamo la query per ottenere i risultati paginati
        //    ListOfProducts = query
        //                        .OrderBy(p => p.ProductID)
        //                        .Skip(skip)
        //                        .Take(pageSize)
        //                        .ToList();

        //    return this;
        //}
    }

}


