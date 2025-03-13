using Core.Entities;

namespace Core.Interfaces
{
    public interface IAuthService
    {
        Task<string> GenerateJwtToken(User user);
    }
}