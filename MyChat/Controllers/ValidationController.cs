using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    public bool EditCheckEmail(string Email)
    {
        User usr = _context.Users.FirstOrDefault(u => u.Id == int.Parse(_userManager.GetUserId(User)));
        bool result = true;
        if (_context.Users.Any(u => u.Email.ToLower().Trim() == Email.ToLower().Trim()))
        {
            if (Email.ToLower().Trim() == usr.Email.ToLower().Trim())
                result = true;
            else
                result = false;
        }
        return result;
    }
    [AcceptVerbs("GET", "POST")]
    public bool EditCheckUsername(string UserName)
    {
        User usr = _context.Users.FirstOrDefault(u => u.Id == int.Parse(_userManager.GetUserId(User)));
        bool result = true;
        if (_context.Users.Any(u => u.UserName.ToLower().Trim() == UserName.ToLower().Trim()))
        {
            if (UserName.ToLower().Trim() == usr.UserName.ToLower().Trim())
                result = true;
            else
                result = false;
        }
        return result;
    }
}