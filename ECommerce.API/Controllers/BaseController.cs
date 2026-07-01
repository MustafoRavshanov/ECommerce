using ECommerce.API.Extentions;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[ApiController]
public class BaseController : ControllerBase
{
    protected int CurrentUserId => User.GetUserId();
    protected string? CurrentUserRole => User.GetRole();
    protected string? PhoneNumber => User.GetPhoneNumber();
    protected string? FirstName => User.GetFirstName();
    protected string? LastName => User.GetLastName();
    protected List<string>? Permissions => User.GetPermissions();
}
