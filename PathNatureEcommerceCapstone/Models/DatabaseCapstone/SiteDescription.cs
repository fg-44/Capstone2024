namespace PathNatureEcommerceCapstone.Models.DatabaseCapstone
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SiteDescription")]
    public partial class SiteDescription
    {
        public int SiteDescriptionID { get; set; }

        [StringLength(500)]
        public string SiteDescriptionTitle { get; set; }

        [StringLength(500)]
        public string SiteDescPragraph { get; set; }

        public string SiteDescriptionImage { get; set; }
    }
}
