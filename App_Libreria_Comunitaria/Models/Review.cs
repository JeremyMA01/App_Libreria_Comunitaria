using System;
using System.Collections.Generic;
using System.Text;

namespace App_Libreria_Comunitaria.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public int id_book { get; set; }
        public string User { get; set; }
        public int Score { get; set; }
        public string Comment { get; set; }
        public bool IsRecommend { get; set; }
        public DateTime PublishedDate { get; set; }
        public virtual Book Book { get; set; }
        //public virtual User User { get; set; } 
    }
}
