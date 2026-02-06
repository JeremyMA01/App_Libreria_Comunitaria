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

        public LibreriaContexts() : base() { }
        public LibreriaContexts(DbContextOptions<LibreriaContexts> options)
            : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        {
        }
        
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
                 

            base.OnModelCreating(modelBuilder);
        }
        

    }

}
