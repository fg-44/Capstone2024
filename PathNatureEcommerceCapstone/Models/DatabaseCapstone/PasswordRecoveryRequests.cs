namespace PathNatureEcommerceCapstone.Models.DatabaseCapstone
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PasswordRecoveryRequests
    {
        [Key]
        public int RequestID { get; set; }

        public int FK_PasswordRecoveryRequests_TokensID { get; set; }

        [StringLength(100)]
        public string NewPassword { get; set; }

        public virtual PasswordRecoveryTokens PasswordRecoveryTokens { get; set; }
    }
}
