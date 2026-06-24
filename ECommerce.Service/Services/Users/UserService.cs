using AutoMapper;
using ECommerce.Domain.DTOs;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Helper;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Net;
using BC = BCrypt.Net.BCrypt;

namespace ECommerce.Service.Services.Users;

public class UserService(ApplicationDbContext applicationDbContext, IMapper mapper) : IUserService
{
    public async Task<ResponseModel<UserDto>> CreateUserAsync(UserCreateDto userCreateDto)
    {
        var existingUser = await applicationDbContext.Users.FirstOrDefaultAsync(u => u.PhoneNumber == userCreateDto.PhoneNumber);

        if (existingUser is not null) 
            return ResponseModel<UserDto>.Fail("User with this phone number already exists.", HttpStatusCode.Conflict);

        var entity = mapper.Map<User>(userCreateDto);
        await applicationDbContext.Users.AddAsync(entity);
        var result = await applicationDbContext.SaveChangesAsync();

        if ( result<1)
            return ResponseModel<UserDto>.Fail("User creation failed.", HttpStatusCode.InternalServerError);

        var createdUser = await applicationDbContext.Users
            .Include(u => u.Role)
            .FirstAsync(u => u.Id == entity.Id);

        var userDto= mapper.Map<UserDto>(createdUser);

        return ResponseModel<UserDto>.Success(userDto, "User created successfully", HttpStatusCode.Created);
    }

    public async Task<TableResponse<List<UserFullDto>>> GetAllUsersFullAsync(TableOptions options)
    {
        var entities = applicationDbContext.Users
            .Include(u => u.Role)
            .ThenInclude(a => a.RolePermissions)
            .AsQueryable();

        var count= await entities.CountAsync();

        var users = await entities
            .Skip(options.First)
            .Take(options.Rows)
            .ToListAsync();

        var resultDtos=mapper.Map<List<UserFullDto>>(users); 

        return new TableResponse<List<UserFullDto>>() { Total = count, Items = resultDtos };
    }

    public async Task<ResponseModel<UserFullDto>> GetUserFullByPhoneNumberAsync(string phoneNumber)
    { 
        var entities = await applicationDbContext.Users
            .Include(u => u.Role)
            .ThenInclude(a=>a.RolePermissions)
            .FirstOrDefaultAsync(x=>x.PhoneNumber == phoneNumber);

        if (entities is null)
            return ResponseModel<UserFullDto>.Fail("User with this phone number not found", HttpStatusCode.NotFound);

        var userDto= mapper.Map<UserFullDto>(entities);

        return ResponseModel<UserFullDto>.Success(userDto, "User retrieved successfully", HttpStatusCode.OK);
    }

    public async Task<ResponseModel<UserDto>> GetUserByIdAsync(int userId)
    {
        var entities = await applicationDbContext.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(x => x.Id == userId);

        if (entities is null)
            return ResponseModel<UserDto>.Fail("User with this id not found", HttpStatusCode.NotFound);

        var userDto = mapper.Map<UserDto>(entities);

        return ResponseModel<UserDto>.Success(userDto, "User retrieved successfully", HttpStatusCode.OK);
    }

    public async Task<ResponseModel<UserDto>> GetUserByPhoneNumberAsync(string phoneNumber)
    {
        var entities = await applicationDbContext.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);

        if (entities is null)
            return ResponseModel<UserDto>.Fail("User with this phone number not found", HttpStatusCode.NotFound);

        var userDto = mapper.Map<UserDto>(entities);

        return ResponseModel<UserDto>.Success(userDto, "User retrieved successfully", HttpStatusCode.OK);
    }

    public async Task<ResponseModel<UserFullDto>> GetUserFullByIdAsync(int userId)
    {
        var entities = await applicationDbContext.Users
          .Include(u => u.Role)
          .ThenInclude(a=>a.RolePermissions)
          .FirstOrDefaultAsync(x => x.Id == userId);

        if (entities is null)
            return ResponseModel<UserFullDto>.Fail("User with this id not found", HttpStatusCode.NotFound);

        var userDto = mapper.Map<UserFullDto>(entities);

        return ResponseModel<UserFullDto>.Success(userDto, "User retrieved successfully", HttpStatusCode.OK);
    }

    public async Task<ResponseModel<UserDto>> UpdateUserAsync(int userId, UserUpdateDto userUpdateDto)
    {
        var entity = await applicationDbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

        if (entity is null)
            return ResponseModel<UserDto>.Fail("User with this id not found", HttpStatusCode.NotFound);

        mapper.Map(userUpdateDto, entity);
        var result = await applicationDbContext.SaveChangesAsync();

        if(result<1)
            return ResponseModel<UserDto>.Fail("Error with saving to database", HttpStatusCode.InternalServerError);

        var dto= mapper.Map<UserDto>(entity);

        return ResponseModel<UserDto>.Success(dto, "User updated successfully", HttpStatusCode.OK);
    }

    public async Task<ResponseModel<bool>> UpdateUserPasswordAsync(int userId, UserUpdatePasswordDto userUpdatePasswordDto)
    {
        var entity=await applicationDbContext.Users.FirstOrDefaultAsync(x=>x.Id==userId);

        if (entity is null)
            return ResponseModel<bool>.Fail("User with this id not found", HttpStatusCode.NotFound);

        if (!BC.Verify(userUpdatePasswordDto.OldPassword, entity.Password))
            return ResponseModel<bool>.Fail("old parol is incorrect", HttpStatusCode.BadRequest);

        if (userUpdatePasswordDto.NewPassword != userUpdatePasswordDto.ConfirmPassword)
            return ResponseModel<bool>.Fail("Enter same code with new parol", HttpStatusCode.BadRequest);

        entity.Password=BC.HashPassword(userUpdatePasswordDto.NewPassword);

        var result= await applicationDbContext.SaveChangesAsync();

        if (result < 1)
            return ResponseModel<bool>.Fail("error with saving to database", HttpStatusCode.InternalServerError);

        return ResponseModel<bool>.Success(true, "New Password saved successfully", HttpStatusCode.OK);
    }
}
