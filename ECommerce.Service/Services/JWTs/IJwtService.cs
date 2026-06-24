using ECommerce.Domain.Entities;

namespace ECommerce.Service.Services.JWTs;

public interface IJwtService
{
    string GenerateToken(User user);
}
