namespace management.Services
{
    public interface ICartService
    {
        Task AddToCartAsync(string userId, int productId, int quantity);
    }
}
