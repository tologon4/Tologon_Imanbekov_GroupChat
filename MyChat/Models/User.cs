using Microsoft.AspNetCore.Identity;

namespace MyChat.Models;

public class User : IdentityUser<int>
{
    public DateTime? BirthDate { get; set; }
    public string? Avatar { get; set; }
}