namespace PathNatureEcommerceCapstone.Models.DatabaseCapstone
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProductDiscounts
    {
        [Key]
        public int ProductDiscountID { get; set; }

        public int FK_ProductDiscounts_ProductID { get; set; }

        public int FK_ProductDiscounts_DiscountID { get; set; }

        public virtual Discounts Discounts { get; set; }

        public virtual Products Products { get; set; }
    }
}
