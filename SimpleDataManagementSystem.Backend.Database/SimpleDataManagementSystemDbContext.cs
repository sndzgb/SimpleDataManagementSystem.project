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
            modelBuilder.Entity<UserEntity>()
                .HasIndex(x => x.RoleId)
                .IsUnique(false);

            // TODO put constraint on rest of string lengths...
            modelBuilder.Entity<UserEntity>()
                .Property(p => p.Username)
                .HasMaxLength(16);

            modelBuilder.Entity<CategoryEntity>()
                .HasMany(e => e.Items)
                .WithOne(x => x.Category)
                .HasForeignKey(e => e.Kategorija)
                .IsRequired();

            modelBuilder.Entity<ItemEntity>()
                .Property(p => p.URLdoslike)
                .HasMaxLength(512)
                .IsRequired(false);

            modelBuilder.Entity<ItemEntity>()
                .HasOne(x => x.Category)
                .WithMany(x => x.Items)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ItemEntity>()
                .HasOne(x => x.Retailer)
                .WithMany(x => x.Items)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ItemEntity> Items { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<RetailerEntity> Retailers { get; set; }
    }
}
