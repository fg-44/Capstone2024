namespace PathNatureEcommerceCapstone.Models.DatabaseCapstone
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ShippingDetails
    {
        [Key]
        public int ShippingDetailID { get; set; }

        public int FK_ShippingDetails_MembersID { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(500)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Cityis required")]
        [StringLength(500)]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required")]
        [StringLength(500)]
        public string State { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [StringLength(50)]
        public string Country { get; set; }

        [Required(ErrorMessage = "Zip code is required")]
        [Display(Name = "Zip Code")]
        [StringLength(50)]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "Country is required")]
        public int? OrderID { get; set; }

        [Display(Name = "Totale da Pagare")]
        public decimal? AmountPaid { get; set; }

        [Display(Name = "Tipo di pagamento")]
        [StringLength(50)]
        public string PaymentType { get; set; }

        public virtual Members Members { get; set; }
    }
}
