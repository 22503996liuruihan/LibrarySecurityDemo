namespace LibraryBackendMVC.Models;

public class Borrowal
{
    public int Id { get; set; }

    // 外键
    public int MemberId { get; set; }
    public int ItemId { get; set; }

    public DateTime BorrowTime { get; set; } = DateTime.Now;
    public DateTime? ReturnTime { get; set; } // 为空代表还没归还

    // 导航属性
    public Member Member { get; set; } = null!;
    public Item Item { get; set; } = null!;
}