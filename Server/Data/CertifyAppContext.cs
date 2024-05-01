using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Certify.Server.Models.CertifyApp;

namespace Certify.Server.Data
{
    public partial class CertifyAppContext : DbContext
    {
        public CertifyAppContext()
        {
        }

        public CertifyAppContext(DbContextOptions<CertifyAppContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Certify.Server.Models.CertifyApp.Customer>()
              .HasOne(i => i.User)
              .WithMany(i => i.Customers)
              .HasForeignKey(i => i.CreatedBy)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<Certify.Server.Models.CertifyApp.Customer>()
              .HasOne(i => i.User1)
              .WithMany(i => i.Customers1)
              .HasForeignKey(i => i.ModifiedBy)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<Certify.Server.Models.CertifyApp.Product>()
              .HasOne(i => i.User)
              .WithMany(i => i.Products)
              .HasForeignKey(i => i.CreatedBy)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<Certify.Server.Models.CertifyApp.Product>()
              .HasOne(i => i.Customer)
              .WithMany(i => i.Products)
              .HasForeignKey(i => i.CustomerId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<Certify.Server.Models.CertifyApp.Product>()
              .HasOne(i => i.User1)
              .WithMany(i => i.Products1)
              .HasForeignKey(i => i.ModifiedBy)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<Certify.Server.Models.CertifyApp.RoleMenuMapping>()
              .HasOne(i => i.Menu)
              .WithMany(i => i.RoleMenuMappings)
              .HasForeignKey(i => i.MenuId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<Certify.Server.Models.CertifyApp.RoleMenuMapping>()
              .HasOne(i => i.Role)
              .WithMany(i => i.RoleMenuMappings)
              .HasForeignKey(i => i.RoleId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<Certify.Server.Models.CertifyApp.Store>()
              .HasOne(i => i.User)
              .WithMany(i => i.Stores)
              .HasForeignKey(i => i.CreatedBy)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<Certify.Server.Models.CertifyApp.Store>()
              .HasOne(i => i.User1)
              .WithMany(i => i.Stores1)
              .HasForeignKey(i => i.ModifiedBy)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<Certify.Server.Models.CertifyApp.User>()
              .HasOne(i => i.User1)
              .WithMany(i => i.Users1)
              .HasForeignKey(i => i.CreatedBy)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<Certify.Server.Models.CertifyApp.User>()
              .HasOne(i => i.User2)
              .WithMany(i => i.Users2)
              .HasForeignKey(i => i.ModifiedBy)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<Certify.Server.Models.CertifyApp.User>()
              .HasOne(i => i.Role)
              .WithMany(i => i.Users)
              .HasForeignKey(i => i.RoleId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<Certify.Server.Models.CertifyApp.User>()
              .HasOne(i => i.Store)
              .WithMany(i => i.Users)
              .HasForeignKey(i => i.StoreId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<Certify.Server.Models.CertifyApp.Customer>()
              .Property(p => p.CreatedOn)
              .HasColumnType("datetime");

            builder.Entity<Certify.Server.Models.CertifyApp.Customer>()
              .Property(p => p.ModifiedOn)
              .HasColumnType("datetime");

            builder.Entity<Certify.Server.Models.CertifyApp.Product>()
              .Property(p => p.CreatedOn)
              .HasColumnType("datetime");

            builder.Entity<Certify.Server.Models.CertifyApp.Product>()
              .Property(p => p.ModifiedOn)
              .HasColumnType("datetime");

            builder.Entity<Certify.Server.Models.CertifyApp.Store>()
              .Property(p => p.CreatedOn)
              .HasColumnType("datetime");

            builder.Entity<Certify.Server.Models.CertifyApp.Store>()
              .Property(p => p.ModifiedOn)
              .HasColumnType("datetime");

            builder.Entity<Certify.Server.Models.CertifyApp.User>()
              .Property(p => p.CreatedOn)
              .HasColumnType("datetime");

            builder.Entity<Certify.Server.Models.CertifyApp.User>()
              .Property(p => p.ModifiedOn)
              .HasColumnType("datetime");
            this.OnModelBuilding(builder);
        }

        public DbSet<Certify.Server.Models.CertifyApp.Customer> Customers { get; set; }

        public DbSet<Certify.Server.Models.CertifyApp.Menu> Menus { get; set; }

        public DbSet<Certify.Server.Models.CertifyApp.Product> Products { get; set; }

        public DbSet<Certify.Server.Models.CertifyApp.Role> Roles { get; set; }

        public DbSet<Certify.Server.Models.CertifyApp.RoleMenuMapping> RoleMenuMappings { get; set; }

        public DbSet<Certify.Server.Models.CertifyApp.Store> Stores { get; set; }

        public DbSet<Certify.Server.Models.CertifyApp.User> Users { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
    
    }
}