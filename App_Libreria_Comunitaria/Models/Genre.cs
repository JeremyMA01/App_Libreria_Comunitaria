using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace App_Libreria_Comunitaria.Models
{
    public class Genre
    {
        public int Id { get; set; } 
        public string Name { get; set; }    
        public bool Active { get; set; }


        [JsonIgnore]
        public virtual ICollection<Book> Books { get; set; }

    }
}
