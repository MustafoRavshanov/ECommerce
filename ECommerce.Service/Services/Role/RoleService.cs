using AutoMapper;
using ECommerce.Domain.DTOs;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Helper;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Net;
using RoleEntity = ECommerce.Domain.Entities.Role;

namespace ECommerce.Service.Services.Role;

public class RoleService(ApplicationDbContext applicationDbContext, IMapper mapper) : IRoleService
{
    public async Task<ResponseModel<RoleDto>> AddRoleAsync(RoleCreateDto roleCreateDto)
    {
        var existingRole = await applicationDbContext.Roles.FirstOrDefaultAsync(x => x.Name == roleCreateDto.Name);

        if (existingRole is not null)
            return ResponseModel<RoleDto>.Fail("Role with this name already exists", HttpStatusCode.Conflict);


        var entity =  mapper.Map<RoleEntity>(roleCreateDto);
        await applicationDbContext.AddAsync(entity);
        await applicationDbContext.SaveChangesAsync();

        foreach (var permission in roleCreateDto.Permissions)
        {
            await applicationDbContext.RolePermissions.AddAsync(new RolePermission
            {
                RoleId = entity.Id,
                Permission = permission
            });
        }

        var result = await applicationDbContext.SaveChangesAsync();

        if (result < 1)
            return ResponseModel<RoleDto>.Fail("Erro with saving to database", HttpStatusCode.InternalServerError);

        var roleDto=mapper.Map<RoleDto>(entity);

        return ResponseModel<RoleDto>.Success(roleDto, "Role created successfully",HttpStatusCode.Created);
    }

    public async Task<ResponseModel<bool>> DeleteRoleAsync(int roleId)
    {
        var entity= await applicationDbContext.Roles
            .Include(u=>u.Users)
            .Include(a=>a.RolePermissions)
            .FirstOrDefaultAsync(x=>x.Id == roleId);

        if (entity is null)
            return ResponseModel<bool>.Fail("this Role not found", HttpStatusCode.NotFound);

        if (entity.Users.Any())
            return ResponseModel<bool>.Fail("Can't remove roles which have users", HttpStatusCode.BadRequest);

        applicationDbContext.RolePermissions.RemoveRange(entity.RolePermissions);
        applicationDbContext.Roles.Remove(entity);  
        var result= await applicationDbContext.SaveChangesAsync();

        if (result < 1)
            return ResponseModel<bool>.Fail("Error with saving to database", HttpStatusCode.InternalServerError);

        return ResponseModel<bool>.Success(true, "Role removed successfully", HttpStatusCode.OK);
    }

    public async Task<TableResponse<List<RoleDto>>> GetAllRolesAsync(TableOptions options)
    {
        var entities=applicationDbContext.Roles
            .Include(u=>u.Users)
            .Include(a=>a.RolePermissions)
            .AsQueryable();

        var count=await applicationDbContext.Roles.CountAsync();

        var roles= await entities
            .Skip(options.First)
            .Take(options.Rows)
            .ToListAsync();

        var resultDtos= mapper.Map<List<RoleDto>>(roles);

        return new TableResponse<List<RoleDto>>() { Total=count, Items=resultDtos };
    }

    public async Task<ResponseModel<RoleDto>> GetRoleByIdAsync(int roleId)
    {
        var entity = applicationDbContext.Roles
            .Include(u => u.Users)
            .Include(a => a.RolePermissions)
            .FirstOrDefaultAsync(x => x.Id == roleId);

        if(entity is null)
            return ResponseModel<RoleDto>.Fail("Role not found", HttpStatusCode.NotFound);

        var roleDto=mapper.Map<RoleDto>(entity);

        return ResponseModel<RoleDto>.Success(roleDto, "Role retrieved successfully",HttpStatusCode.OK);
    }

    public async Task<ResponseModel<RoleDto>> UpdateRoleAsync(int roleId, RoleUpdateDto roleUpdateDto)
    {
        var entity = await applicationDbContext.Roles
            .Include(u => u.Users)
            .Include(a => a.RolePermissions)
            .FirstOrDefaultAsync(x => x.Id == roleId);

        if(entity is null)
            return ResponseModel<RoleDto>.Fail("Role not found", HttpStatusCode.NotFound);

        mapper.Map(roleUpdateDto, entity);
        var result = await applicationDbContext.SaveChangesAsync();

        if (result < 1)
            return ResponseModel<RoleDto>.Fail("Error with saving to database", HttpStatusCode.InternalServerError);

        var roleDto = mapper.Map<RoleDto>(entity);

        return ResponseModel<RoleDto>.Success(roleDto, "Role updated successfully", HttpStatusCode.OK);
    }

