using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyChat.Models;

namespace MyChat.Controllers;

public class UserController : Controller
{
    private readonly SignInManager<User> _signInManager;
    private IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _environment;
    private readonly UserManager<User> _userManager;
    private MyChatDb _db;

 
    public UserController(UserManager<User> userManager, SignInManager<User> signInManager,
        IWebHostEnvironment environment, MyChatDb db, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _signInManager = signInManager;
        _userManager = userManager;
        _environment = environment;
        _db = db;
    }

    [Authorize]
    public async Task<IActionResult> Profile(int? userId)
    {
        var referrer = _httpContextAccessor.HttpContext.Request.Headers["Referer"].ToString();
        if (!userId.HasValue)
            return Redirect(referrer);
        User? user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
        return View(user);
    }


}