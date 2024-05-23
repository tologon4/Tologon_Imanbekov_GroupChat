using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyChat.Models;

namespace MyChat.Controllers;

public class ValidationController : Controller
{
    private MyChatDb _context;
    private UserManager<User> _userManager;

    public ValidationController(MyChatDb context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [AcceptVerbs("GET", "POST")]
    public bool AgeCheck(DateTime BirthDate)
    {
        return !((DateTime.Now.Year - BirthDate.Year) < 18);
    }
    [AcceptVerbs("GET", "POST")]
    public bool CheckEmail(string Email)
    {
        return !_context.Users.Any(u => u.Email.ToLower().Trim() == Email.ToLower().Trim());
    }
    [AcceptVerbs("GET", "POST")]
    public bool CheckUsername(string UserName)
    {
        return !_context.Users.Any(u => u.UserName.ToLower().Trim() == UserName.ToLower().Trim());

    }
    
    
    [AcceptVerbs("GET", "POST")]
    public async Task<bool> EditCheckEmail(string? Email, int? Id)
    {
        User adminUsr = await _context.Users.FirstOrDefaultAsync(u => u.Id == int.Parse(_userManager.GetUserId(User)));
        User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == Id);
        bool result = true;
        if (_context.Users.Any(u => u.Email.ToLower().Trim() == Email.ToLower().Trim()))
        {
            if ((Email.ToLower().Trim() == adminUsr.Email.ToLower().Trim() || Email.ToLower().Trim()==user.Email.ToLower().Trim()) && Email.ToLower().Trim() != "admin@admin.com" )
                result = true;
            else
                result = false;
        }
        return result;
    }
    [AcceptVerbs("GET", "POST")]
    public async Task<bool> EditCheckUsername(string? UserName, int? Id)
    {
        User adminUsr = await _context.Users.FirstOrDefaultAsync(u => u.Id == int.Parse(_userManager.GetUserId(User)));
        User? user = await _context.Users.FirstOrDefaultAsync(u => u.Id == Id);
        bool result = true;
        if (_context.Users.Any(u => u.UserName.ToLower().Trim() == UserName.ToLower().Trim()))
        {
            if ((UserName.ToLower().Trim() == adminUsr.UserName.ToLower().Trim() || UserName.ToLower().Trim()==user.UserName.ToLower().Trim()) && UserName.ToLower().Trim() != "admin@admin.com" )
                result = true;
            else
                result = false;
        }
        return result;
    }
}