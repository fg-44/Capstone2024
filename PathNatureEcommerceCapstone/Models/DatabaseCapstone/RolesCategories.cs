namespace PathNatureEcommerceCapstone.Models.DatabaseCapstone
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RolesCategories
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RolesCategories()
        {
            RoleCategoryMappings = new HashSet<RoleCategoryMappings>();
        }

        [Key]
        public int RoleCategoriesID { get; set; }

        [Required]
        [StringLength(50)]
        public string RoleCategoriesName { get; set; }

        [StringLength(500)]
        public string RoleCategoriesDescription { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RoleCategoryMappings> RoleCategoryMappings { get; set; }
    }
}
