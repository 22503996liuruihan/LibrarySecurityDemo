namespace LibraryBackendMVC.Models;

public class Member
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string Role { get; set; } = "User"; // Admin 管理员 / User普通会员
    public DateTime CreateTime { get; set; } = DateTime.Now;

    // 导航属性：该会员的借阅记录
    public List<Borrowal> Borrowals { get; set; } = new List<Borrowal>();
}