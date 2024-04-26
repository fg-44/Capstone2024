namespace PathNatureEcommerceCapstone.Models.DatabaseCapstone
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MemberRoles
    {
        [Key]
        public int MemberRoleID { get; set; }

        public int FK_MemberRoles_MembersID { get; set; }

        public int FK_MemberRoles_RolesID { get; set; }

        public virtual Members Members { get; set; }

        public virtual Roles Roles { get; set; }
    }
}
