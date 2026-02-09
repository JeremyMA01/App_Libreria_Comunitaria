using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

// App_Libreria_Comunitaria/Models/Donation.cs
namespace App_Libreria_Comunitaria.Models
{
    public class Donation
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public string Estado { get; set; } = "Nuevo";
        public DateTime DonationDate { get; set; } = DateTime.Now;
        public string DonatedBy { get; set; } 
        public bool ConvertedToInventory { get; set; } = false;
        public bool Active { get; set; } = true;

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }
    }
}   