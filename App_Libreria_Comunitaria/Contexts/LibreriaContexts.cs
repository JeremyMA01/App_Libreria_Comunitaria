using App_Libreria_Comunitaria.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace App_Libreria_Comunitaria.Contexts
{
    public class LibreriaContexts : DbContext
    {
        public DbSet<Book> books { get; set; }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Review> reviews { get; set; }

        public DbSet<Message> Messages { get; set; } = null!;

        public DbSet<Category> Categories { get; set; }
        public DbSet<Donation> Donations { get; set; }


        public LibreriaContexts() : base() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=AppLibreriaBD;User ID=sa;Password=sa123;Trusted_Connection=False;MultipleActiveResultSets=True;TrustServerCertificate=True;");
        }
        public LibreriaContexts(DbContextOptions<LibreriaContexts> options) : base(options) { }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(b => b.Id);

                entity.HasOne(b => b.Genre)
                .WithMany(g => g.Books)
                .HasForeignKey(m => m.GenreId);
            });
            
            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.HasOne(r => r.Book)
                 .WithMany(b => b.Review)
                 .HasForeignKey(r => r.Id_Book);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
                entity.Property(c => c.Description).HasMaxLength(500);
                entity.Property(c => c.CreatedDate).HasDefaultValueSql("GETDATE()");
                entity.Property(c => c.Active).HasDefaultValue(true);
            });

            
            modelBuilder.Entity<Donation>(entity =>
            {
                entity.HasKey(d => d.Id);

                entity.Property(d => d.Title).IsRequired().HasMaxLength(200);
                entity.Property(d => d.Author).IsRequired().HasMaxLength(200);
                entity.Property(d => d.DonatedBy).IsRequired().HasMaxLength(100);
                entity.Property(d => d.Estado).HasMaxLength(20);
                entity.Property(d => d.DonationDate).HasDefaultValueSql("GETDATE()");
                entity.Property(d => d.ConvertedToInventory).HasDefaultValue(false);

                
                entity.HasOne(d => d.Category)
                    .WithMany() 
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            base.OnModelCreating(modelBuilder);
        }
        

    }

}
