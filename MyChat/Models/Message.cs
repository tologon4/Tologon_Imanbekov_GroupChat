namespace MyChat.Models;

public class Message
{
    public int Id { get; set; }
    public string Content { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public DateTime? CreatedTime { get; set; }
    public DateTime? EditedTime { get; set; }
}