using ECommerce.Domain.DTOs;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Helper;

namespace ECommerce.Service.Services.Role;

public interface IRoleService
{
    Task<ResponseModel<RoleDto>> AddRoleAsync(RoleCreateDto roleCreateDto);
    Task<ResponseModel<RoleDto>> GetRoleByIdAsync(int roleId);
    Task<TableResponse<List<RoleDto>>> GetAllRolesAsync(TableOptions options);
    Task<ResponseModel<RoleDto>> UpdateRoleAsync(int roleId, RoleUpdateDto roleUpdateDto);
    Task<ResponseModel<bool>> DeleteRoleAsync(int roleId);

    Task<ResponseModel<RoleDto>> UpdatePermissionsAsync(int roleId, List<Permission> permissions);
    Task<ResponseModel<RoleDto>> GivePermissionAsync(int roleId, Permission permission);
    Task<ResponseModel<RoleDto>> RevokePermissionAsync(int roleId, Permission permission);
    Task<ResponseModel<bool>> RevokeAllPermissionsAsync(int roleId);
}
