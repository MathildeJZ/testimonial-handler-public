using Testimonial.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Testimonial.Application.Common;
using Testimonial.Application.Features.Users;
using Testimonial.Domain.Authentication;


namespace Testimonial.Infrastructure.Features.Users;

public class UserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    // ----------------------------------------------------
    // CREATE USER
    // ----------------------------------------------------
    public async Task<Result<CreateUserResponseDTO>> CreateUserAsync(CreateUserRequest request)
    {
        // TJEK OM EMAIL ALLEREDE FINDES
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
            return Result<CreateUserResponseDTO>.Failure("User with this email already exists");

        // OPRET NY BRUGER
        var newUser = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        var result = await _userManager.CreateAsync(newUser, request.Password);
        if (!result.Succeeded)
            return Result<CreateUserResponseDTO>.Failure(
                result.Errors.Select(e => e.Description).ToArray()
            );

        // TILFØJ ROLLER
        if (request.Roles != null && request.Roles.Any())
        {
            var roleResult = await _userManager.AddToRolesAsync(newUser, request.Roles);
            if (!roleResult.Succeeded)
                return Result<CreateUserResponseDTO>.Failure(
                    roleResult.Errors.Select(e => e.Description).ToArray()
                );
        }

        // RETURNÉR DTO
        return Result<CreateUserResponseDTO>.Success(
            new CreateUserResponseDTO
            {
                Id = newUser.Id,
                Name = newUser.FullName,
                Email = newUser.Email!,
                Role = request.Roles?.FirstOrDefault()
            }
        );
    }


    // ----------------------------------------------------
    // DELETE USER
    // ----------------------------------------------------
    public async Task<Result<string>> DeleteUserAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return Result<string>.Failure("User not found");

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
            return Result<string>.Failure(result.Errors.Select(e => e.Description).ToArray());

        return Result<string>.Success("User deleted successfully");
    }

    // ----------------------------------------------------
    // UPDATE USER
    // ----------------------------------------------------
    public async Task<Result<UpdateUserResponseDTO>> UpdateUserAsync(string id, UpdateUserRequest request)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return Result<UpdateUserResponseDTO>.Failure("User not found");

        // --- OPDATER NAVN ---
        if (!string.IsNullOrEmpty(request.FirstName))
            user.FirstName = request.FirstName;

        if (!string.IsNullOrEmpty(request.LastName))
            user.LastName = request.LastName;

        
      // --- OPDATER EMAIL ---
            if (!string.IsNullOrEmpty(request.Email) && request.Email != user.Email)
            {
                // TJEK OM EMAIL ALLEREDE FINDES HOS EN ANDEN BRUGER
                var existing = await _userManager.FindByEmailAsync(request.Email);
                if (existing != null && existing.Id != user.Id)
                    return Result<UpdateUserResponseDTO>.Failure("Email is already taken");

                user.Email = request.Email;
                user.NormalizedEmail = request.Email.ToUpper();
                user.UserName = request.Email;
                user.NormalizedUserName = request.Email.ToUpper();
            }


        // --- OPDATER PASSWORD ---
        if (!string.IsNullOrEmpty(request.Password))
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var passwordResult = await _userManager.ResetPasswordAsync(user, token, request.Password);

            if (!passwordResult.Succeeded)
                return Result<UpdateUserResponseDTO>.Failure(
                passwordResult.Errors.Select(e => e.Description).ToArray()
    );

        }

        // --- OPDATER ROLLER ---
        if (request.Roles != null)
        {
            var currentRoles = await _userManager.GetRolesAsync(user);

            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
                return Result<UpdateUserResponseDTO>.Failure(
                removeResult.Errors.Select(e => e.Description).ToArray()
    );


            if (request.Roles.Any())
            {
                var addResult = await _userManager.AddToRolesAsync(user, request.Roles);
                if (!addResult.Succeeded)
                    return Result<UpdateUserResponseDTO>.Failure(
                    addResult.Errors.Select(e => e.Description).ToArray()
    );

            }
        }

        // --- GEM ÆNDRINGER ---
        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
            return Result<UpdateUserResponseDTO>.Failure(
            updateResult.Errors.Select(e => e.Description).ToArray()
    );


        // --- RETURNÉR DTO ---
        return Result<UpdateUserResponseDTO>.Success(
            new UpdateUserResponseDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Roles = (await _userManager.GetRolesAsync(user)).ToList()
            }
        );
    }
}



