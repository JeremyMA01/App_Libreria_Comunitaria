using System.ComponentModel.DataAnnotations;

namespace App_Libreria_Comunitaria.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string SenderName { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string RecipientName { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Subject { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Priority { get; set; } = "Normal";

        public bool IsUrgent { get; set; } = false;

        [Required, MaxLength(2000)]
        public string Content { get; set; } = string.Empty;

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public bool IsRead { get; set; } = false;
    }
}