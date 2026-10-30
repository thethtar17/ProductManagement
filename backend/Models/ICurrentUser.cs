namespace management.Models
{
    public interface ICurrentUser
    {
        string UserId { get; }
        string Username { get; }
    }
}
