using App_Libreria_Comunitaria.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace App_Libreria_Comunitaria.Contexts
{
    public class ReviewContexts : DbContext
    {
        public DbSet<Review> reviews { get; set; }
        public DbSet<Book> books { get; set; }
        //public DbSet<User> users { get; set; }

        public ReviewContexts(DbContextOptions<ReviewContexts> options): base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=AppLibreriaDB;User ID=sa;Password=sa123;Trusted_Connection=False;MultipleActiveResultSets=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Review>()
                 .HasOne(r => r.Book)
                 .WithMany(b => b.Review)
                 .HasForeignKey(r => r.IdBook);

            /*
            modelBuilder.Entity<Review>()
                 .HasOne(r => r.User)
                 .HasMany(u => u.Review)
                 .HasForeignKey(r => r.IdUser); 
            */
        }




    }
}
