using PathNatureEcommerceCapstone.Models.DatabaseCapstone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PathNatureEcommerceCapstone.Models.ViewModel
{
    public class ItemCart
    {
        public Products Product { get; set; } // Prodotto nel carrello
        public int ProductId { get; set; } // ID del prodotto nel carrello
        public string ProductName { get; set; } // Nome del prodotto
        public int Quantity { get; set; } // Quantità del prodotto nel carrello
        public decimal Price { get; set; } // Prezzo del prodotto
        public decimal TotalPrice { get { return Price * Quantity; } }  // Prezzo totale del prodotto
        public decimal? AmountPaid { get; set; } // Importo pagato
    }

    public class CartViewModel
    {
        public List<ItemCartViewModel> CartItems { get; set; } // Lista degli elementi nel carrello
        public SidebarViewModel SidebarViewModel { get; set; } // Proprietà per la barra laterale
        public decimal TotalPrice { get; set; } // Prezzo totale del carrello
        public ShippingDetailsViewModel ShippingDetails { get; set; } // Informazioni di spedizione
        public int TotalQuantity { get { return CartItems.Sum(item => item.Quantity); } } // Numero totale di prodotti nel carrello
        // Proprietà per calcolare il totale dell'importo dell'ordine
        public decimal TotalAmount
        {
            get
            {
                if (CartItems != null)
                {
                    return CartItems.Sum(item => item.TotalPrice);
                }
                return 0; // Se non ci sono elementi nel carrello, restituisci 0 come totale
            }
        }
        // Costruttore per inizializzare la lista degli elementi nel carrello
        public CartViewModel()
        {
            CartItems = new List<ItemCartViewModel>();

        }

    }

}