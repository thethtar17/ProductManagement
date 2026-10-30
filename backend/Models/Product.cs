namespace management.Models
{
  
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Default value
        public int UserId { get; set; } // Foreign key
        public User? User { get; set; }  // Navigation property

        public byte[]? ImageData { get; set; } // Binary data for the image
        public string? ImageMimeType { get; set; } // Mime type (e.g., "image/jpeg")
        public ICollection<CartItem> CartItems { get; set; }
    }
}
