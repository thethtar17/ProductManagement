using System.Security.Claims;

namespace management.Models
{
    public class CurrentUser : ICurrentUser
    {
        public string UserId { get; private set; }
        public string Username { get; private set; }

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            var user = httpContextAccessor.HttpContext?.User;
            if (user != null)
            {
                UserId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                Username = user.FindFirstValue(ClaimTypes.Name);
            }
        }
    }
}
