using PathNatureEcommerceCapstone.Models.DatabaseCapstone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PathNatureEcommerceCapstone.Models.ViewModel
{

    public class ItemCartViewModel
    {
        public List<ItemCart> CartItems { get; set; }
        public List<Products> Products { get; set; }
        public int ProductId { get; set; } // ID del prodotto nel carrello
        public string ProductName { get; set; } // Nome del prodotto
        public int Quantity { get; set; } // Quantità del prodotto nel carrello
        public decimal Price { get; set; } // Prezzo del prodotto
        public decimal TotalPrice { get { return Quantity * Price; } } // Prezzo totale per quel prodotto nel carrello
        public decimal  AmountPaid { get; set; } // Importo pagato
        public string UserID { get; set; } // proprietà per associare l'articolo all'utente
        public string PaymentType { get; set; } // Tipo di pagamento
    }

    public class ShippingDetailsViewModel
    {
        public int MemberID { get; set; } // ID del membro destinatario
        public string Address { get; set; } // Indirizzo di spedizione
        public string City { get; set; } // Città di spedizione
        public string State { get; set; } // Stato di spedizione
        public string Country { get; set; } // Paese di spedizione
        public string ZipCode { get; set; } // Codice postale di spedizione
        public string PaymentType { get; set; } // Tipo di pagamento
    }


}
