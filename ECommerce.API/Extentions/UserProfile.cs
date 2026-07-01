using System.Security.Claims;

namespace ECommerce.API.Extentions;

public static class UserProfile
{
    public static int GetUserId(this ClaimsPrincipal user)
    {
        var value = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(value, out var id) ?  id : 0;
    }

    public static string? GetRole(this ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.Role)?.Value;        
    }

    public static string? GetPhoneNumber(this ClaimsPrincipal user)
    { 
        return user.FindFirst(ClaimTypes.MobilePhone)?.Value;
    }

    public static string? GetFirstName(this ClaimsPrincipal user)
    { 
        return user.FindFirst(ClaimTypes.GivenName)?.Value;
    }

    public static string? GetLastName(this ClaimsPrincipal user)
    { 
        return user.FindFirst(ClaimTypes.Surname)?.Value;
    }

    public static List<string> GetPermissions(this ClaimsPrincipal user)
    {
        return user.FindAll("Permission").Select(c => c.Value).ToList();
    }
}
