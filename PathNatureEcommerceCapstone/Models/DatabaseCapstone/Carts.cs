namespace PathNatureEcommerceCapstone.Models.DatabaseCapstone
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Carts
    {
        [Key]
        public int CartID { get; set; }

        public int FK_Carts_ProductsID { get; set; }

        public int FK_Carts_MembersID { get; set; }

        public int FK_Carts_CartStatusesID { get; set; }

        public virtual CartStatuses CartStatuses { get; set; }

        public virtual Members Members { get; set; }

        public virtual Products Products { get; set; }
    }
}
