using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Mini_AMS.Models;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Mini_AMS.Pages.Admin
{
    [Authorize(Roles = "Admin,Accountant")]
    public class RoleManagementModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public RoleManagementModel(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public List<UserRoleViewModel> Users { get; set; } = new();
        public List<RoleViewModel> Roles { get; set; } = new();

        public class UserRoleViewModel
        {
            public string Id { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public List<string> Roles { get; set; } = new();
        }

        public class RoleViewModel
        {
            public string Name { get; set; } = string.Empty;
            public int UserCount { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Get all users
            var users = await _userManager.Users.ToListAsync();
            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                Users.Add(new UserRoleViewModel
                {
                    Id = user.Id,
                    Email = user.Email ?? string.Empty,
                    Roles = userRoles.ToList()
                });
            }

            // Get all roles
            var roles = await _roleManager.Roles.ToListAsync();
            foreach (var role in roles)
            {
                var userCount = await GetUserCountInRoleAsync(role.Name);
                Roles.Add(new RoleViewModel
                {
                    Name = role.Name ?? string.Empty,
                    UserCount = userCount
                });
            }

            return Page();
        }

        private async Task<int> GetUserCountInRoleAsync(string? roleName)
        {
            if (string.IsNullOrEmpty(roleName))
                return 0;

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();
                using var command = new SqlCommand("sp_GetUserCountInRole", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RoleName", roleName);

                var result = await command.ExecuteScalarAsync();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }
    }
} 