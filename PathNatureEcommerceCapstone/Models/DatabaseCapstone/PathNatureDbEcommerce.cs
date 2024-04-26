using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace PathNatureEcommerceCapstone.Models.DatabaseCapstone
{
    public partial class PathNatureDbEcommerce : DbContext
    {
        public PathNatureDbEcommerce()
            : base("name=PathNatureDbEcommerce")
        {
        }

        public virtual DbSet<Carts> Carts { get; set; }
        public virtual DbSet<CartStatuses> CartStatuses { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Discounts> Discounts { get; set; }
        public virtual DbSet<MemberRoles> MemberRoles { get; set; }
        public virtual DbSet<Members> Members { get; set; }
        public virtual DbSet<PasswordRecoveryRequests> PasswordRecoveryRequests { get; set; }
        public virtual DbSet<PasswordRecoveryTokens> PasswordRecoveryTokens { get; set; }
        public virtual DbSet<ProductDiscounts> ProductDiscounts { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<RoleCategoryMappings> RoleCategoryMappings { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<RolesCategories> RolesCategories { get; set; }
        public virtual DbSet<ShippingDetails> ShippingDetails { get; set; }
        public virtual DbSet<SiteDescription> SiteDescription { get; set; }
        public virtual DbSet<SlideImages> SlideImages { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CartStatuses>()
                .HasMany(e => e.Carts)
                .WithRequired(e => e.CartStatuses)
                .HasForeignKey(e => e.FK_Carts_CartStatusesID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Categories>()
                .Property(e => e.CategoryImage)
                .IsUnicode(false);

            modelBuilder.Entity<Categories>()
                .HasMany(e => e.Products)
                .WithRequired(e => e.Categories)
                .HasForeignKey(e => e.FK_Products_CategoriesID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Categories>()
                .HasMany(e => e.RoleCategoryMappings)
                .WithOptional(e => e.Categories)
                .HasForeignKey(e => e.FK_RoleCategoryMap_CategoryID);

            modelBuilder.Entity<Discounts>()
                .HasMany(e => e.ProductDiscounts)
                .WithRequired(e => e.Discounts)
                .HasForeignKey(e => e.FK_ProductDiscounts_DiscountID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Members>()
                .HasMany(e => e.Carts)
                .WithRequired(e => e.Members)
                .HasForeignKey(e => e.FK_Carts_MembersID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Members>()
                .HasMany(e => e.MemberRoles)
                .WithRequired(e => e.Members)
                .HasForeignKey(e => e.FK_MemberRoles_MembersID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Members>()
                .HasMany(e => e.PasswordRecoveryTokens)
                .WithRequired(e => e.Members)
                .HasForeignKey(e => e.FK_PasswordRecoveryTokens_MembersID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Members>()
                .HasMany(e => e.ShippingDetails)
                .WithRequired(e => e.Members)
                .HasForeignKey(e => e.FK_ShippingDetails_MembersID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PasswordRecoveryTokens>()
                .HasMany(e => e.PasswordRecoveryRequests)
                .WithRequired(e => e.PasswordRecoveryTokens)
                .HasForeignKey(e => e.FK_PasswordRecoveryRequests_TokensID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Products>()
                .Property(e => e.ProductImage)
                .IsUnicode(false);

            modelBuilder.Entity<Products>()
                .Property(e => e.Altdescription)
                .IsUnicode(false);

            modelBuilder.Entity<Products>()
                .HasMany(e => e.Carts)
                .WithRequired(e => e.Products)
                .HasForeignKey(e => e.FK_Carts_ProductsID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Products>()
                .HasMany(e => e.ProductDiscounts)
                .WithRequired(e => e.Products)
                .HasForeignKey(e => e.FK_ProductDiscounts_ProductID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Roles>()
                .HasMany(e => e.MemberRoles)
                .WithRequired(e => e.Roles)
                .HasForeignKey(e => e.FK_MemberRoles_RolesID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Roles>()
                .HasMany(e => e.Members)
                .WithOptional(e => e.Roles)
                .HasForeignKey(e => e.FK_Member_RoleID);

            modelBuilder.Entity<RolesCategories>()
                .HasMany(e => e.RoleCategoryMappings)
                .WithOptional(e => e.RolesCategories)
                .HasForeignKey(e => e.FK_RoleCategoryMap_RoleCategoriesID);

            modelBuilder.Entity<SiteDescription>()
                .Property(e => e.SiteDescriptionImage)
                .IsUnicode(false);

            modelBuilder.Entity<SlideImages>()
                .Property(e => e.SlideImage)
                .IsUnicode(false);
        }
    }
}
