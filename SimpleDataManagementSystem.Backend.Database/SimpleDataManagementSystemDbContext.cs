using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimpleDataManagementSystem.Backend.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Database
{
    public class SimpleDataManagementSystemDbContext : DbContext
    {
        public SimpleDataManagementSystemDbContext(DbContextOptions<SimpleDataManagementSystemDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            #region UserEntity

            modelBuilder.Entity<UserEntity>()
                .ToTable("Users", "dbo");

            modelBuilder.Entity<UserEntity>()
                .HasKey(x => x.Id)
                .HasName("PK_Users_Id");

            modelBuilder.Entity<UserEntity>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("int");

            modelBuilder.Entity<UserEntity>()
                .Property(x => x.CreatedUTC)
                .HasMaxLength(255);

            modelBuilder.Entity<UserEntity>()
                .Property(x => x.RoleId)
                .IsRequired(false);

            modelBuilder.Entity<UserEntity>()
                .HasOne(x => x.Role)
                .WithMany(x => x.Users)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<UserEntity>()
                .HasIndex(x => x.RoleId)
                .IsUnique(false);

            modelBuilder.Entity<UserEntity>()
                .Property(p => p.Username)
                .HasMaxLength(16);

            modelBuilder.Entity<UserEntity>()
                .HasIndex(x => x.Username)
                .HasDatabaseName("UQ_Users_Username")
                .IsUnique(true);

            modelBuilder.Entity<UserEntity>()
                .Property(x => x.PasswordHash)
                .HasColumnType("nvarchar")
                .HasMaxLength(255)
                .IsRequired(false);

            #endregion


            #region CategoryEntity

            modelBuilder.Entity<CategoryEntity>()
                .ToTable("Categories", "dbo");

            modelBuilder.Entity<CategoryEntity>()
                .HasMany(e => e.Items)
                .WithOne(x => x.Category)
                .HasForeignKey(e => e.Kategorija)
                .IsRequired();

            modelBuilder.Entity<CategoryEntity>()
                .HasKey(x => x.ID)
                .HasName("PK_Categories_ID");

            modelBuilder.Entity<CategoryEntity>()
                .Property(x => x.ID)
                .IsRequired(true)
                .ValueGeneratedOnAdd()
                .HasColumnType("int");

            modelBuilder.Entity<CategoryEntity>()
                .Property(x => x.Name)
                .HasColumnType("nvarchar")
                .HasMaxLength(255)
                .IsRequired(true);

            modelBuilder.Entity<CategoryEntity>()
                .HasIndex(x => x.Name)
                .HasDatabaseName("UQ_Categories_Name")
                .IsUnique(true);

            #endregion


            #region RolesEntity

            modelBuilder.Entity<RoleEntity>()
                .ToTable("Roles", "dbo");

            modelBuilder.Entity<RoleEntity>()
                .Property(x => x.Name)
                .HasColumnType("nvarchar")
                .HasMaxLength(255)
                .IsRequired(true);

            modelBuilder.Entity<RoleEntity>()
                .HasKey(x => x.Id)
                .HasName("PK_Roles_Id");

            modelBuilder.Entity<RoleEntity>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("int");

            modelBuilder.Entity<RoleEntity>()
                .Property(x => x.CreatedUTC)
                .HasColumnType("datetime2")
                .HasDefaultValue(DateTime.UtcNow);

            modelBuilder.Entity<RoleEntity>()
                .HasIndex(x => x.Name)
                .HasDatabaseName("UQ_Roles_Name")
                .IsUnique(true);

            #endregion


            #region RetailerEntity

            modelBuilder.Entity<RetailerEntity>()
                .ToTable("Retailers", "dbo");

            modelBuilder.Entity<RetailerEntity>()
                .Property(p => p.LogoImageUrl)
                .HasMaxLength(512)
                .IsRequired(false);

            modelBuilder.Entity<RetailerEntity>()
                .HasKey(x => x.ID)
                .HasName("PK_Retailers_ID");

            modelBuilder.Entity<RetailerEntity>()
                .Property(x => x.ID)
                .ValueGeneratedOnAdd()
                .HasColumnType("int");

            modelBuilder.Entity<RetailerEntity>()
                .Property(x => x.Name)
                .HasColumnType("nvarchar")
                .HasMaxLength(255)
                .IsRequired(true);

            modelBuilder.Entity<RetailerEntity>()
                .HasIndex(x => x.Name)
                .HasDatabaseName("UQ_Retailers_Name")
                .IsUnique(true);

            #endregion


            #region ItemEntity

            // TODO for future releases
            //modelBuilder.Entity<ItemEntity>()
            //    .HasKey(k => new { k.Nazivproizvoda, k.RetailerID })
            //    .HasName("PK_Items_Nazivproizvoda_RetailerID");

            modelBuilder.Entity<ItemEntity>()
                .ToTable("Items", "dbo");

            modelBuilder.Entity<ItemEntity>()
                .HasKey(k => k.Nazivproizvoda)
                .HasName("PK_Items_Nazivproizvoda");

            modelBuilder.Entity<ItemEntity>()
                .Property(x => x.Nazivproizvoda)
                .HasColumnType("nvarchar")
                .HasMaxLength(512)
                .IsRequired(true);
            
            modelBuilder.Entity<ItemEntity>()
                .Property(x => x.Nazivretailera)
                .HasColumnType("nvarchar")
                .HasMaxLength(512)
                .IsRequired(false);

            modelBuilder.Entity<ItemEntity>()
                .Property(x => x.Datumakcije)
                .HasColumnType("nvarchar")
                .HasMaxLength(512).IsRequired(false);

            modelBuilder.Entity<ItemEntity>()
                .Property(p => p.URLdoslike)
                .HasMaxLength(512)
                .IsRequired(false);

            modelBuilder.Entity<ItemEntity>()
                .Property(x => x.Kategorija)
                .IsRequired(false);

            modelBuilder.Entity<ItemEntity>()
                .HasOne(x => x.Category)
                .WithMany(x => x.Items)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<ItemEntity>()
                .HasOne(x => x.Retailer)
                .WithMany(x => x.Items)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ItemEntity>()
                .Property(p => p.Cijena)
                .HasPrecision(18, 2);

            #endregion
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ItemEntity> Items { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<RetailerEntity> Retailers { get; set; }
    }
}
