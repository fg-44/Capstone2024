namespace PathNatureEcommerceCapstone.Models.DatabaseCapstone
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PasswordRecoveryTokens
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PasswordRecoveryTokens()
        {
            PasswordRecoveryRequests = new HashSet<PasswordRecoveryRequests>();
        }

        [Key]
        public int TokenID { get; set; }

        public int FK_PasswordRecoveryTokens_MembersID { get; set; }

        [Required]
        [StringLength(100)]
        public string Token { get; set; }

        public DateTime ExpirationTime { get; set; }

        public DateTime? CreatedAt { get; set; }

        public virtual Members Members { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PasswordRecoveryRequests> PasswordRecoveryRequests { get; set; }
    }
}
