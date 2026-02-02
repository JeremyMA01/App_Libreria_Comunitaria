using Microsoft.EntityFrameworkCore;
using Proyecto_2Parcial.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto_2Parcial.Contexts
{
    public class BookContexts : DbContext
    {
        public DbSet<Book> books { get; set; }
        public DbSet<Genre> Genres { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=BookPoyecto;User ID=sa;Password=1234;Trusted_Connection=False;MultipleActiveResultSets=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Genre)
                .WithMany(g => g.Books)
                .HasForeignKey(m => m.GenreId);

            base.OnModelCreating(modelBuilder);
        }
        

    }

}
