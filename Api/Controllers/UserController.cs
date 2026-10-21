using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Testimonial.Application.Features.Users;
using Testimonial.Domain.Authentication;
using Testimonial.Infrastructure.Features.Users;
using Testimonial.Infrastructure.Identity;
using Testimonial.Infrastructure.Persistence;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AppDbContext _context;

    public UserController(
        UserService userService,
        UserManager<ApplicationUser> userManager,
        AppDbContext context)
    {
        _userService = userService;
        _userManager = userManager;
        _context = context;
    }

    // -----------------------------
    // CREATE USER
    // -----------------------------
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        var result = await _userService.CreateUserAsync(request);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok(result.Data);
    }

    // -----------------------------
    // DELETE USER
    // -----------------------------
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var result = await _userService.DeleteUserAsync(id);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok("User Deleted");
    }

    // -----------------------------
    // UPDATE USER
    // -----------------------------
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserRequest request)
    {

        Console.WriteLine("=== UPDATE REQUEST ===");
        Console.WriteLine("Email: " + request.Email);
        Console.WriteLine("FirstName: " + request.FirstName);
        Console.WriteLine("LastName: " + request.LastName);
        Console.WriteLine("Password: " + (string.IsNullOrEmpty(request.Password) ? "null" : "SET"));
        Console.WriteLine("Roles: " + string.Join(",", request.Roles ?? new List<string>()));

        var result = await _userService.UpdateUserAsync(id, request);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok(result.Data);
    }


    // -----------------------------
    // GET USER BY ID
    // -----------------------------
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound("Bruger ikke fundet");

        var roles = await _userManager.GetRolesAsync(user);

        return Ok(new
        {
            Id = user.Id,
            Name = user.UserName,
            Email = user.Email,
            Role = roles.FirstOrDefault() ?? "User",
            ProfileImageUrl = user.ProfileImageUrl
        });
    }

    // -----------------------------
    // GET ALL USERS
    // -----------------------------
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userManager.Users
            .Select(u => new
            {
                Id = u.Id,
                Name = u.Name ?? "",
                Email = u.Email ?? "",
                ProfileImageUrl = u.ProfileImageUrl ?? ""
            })
            .ToListAsync();

        return Ok(users);
    }

    // -----------------------------
    // UPLOAD PROFILE IMAGE
    // -----------------------------
    [HttpPost("upload-image/{id}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadImage(string id, [FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Ingen fil uploadet");

        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound("Bruger ikke fundet");

        var folderPath = Path.Combine("wwwroot", "profileImages");
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        var uniqueName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(folderPath, uniqueName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        user.ProfileImageUrl = $"profileImages/{uniqueName}";
        await _userManager.UpdateAsync(user);

        return Ok(new { imageUrl = user.ProfileImageUrl });
    }

    // -----------------------------
    // SEARCH USERS
    // -----------------------------
    [HttpGet("search")]
    public async Task<IActionResult> SearchUsers([FromQuery] string name)
    {
        var users = await _userManager.Users
            .Where(u => u.Name.Contains(name))
            .Select(u => new { u.Id, u.Name })
            .ToListAsync();

        return Ok(users);
    }

    // -----------------------------
    // CREATE ADMIN USER
    // -----------------------------
    [HttpGet("create-admin")]
    public async Task<IActionResult> CreateAdmin([FromServices] RoleManager<IdentityRole> roleManager)
    {
        if (!await roleManager.RoleExistsAsync("Admin"))
            await roleManager.CreateAsync(new IdentityRole("Admin"));

        // 2. Find admin-brugeren
        var user = await _userManager.FindByEmailAsync("admin@test.dk");

        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = "admin@test.dk",
                Email = "admin@test.dk",
                EmailConfirmed = true,
                FirstName = "Administrator",
                LastName = ""
            };
            // 3. Hvis brugeren ikke findes → opret den
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = "admin@test.dk",
                    Email = "admin@test.dk",
                    EmailConfirmed = true,
                    //Name = "Administrator"
                };

                var result = await _userManager.CreateAsync(user, "Password123!");
                if (!result.Succeeded)
                    return BadRequest(result.Errors);
            }
        }

        // 4. Giv brugeren Admin-rolle
        await _userManager.AddToRoleAsync(user, "Admin");

        return Ok("Admin user oprettet");
    }

    // -----------------------------
    // GET TEACHERS (Identity role)
    // -----------------------------
    [HttpGet("teachers")]
    public async Task<IActionResult> GetTeachers()
    {
        var teachers = await _userManager.GetUsersInRoleAsync("Teacher");

        var result = teachers.Select(u => new
        {
            Id = u.Id,
            Name = u.Name ?? "",
            Email = u.Email ?? "",
            ProfileImageUrl = u.ProfileImageUrl ?? ""
        });

        return Ok(result);
    }

    // -----------------------------
    // ASSIGN TEACHER ROLE
    // -----------------------------
    [HttpPost("assign-teacher/{id}")]
    public async Task<IActionResult> AssignTeacherRole(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound("Bruger ikke fundet");

        await _userManager.AddToRoleAsync(user, "Teacher");

        return Ok("Bruger er nu lærer");
    }

    // -----------------------------
    // GET TEACHERS FROM Teacher TABLE
    // -----------------------------
    [HttpGet("teachers-from-db")]
    public async Task<IActionResult> GetTeachersFromDb()
    {
        var teachers = await _context.Teacher
            .Select(t => new
            {
                Id = t.TeacherId,
                FirstName = t.FirstName,
                LastName = t.LastName,
                FullName = (t.FirstName + " " + t.LastName).Trim(),
                Email = t.Email
            })
            .ToListAsync();

        return Ok(teachers);
    }


    //En lærer/user har visse klasser

    [HttpGet("{id}/classes")]
    public async Task<IActionResult> GetUserClasses(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound("Bruger ikke fundet");

        //hent alle roller
        var roles = await _userManager.GetRolesAsync(user);

        //Mapping fra klassetrin -> classId
        var classMap = new Dictionary<int, int>
        {
            {2, 1},
            {4, 2},
            {6, 3},
            {9, 4},
        };

        //find roller de rmacher "Class_X"
        var classIds = roles
            .Where(r => r.StartsWith("Class_"))
            .Select(r =>
            {
                //Klassetrin 2,4,6,9
                if (int.TryParse(r.Replace("Class_", ""), out int grade))
                {
                    //oversæt klassetring -> classId
                    if (classMap.TryGetValue(grade, out int classId))
                        return classId;
                }

                return -1;
            })
            .Where(id => id > 0)
            .ToList();
        return Ok(classIds);
    }




}
