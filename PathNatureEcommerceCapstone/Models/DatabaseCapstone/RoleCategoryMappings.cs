namespace PathNatureEcommerceCapstone.Models.DatabaseCapstone
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RoleCategoryMappings
    {
        [Key]
        public int MappingID { get; set; }

        public int? FK_RoleCategoryMap_RoleCategoriesID { get; set; }

        public int? FK_RoleCategoryMap_CategoryID { get; set; }

        public virtual Categories Categories { get; set; }

        public virtual RolesCategories RolesCategories { get; set; }
    }
}
