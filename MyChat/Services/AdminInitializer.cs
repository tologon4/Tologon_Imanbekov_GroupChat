using Microsoft.AspNetCore.Identity;
using MyChat.Models;

namespace MyChat.Services;

public class AdminInitializer
{
    public static async Task SeedAdminUser(RoleManager<IdentityRole<int>> _roleManager, UserManager<User> _userManager)
    {
        string adminEmail = "admin@admin.com";
        string adminPassword = "Qwe123@";
        var roles = new[] { "admin", "user"};
        foreach (var role in roles)
            if (await _roleManager.FindByNameAsync(role) is null)
                await _roleManager.CreateAsync(new IdentityRole<int>(role));
        if (await _userManager.FindByEmailAsync(adminEmail) is null)
        {
            User admin = new User() { Email = adminEmail, UserName = adminEmail };
            var result = await _userManager.CreateAsync(admin, adminPassword);
            if (result.Succeeded)
                await _userManager.AddToRoleAsync(admin, "admin");
        }

    }
}