using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyChat.Models;

namespace MyChat.Controllers;
[Authorize]
[Authorize(Roles = "admin")]
public class AdminController : Controller
{
    private readonly SignInManager<User> _signInManager;
    private IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _environment;
    private readonly UserManager<User> _userManager;
    private IEmailSender _emailSender;
    private MyChatDb _db;

 
    public AdminController(UserManager<User> userManager, SignInManager<User> signInManager,
        IWebHostEnvironment environment, MyChatDb db, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _signInManager = signInManager;
        _userManager = userManager;
        _environment = environment;
        _db = db;
    }

    [Authorize]
    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Users()
    {
        return View(_db.Users.Where(u => u.Id != 1).ToList());
    }

    [Authorize]
    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Block(int? userId)
    {
        if (!userId.HasValue)
            return Json(new {blockIdent = "no such user"});
        User? user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
            return Json(new { blockIdent = "no such user"});
        user.LockoutEnabled = !user.LockoutEnabled;
        if (user.LockoutEnabled == false)
            user.LockoutEnd = DateTimeOffset.MaxValue;
        _db.Users.Update(user);
        await _db.SaveChangesAsync();
        return Json(new {blockIdentVar = user.LockoutEnabled});
    }
    
    [Authorize]
    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> NewUser()
    {
        return View();
    }
    
    [Authorize]
    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> NewUser(RegisterViewModel model, IFormFile uploadedFile)
    {
        if (ModelState.IsValid)
        {
            string newFileName = Path.ChangeExtension($"{model.UserName.Trim()}-ProfileN=1", Path.GetExtension(uploadedFile.FileName));
            string path= $"/userImages/" + newFileName.Trim();
            using (var fileStream = new FileStream(_environment.WebRootPath + path, FileMode.Create))
            {
                await uploadedFile.CopyToAsync(fileStream);
            }
            model.BirthDate = model.BirthDate.ToUniversalTime();
            User user = new User()
            {
                Email = model.Email,
                UserName = model.UserName,
                BirthDate = model.BirthDate,
                PhoneNumber = model.PhoneNumber,
                Avatar = path
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "user");
                return RedirectToAction("Profile", "User", new {userId = user.Id});
            }
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
        }
        ModelState.AddModelError("", "Что-то пошло не так, проверьте все данные!");
        return View(model);
    }
}