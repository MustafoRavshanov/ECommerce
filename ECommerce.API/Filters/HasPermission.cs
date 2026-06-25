using ECommerce.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace ECommerce.API.Filters;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public class HasPermissionAttribute(Permission permission) : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        if (!user.Identity?.IsAuthenticated ?? true)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var permissionsClaim = user.FindFirst("permissions")?.Value;

        if (permissionsClaim is null)
        {
            context.Result = new ForbidResult();
            return;
        }

        var permissions = JsonSerializer.Deserialize<List<string>>(permissionsClaim);
        if (permissions is null || !permissions.Contains(permission.ToString()))
        {
            context.Result = new ForbidResult();
            return;
        }
    }
}
