using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace MyChat.Models;

public class EditViewModel
{
    public int? Id { get; set; }
    [Remote(action: "EditCheckEmail", controller:"Validation", ErrorMessage = "Этот Email уже занят, попробуйте еще раз!")]
    public string? Email { get; set; }
    [DataType(DataType.Password)]
    public string? Password { get; set; }
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Неправильный Confirm Password, попробуйте еще раз!")]
    public string? ConfirmPassword { get; set; }
    [Remote(action: "EditCheckUsername", controller:"Validation", ErrorMessage = "Этот UserName уже занят, попробуйте еще раз!")]
    [RegularExpression(@"^\S+(?:\S+)?$", ErrorMessage = "Заполните UserName без пробела!")]
    public string? UserName { get; set; }
    [RegularExpression(@"^0\d{9}$", ErrorMessage = "Заполните в формате x-цифра: 0 xxx xx xx xx")]
    public string? PhoneNumber { get; set; }
    [Remote(action: "AgeCheck", controller: "Validation", ErrorMessage = "Регистрация только 18+")]
    public DateTime? BirthDate { get; set; }
}