namespace management.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public Product Product { get; set; } // Navigation property to Product
        public User User { get; set; } // Nav
      
    }
}
