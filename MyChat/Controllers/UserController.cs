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
    public async Task<IActionResult> Chat()
    {
        return View();
    }
    
    [Authorize]
    public async Task<IActionResult> Profile(int? userId)
    {
        ViewBag.CurrentUser = await _db.Users.FirstOrDefaultAsync(u => u.Id == int.Parse(_userManager.GetUserId(User)));
        var referrer = _httpContextAccessor.HttpContext.Request.Headers["Referer"].ToString();
        if (!userId.HasValue)
            return Redirect(referrer);
        User? user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
        ViewBag.BlockIdent = user.LockoutEnabled;
        return View(user);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Edit(int? userId)
    {
        var referrer = _httpContextAccessor.HttpContext.Request.Headers["Referer"].ToString();
        if (!userId.HasValue)
            return Redirect(referrer);
        User? model = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
        EditViewModel user = new EditViewModel()
        {
            Id = model.Id,
            Email = model.Email,
            UserName = model.UserName,
            PhoneNumber = model.PhoneNumber,
            BirthDate = model.BirthDate
        };
        return View(user);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Edit(EditViewModel model, IFormFile? uploadedFile)
    {
        User user = _db.Users.FirstOrDefault(u=> u.Id == model.Id);
        if (ModelState.IsValid)
        {
            string? path = null;
            if (uploadedFile!=null)
            {
                var buffer = user.Avatar.Split('=');
                var buffer2 = buffer[buffer.Length - 1].Split('.');
                string newFileName = Path.ChangeExtension($"{model.UserName.Trim()}-ProfileN={int.Parse(buffer2[0])+1}", Path.GetExtension(uploadedFile.FileName));
                path= $"/userImages/" + newFileName.Trim();
                using (var fileStream = new FileStream(_environment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
            }
            model.BirthDate = model.BirthDate.Value.ToUniversalTime();
            user.Id = model.Id != null? model.Id : user.Id;
            user.Email = model.Email != null ? model.Email : user.Email;
            user.UserName = model.UserName != null ? model.UserName : user.UserName ;
            user.PhoneNumber = model.PhoneNumber != null ? model.PhoneNumber : user.PhoneNumber;
            user.BirthDate = model.BirthDate != null ? model.BirthDate : user.BirthDate;
            user.Avatar = path != null ? path : user.Avatar;
            user.PasswordHash = model.Password != null ? _userManager.PasswordHasher.HashPassword(user, model.Password) : user.PasswordHash;
            await _userManager.UpdateAsync(user);
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
            return RedirectToAction("Profile", new {userId = user.Id});
        }
        ModelState.AddModelError("", "Что-то пошло не так, проверьте все данные!");
        return View(model);
    }
}