    public async Task<ResponseModel<RoleDto>> GivePermissionAsync(int roleId, Permission permission)
    {
        var entity = await applicationDbContext.Roles
            .Include(a => a.RolePermissions)
            .FirstOrDefaultAsync(x => x.Id == roleId);

        if (entity is null)
            return ResponseModel<RoleDto>.Fail("Role not found", HttpStatusCode.NotFound);

        if (entity.RolePermissions.Any(x => x.Permission == permission))
            return ResponseModel<RoleDto>.Fail("This permission already exists", HttpStatusCode.Conflict);

        entity.RolePermissions.Add(new RolePermission
        {
            RoleId=roleId,
            Permission=permission
        });

        var result = await applicationDbContext.SaveChangesAsync();

        if (result < 1)
            return ResponseModel<RoleDto>.Fail("Error with saving to database", HttpStatusCode.InternalServerError);

        var roleDto=mapper.Map<RoleDto>(entity);

        return ResponseModel<RoleDto>.Success(roleDto, "Permission saved successfully", HttpStatusCode.OK);
    }

    public async Task<ResponseModel<bool>> RevokeAllPermissionsAsync(int roleId)
    {
        var entity = await applicationDbContext.Roles
           .Include(a => a.RolePermissions)
           .FirstOrDefaultAsync(x => x.Id == roleId);

        if (entity is null)
            return ResponseModel<bool>.Fail("Role not found", HttpStatusCode.NotFound);

        if(!entity.RolePermissions.Any())
            return ResponseModel<bool>.Fail("Permissions already empty", HttpStatusCode.BadRequest);

        applicationDbContext.RolePermissions.RemoveRange(entity.RolePermissions);

        var result = await applicationDbContext.SaveChangesAsync();

        if (result < 1)
            return ResponseModel<bool>.Fail("Error with saving to database", HttpStatusCode.InternalServerError);

        return ResponseModel<bool>.Success(true, "Permissions removed successfully",HttpStatusCode.OK);
    }

    public async Task<ResponseModel<RoleDto>> RevokePermissionAsync(int roleId, Permission permission)
    {
        var entity = await applicationDbContext.Roles
           .Include(a => a.RolePermissions)
           .FirstOrDefaultAsync(x => x.Id == roleId);

        if (entity is null)
            return ResponseModel<RoleDto>.Fail("Role not found", HttpStatusCode.NotFound);

        if (!entity.RolePermissions.Any())
            return ResponseModel<RoleDto>.Fail("Permissions already empty", HttpStatusCode.BadRequest);

        var rolePermission=entity.RolePermissions.FirstOrDefault(x=>x.Permission==permission);

        if(rolePermission is null)
            return ResponseModel<RoleDto>.Fail("This permission doesn't exists", HttpStatusCode.NotFound);


        applicationDbContext.RolePermissions.Remove(rolePermission);
        var result = await applicationDbContext.SaveChangesAsync();

        if(result< 1)
            return ResponseModel<RoleDto>.Fail("Error with saving to database", HttpStatusCode.InternalServerError);

        var roleDto=mapper.Map<RoleDto>(entity);

        return ResponseModel<RoleDto>.Success(roleDto, "This permission removed successfully");
    }

    public async Task<ResponseModel<RoleDto>> UpdatePermissionsAsync(int roleId, List<Permission> permissions)
    {
        var entity = await applicationDbContext.Roles
          .Include(a => a.RolePermissions)
          .FirstOrDefaultAsync(x => x.Id == roleId);

        if (entity is null)
            return ResponseModel<RoleDto>.Fail("Role not found", HttpStatusCode.NotFound);

        applicationDbContext.RolePermissions.RemoveRange(entity.RolePermissions);

        foreach (Permission permission in permissions)
        {
            await applicationDbContext.RolePermissions.AddAsync(new RolePermission
            {
                RoleId = roleId,
                Permission = permission
            });
        }

        var result = await applicationDbContext.SaveChangesAsync();

        if (result < 1)
            return ResponseModel<RoleDto>.Fail("Error with saving to database", HttpStatusCode.InternalServerError);

        var roleDto = mapper.Map<RoleDto>(entity);

        return ResponseModel<RoleDto>.Success(roleDto, "Permissions updated successfully", HttpStatusCode.OK);
    }
}
