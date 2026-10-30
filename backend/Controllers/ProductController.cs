using System.Security.Claims;
using management.Data;
using management.Models;
using management.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace management.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ICartService _cartService;
        public ProductController(ApplicationDbContext context, ICartService cartService)
        {
            _context = context;
            _cartService = cartService;
        }

     

        [Authorize]
        [HttpGet("current-user-id")]
        public ActionResult<string> GetCurrentUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            return Ok(userId);
        }

        // Get all products for the current user
        [Authorize]
        [HttpGet("current-user-products")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetCurrentUserProducts()
        {
            // Get the current user's email from the claims
            //var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var userEmail = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userEmail == null)
            {
                return Unauthorized("User not authenticated.");
            }

          
            var userProducts = await _context.Products
                .Where(p => p.User.Email == userEmail)
               .Select(p => new
               {
                   p.Id,
                   p.Name,
                   p.Price,
                   p.Stock,
                   p.ImageData,
                   ImageUrl = $"{Request.Scheme}://{Request.Host}/api/products/{p.Id}/image" // Generate image URL
               })
               .ToListAsync();
            if (!userProducts.Any())
            {
                return NotFound("No products found for this user.");
            }

            return Ok(userProducts);
        }

 
   
        [HttpGet("{id}/image")]
        public IActionResult GetProductImage(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null || product.ImageData == null)
            {
                return NotFound(); // Or return a default image if preferred
            }

            return File(product.ImageData, product.ImageMimeType);
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetMyModels()
        {
            var products = await _context.Products
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Price,
                    p.Stock,
                    p.ImageData,
                  ImageUrl = $"{Request.Scheme}://{Request.Host}/api/products/{p.Id}/image" // Generate image URL
                })
                .ToListAsync();

            return Ok(products);
        }


        [Authorize]
        [HttpPost("add")]
        public async Task<ActionResult<Product>> AddProduct([FromForm] ProductCreateDto productDto)
        {
            if (productDto == null)
            {
                return BadRequest("Product data is null.");
            }

            var userId = User.FindFirstValue("userId");
            if (userId == null)
            {
                return Unauthorized("User not authenticated.");
            }
            if (!int.TryParse(userId, out var userIds))
            {
                return BadRequest("Invalid user ID.");
            }

            // Convert the uploaded image to a byte array
            byte[]? imageData = null;
            string? imageMimeType = null;
         
            if (productDto.Image != null && productDto.Image.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await productDto.Image.CopyToAsync(memoryStream);
                    imageData = memoryStream.ToArray();
                    imageMimeType = productDto.Image.ContentType;
                }
            }

            // Create the product and assign values
            var product = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                Stock = productDto.Stock,
                UserId = int.Parse(userId),  // Assuming userId is an integer
                ImageData = imageData,
                ImageMimeType = imageMimeType,
            };

            // Add the product to the database
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCurrentUserProducts), new { id = product.Id }, product);
        }


        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            // Get the current user's identifier (e.g., email or ID)
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized("User not authenticated.");
            }

            // Find the product by ID and ensure it belongs to the current user
            var product = await _context.Products
                .Where(p => p.User.Email== userId && p.Id == id)
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound("Product not found or you do not have permission to delete it.");
            }

            // Remove the product
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Product deleted successfully." });
        }


        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductCreateDto productDto)
        {
            if (productDto == null)
            {
                return BadRequest("Product data is null.");
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            // Update product properties
            product.Name = productDto.Name;
            product.Price = productDto.Price;
            product.Stock = productDto.Stock;

            // Update the image if a new image is provided
            if (productDto.Image != null && productDto.Image.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await productDto.Image.CopyToAsync(memoryStream);
                    product.ImageData = memoryStream.ToArray();  // Update image data
                }
            }

            // Save the changes
            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return NoContent(); // Return 204 No Content if the update is successful
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            var productDto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                ImageUrl = $"{Request.Scheme}://{Request.Host}/api/products/{product.Id}/image", // Generate image URL
                ImageData = product.ImageData // Send ImageData if required
            };

            return Ok(productDto);
        }


        [HttpPost("add-to-cart")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDto addToCartDto)
        {
            try
            {
                var userId = User.FindFirstValue("userId");

                await _cartService.AddToCartAsync(userId, addToCartDto.ProductId, addToCartDto.Quantity);
                return Ok(new { Message = "Item added to cart successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("current-user-cart")]
        public async Task<ActionResult<IEnumerable<CartProductDto>>> GetCurrentUserCart()
        {
            // Get the current user's email from the claims
            var userEmail = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userEmail == null)
            {
                return Unauthorized("User not authenticated.");
            }

            // Fetch the user's cart items
            var userCart = await _context.CartItems
                .Where(c => c.User.Email == userEmail)
                .Select(c => new
                {
                    c.Product.Id,
                    c.Product.Name,
                    c.Product.Price,
                    c.Product.Stock,
                    c.Product.ImageData,
                    c.Quantity,
                    ImageUrl = $"{Request.Scheme}://{Request.Host}/api/products/{c.Product.Id}/image" // Generate image URL
                })
                .ToListAsync();

            if (!userCart.Any())
            {
                return NotFound("Your cart is empty.");
            }

            return Ok(userCart);
        }

    }

}
