using ECommerce.API.Filters;
using ECommerce.Domain.DTOs;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Helper;
using ECommerce.Service.Services.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/role")]
[Authorize]
public class RoleController([FromBody] IRoleService roleService): ControllerBase
{
    [HttpPost("create-role")]
    [HasPermission(Permission.RolesManage)]
    public async Task<ResponseModel<RoleDto>> CreateRoleAsync([FromBody] RoleCreateDto dto) =>
        await roleService.AddRoleAsync(dto);

    [HttpGet("get-all-role")]
    [HasPermission(Permission.RolesManage)]
    public async Task<TableResponse<List<RoleDto>>> GetAllAsync([FromQuery]TableOptions tableoptions) =>
        await roleService.GetAllRolesAsync(tableoptions);

    [HttpGet("get-role-by-id/{id}")]
    [HasPermission(Permission.RolesManage)]
    public async Task<ResponseModel<RoleDto>> GetByIdAsync([FromRoute]int id) =>
        await roleService.GetRoleByIdAsync(id);

    [HttpPut("update-role/{id}")]
    [HasPermission(Permission.RolesManage)]
    public async Task<ResponseModel<RoleDto>> UpdateAsync([FromBody] RoleUpdateDto dto, [FromRoute] int id) =>
        await roleService.UpdateRoleAsync(id, dto);

    [HttpDelete("delete-role/{id}")]
    [HasPermission(Permission.RolesManage)]
    public async Task<ResponseModel<bool>> DeleteAsync([FromRoute] int id) =>
        await roleService.DeleteRoleAsync(id);

    [HttpPut("update-permission/{roleId}")]
    [HasPermission(Permission.RolesManage)]
    public async Task<ResponseModel<RoleDto>> UpdatePermissionAsync([FromRoute] int roleId, List<Permission> permissions) =>
        await roleService.UpdatePermissionsAsync(roleId, permissions);

    [HttpPut("give-permission/{roleId}")]
    [HasPermission(Permission.RolesManage)]
    public async Task<ResponseModel<RoleDto>> GivePermissionAsync([FromRoute] int roleId, [FromBody] Permission permission) =>
        await roleService.GivePermissionAsync(roleId, permission);

    [HttpPut("remove-permission/{roleId}")]
    [HasPermission(Permission.RolesManage)]
    public async Task<ResponseModel<RoleDto>> RemovePermissionAsync([FromRoute] int roleId, [FromBody] Permission permission) =>
        await roleService.RevokePermissionAsync(roleId, permission);

    [HttpPut("remove-all-permission/{roleId}")]
    [HasPermission(Permission.RolesManage)]
    public async Task<ResponseModel<bool>> RemoveAllPermissionAsync([FromRoute] int roleId) =>
        await roleService.RevokeAllPermissionsAsync(roleId);
}
