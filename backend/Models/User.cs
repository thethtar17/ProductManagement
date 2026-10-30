using System.ComponentModel.DataAnnotations;

namespace management.Models
{
    public class User
    {
        public int Id { get; set; } // Primary key

        [Required]
        public string Username { get; set; } // User's username

        [Required]
        public string Email { get; set; } // User's email

        [Required]
        public string PasswordHash { get; set; } // Encrypted password

        public string Role { get; set; } = "USER"; // Default role

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;// Date of account creation

        public DateTime UpdatedAt { get; set; } // Date of last update

        // Navigation property
        public ICollection<Product> Products { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
    }
}
