using management.Data;
using management.Models;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace management.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;

        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

        //public async Task AddToCartAsync(string userId, int productId, int quantity)
        //{
        //    // Find the user and product
        //    var user = await _context.Users.FindAsync(userId);

        //    var product = await _context.Products.FindAsync(productId);

        //    if (user == null)
        //        throw new ArgumentException("User not found");

        //    if (product == null)
        //        throw new ArgumentException("Product not found");

        //    // Check if the cart item already exists for this user and product
        //    var existingCartItem = await _context.CartItems
        //        .FirstOrDefaultAsync(c => c.UserId == user.Id && c.ProductId == product.Id);

        //    if (existingCartItem != null)
        //    {
        //        // If the item exists, update the quantity
        //        existingCartItem.Quantity += quantity;
        //        _context.CartItems.Update(existingCartItem);
        //    }
        //    else
        //    {
        //        // If the item does not exist, create a new cart item
        //        var newCartItem = new CartItem
        //        {
        //            UserId = user.Id,
        //            ProductId = product.Id,
        //            Quantity = quantity
        //        };
        //        await _context.CartItems.AddAsync(newCartItem);
        //    }

        //    // Save changes to the database
        //    await _context.SaveChangesAsync();
        //}
        //public Task AddToCartAsync(string userId, int productId, int quantity)
        //{
        //    throw new NotImplementedException();
        //}
        //public async Task AddToCartAsync(string userId, int productId, int quantity)
        //{
        //    // Find the user and product
        //    var user = await _context.Users.FindAsync(userId);
        //    var product = await _context.Products.FindAsync(productId);

        //    if (user == null)
        //        throw new ArgumentException("User not found");

        //    if (product == null)
        //        throw new ArgumentException("Product not found");

        //    // Check if the cart item already exists for this user and product
        //    var existingCartItem = await _context.CartItems
        //        .FirstOrDefaultAsync(c => c.UserId == user.Id && c.ProductId == product.Id);

        //    if (existingCartItem != null)
        //    {
        //        // If the item exists, update the quantity
        //        existingCartItem.Quantity += quantity;
        //        _context.CartItems.Update(existingCartItem);
        //    }
        //    else
        //    {
        //        // If the item does not exist, create a new cart item
        //        var newCartItem = new CartItem
        //        {
        //            UserId = user.Id,
        //            ProductId = product.Id,
        //            Quantity = quantity
        //        };
        //        await _context.CartItems.AddAsync(newCartItem);
        //    }

        //    // Save changes to the database
        //    await _context.SaveChangesAsync();
        //}

        public async Task AddToCartAsync(string userId, int productId, int quantity)
        {
            // Convert userId from string to int
            if (!int.TryParse(userId, out var userIntId))
            {
                throw new ArgumentException("UserId must be a valid integer");
            }

            // Find the user by userId (now as an int)
            var user = await _context.Users.FindAsync(userIntId);
            var product = await _context.Products.FindAsync(productId);

            if (user == null)
                throw new ArgumentException("User not found");

            if (product == null)
                throw new ArgumentException("Product not found");

            // Check if the cart item already exists for this user and product
            var existingCartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.UserId == userIntId && c.ProductId == productId);

            if (existingCartItem != null)
            {
                // If the item exists, update the quantity
                existingCartItem.Quantity += quantity;
                _context.CartItems.Update(existingCartItem);
            }
            else
            {
                // If the item does not exist, create a new cart item
                var newCartItem = new CartItem
                {
                    UserId = userIntId,
                    ProductId = productId,
                    Quantity = quantity
                };
                await _context.CartItems.AddAsync(newCartItem);
            }

            // Save changes to the database
            await _context.SaveChangesAsync();
        }

    }
}
