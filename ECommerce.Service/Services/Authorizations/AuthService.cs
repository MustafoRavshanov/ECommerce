using AutoMapper;
using ECommerce.Domain.DTOs;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Helper;
using ECommerce.Infrastructure.Data;
using ECommerce.Service.Services.JWTs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;
using BC = BCrypt.Net.BCrypt;

namespace ECommerce.Service.Services.Authorizations;

public class AuthService(ApplicationDbContext applicationDbContext, IJwtService jwtService, IMapper mapper, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IConfiguration configuration) : IAuthService
{
    public async Task<ResponseModel<AuthResponseDto>> LoginAsync(LoginDto dto)
    {
        var user = await userManager.Users
            .AsNoTracking()
            .Include(u => u.Role)
            .ThenInclude(a => a.RolePermissions)
            .FirstOrDefaultAsync(x => x.PhoneNumber == dto.PhoneNumber);

        if (user is null)
            return ResponseModel<AuthResponseDto>.Fail("PhoneNumber or Password incorrect", HttpStatusCode.Unauthorized);

        if (!await userManager.CheckPasswordAsync(user, dto.Password!))
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

        var existingUser = await userManager.Users
            .Include(u => u.Role)
            .ThenInclude(a => a.RolePermissions)
            .FirstOrDefaultAsync(x => x.PhoneNumber == dto.PhoneNumber);

        if (existingUser is not null)
            return ResponseModel<AuthResponseDto>.Fail("User with this phone number already exists", HttpStatusCode.Conflict);

        if (dto.Password != dto.ConfirmedPassword)
            return ResponseModel<AuthResponseDto>.Fail("Passwords should be same", HttpStatusCode.BadRequest);

        var customerRole = await roleManager.Roles
            .Include(u => u.RolePermissions)
            .FirstOrDefaultAsync(x => x.Name == "Customer");

        if (customerRole is null)
            return ResponseModel<AuthResponseDto>.Fail("Customer role doesn't exists", HttpStatusCode.InternalServerError);

        var hasher = new PasswordHasher<ApplicationUser>();

        var user = mapper.Map<ApplicationUser>(dto);
        user.PasswordHash = hasher.HashPassword(null, dto.Password!);
        user.RoleId=customerRole.Id;
        user.IsActive = true;

        var result=await userManager.CreateAsync(user);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return ResponseModel<AuthResponseDto>.Fail($"Xatolik: {errors}", HttpStatusCode.InternalServerError);
        }

        var customer = new Customer 
        { 
            Id = user.Id,
            FirstName=user.FirstName,
            LastName=user.LastName,
            PhoneNumber=user.PhoneNumber,
        };

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
        var user = await userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == dto.PhoneNumber);

        if (user != null)
            return ResponseModel<bool>.Fail("User with this phone number already exists", HttpStatusCode.Conflict);

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

    public async Task<ResponseModel<string>> BlockUserAsync(string phoneNumber, string apiKey, DateTime? endDay)
    {
        var key = configuration["Key:ApiKey"];

        if (key != apiKey)
            return ResponseModel<string>.Fail("your api key is incorrect", HttpStatusCode.BadRequest);

        var user = await userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);

        if (user is null)
            return ResponseModel<string>.Fail("User with this phone number not found", HttpStatusCode.NotFound);

        if(!user.LockoutEnabled)
            user.LockoutEnabled = true;
        
        if (user.LockoutEnd > DateTime.Now)
            return ResponseModel<string>.Fail($"User already blocked and have time: {(user.LockoutEnd - DateTimeOffset.UtcNow).ToString()} to opened");

        if (endDay <DateTime.Now)
            user.LockoutEnd = DateTime.MaxValue;
        else
            user.LockoutEnd = endDay;

        var result = await userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return ResponseModel<string>.Fail($"Xatolik: {errors}", HttpStatusCode.InternalServerError);
        }

        return ResponseModel<string>.Success("Success", $"User blocked successfully. User will open {user.LockoutEnd} from block", HttpStatusCode.OK);
    }

    public async Task<ResponseModel<string>> RemoveBlockFromUserAsync(string phoneNumber, string apiKey)
    {
        var key = configuration["Key:ApiKey"];

        if (key != apiKey)
            return ResponseModel<string>.Fail("your api key is incorrect", HttpStatusCode.BadRequest);

        var user = await userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);

        if (user is null)
            return ResponseModel<string>.Fail("User with this phone number not found", HttpStatusCode.NotFound);

        if (!user.LockoutEnabled)
            return ResponseModel<string>.Fail("User never been blocked", HttpStatusCode.BadRequest);

        if (user.LockoutEnd < DateTime.Now)
            return ResponseModel<string>.Fail("User already been unblocked", HttpStatusCode.BadRequest);

        user.LockoutEnd = DateTime.MinValue;

        var result = await userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return ResponseModel<string>.Fail($"Xatolik: {errors}", HttpStatusCode.InternalServerError);
        }

        return ResponseModel<string>.Success("success", "User has been unblocked successfully", HttpStatusCode.OK);

    }
}
