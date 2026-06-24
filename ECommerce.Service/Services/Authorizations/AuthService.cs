using AutoMapper;
using ECommerce.Domain.DTOs;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Helper;
using ECommerce.Infrastructure.Data;
using ECommerce.Service.Services.JWTs;
using Microsoft.EntityFrameworkCore;
using System.Net;
using BC = BCrypt.Net.BCrypt;

namespace ECommerce.Service.Services.Authorizations;

public class AuthService(ApplicationDbContext applicationDbContext, IJwtService jwtService, IMapper mapper) : IAuthService
{
    public async Task<ResponseModel<AuthResponseDto>> LoginAsync(LoginDto dto)
    {
        var user = await applicationDbContext.Users
            .Include(u => u.Role)
            .ThenInclude(a => a.RolePermissions)
            .FirstOrDefaultAsync(x => x.PhoneNumber == dto.PhoneNumber);

        if (user is null)
            return ResponseModel<AuthResponseDto>.Fail("PhoneNumber or Password incorrect", HttpStatusCode.Unauthorized);

        if (!BC.Verify(dto.Password, user.Password))
            return ResponseModel<AuthResponseDto>.Fail("PhoneNumber or Password incorrect", HttpStatusCode.Unauthorized);

        if (!user.IsActive)
            return ResponseModel<AuthResponseDto>.Fail("Your account isn't active", HttpStatusCode.Forbidden);

        var token = jwtService.GenerateToken(user);

        var responseDto = new AuthResponseDto
        {
            AccessToken = token,
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            UserDto = mapper.Map<UserDto>(user)
        };
        return ResponseModel<AuthResponseDto>.Success(responseDto, "You enter successfully", HttpStatusCode.OK);
    }

    public async Task<ResponseModel<AuthResponseDto>> RegisterAsync(RegisterDto dto)
    {
        var otp = await applicationDbContext.OtpCodes.FirstOrDefaultAsync(x => x.PhoneNumber == dto.PhoneNumber && x.IsUsed == true);

        if (otp is null)
            return ResponseModel<AuthResponseDto>.Fail("Phone number didn't confirmed", HttpStatusCode.BadRequest);

        var existingUser = await applicationDbContext.Users
            .Include(u => u.Role)
            .ThenInclude(a => a.RolePermissions)
            .FirstOrDefaultAsync(x => x.PhoneNumber == dto.PhoneNumber);

        if (existingUser is not null)
            return ResponseModel<AuthResponseDto>.Fail("User with this phone number already exists", HttpStatusCode.Conflict);

        if (dto.Password != dto.ConfirmedPassword)
            return ResponseModel<AuthResponseDto>.Fail("Passwords should be same", HttpStatusCode.BadRequest);

        var customerRole = await applicationDbContext.Roles
            .Include(u => u.RolePermissions)
            .FirstOrDefaultAsync(x => x.Name == "Customer");
        if (customerRole is null)
            return ResponseModel<AuthResponseDto>.Fail("Customer role doesn't exists", HttpStatusCode.InternalServerError);

        var user = mapper.Map<User>(dto);
        user.Password = BC.HashPassword(dto.Password);
        user.RoleId=customerRole.Id;
        user.IsActive = true;

        await applicationDbContext.Users.AddAsync(user);
        var result = await applicationDbContext.SaveChangesAsync();

        if (result < 1)
            return ResponseModel<AuthResponseDto>.Fail("Error with saving to database", HttpStatusCode.InternalServerError);

        var customer = new Customer { Id = user.Id };
        await applicationDbContext.Customers.AddAsync(customer);
        var result2 = await applicationDbContext.SaveChangesAsync();

        if (result2 < 1)
            return ResponseModel<AuthResponseDto>.Fail("Error with saving to database as Customer", HttpStatusCode.InternalServerError);

        user.Role = customerRole;
        var token = jwtService.GenerateToken(user);

        var responseDto = new AuthResponseDto
        {
            AccessToken = token,
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            UserDto = mapper.Map<UserDto>(user)
        };

        return ResponseModel<AuthResponseDto>.Success(responseDto,"User registered successfully", HttpStatusCode.Created);
    }

    public async Task<ResponseModel<bool>> SendOtpAsync(SendOtpDto dto)
    {
        var code = new Random().Next(100000, 999999);
        var oldOtps = applicationDbContext.OtpCodes.Where(x => x.PhoneNumber == dto.PhoneNumber);
        applicationDbContext.OtpCodes.RemoveRange(oldOtps);

        var otp = new OtpCode
        {
            PhoneNumber = dto.PhoneNumber,
            Code = code,
            ExpirationTime = DateTime.UtcNow.AddMinutes(2),
            IsUsed = false,
        };

        await applicationDbContext.OtpCodes.AddAsync(otp);
        var result = await applicationDbContext.SaveChangesAsync();

        if (result < 1)
            return ResponseModel<bool>.Fail("Error with saving to database", HttpStatusCode.InternalServerError);

        Console.WriteLine($"OTP Code: {code}");

        return ResponseModel<bool>.Success(true, "Code sent", HttpStatusCode.OK);
    }

    public async Task<ResponseModel<bool>> VerifyOtpAsync(VerifyOtpDto dto)
    {
        var otp= await applicationDbContext.OtpCodes.FirstOrDefaultAsync(x=>x.PhoneNumber==dto.PhoneNumber);

        if (otp is null)
            return ResponseModel<bool>.Fail("Code doesn't exists in database", HttpStatusCode.NotFound);

        if (otp.Code != dto.Code)
            return ResponseModel<bool>.Fail("This code is incorrect", HttpStatusCode.Conflict);

        if (otp.ExpirationTime < DateTime.UtcNow)
            return ResponseModel<bool>.Fail("Code has expired", HttpStatusCode.BadRequest);

        if (otp.IsUsed)
            return ResponseModel<bool>.Fail("This code already used", HttpStatusCode.BadRequest);

        otp.IsUsed = true;
        var result = await applicationDbContext.SaveChangesAsync();

        if (result < 1)
            return ResponseModel<bool>.Fail("Error with saving to database", HttpStatusCode.InternalServerError);

        return ResponseModel<bool>.Success(true, "Phone number is confirmed", HttpStatusCode.OK);

    }
}
