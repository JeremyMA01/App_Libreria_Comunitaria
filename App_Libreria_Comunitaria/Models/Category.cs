using System;
using System.Collections.Generic;
using System.Text;

namespace App_Libreria_Comunitaria.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool Active { get; set; } = true;

        
        //public List<Book> Books { get; set; } = new();
    }
}