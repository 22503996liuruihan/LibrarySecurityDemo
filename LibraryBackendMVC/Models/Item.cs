namespace LibraryBackendMVC.Models;

public class Item
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;
    public int Stock { get; set; }

    // 导航属性：这本书的借阅记录
    public List<Borrowal> Borrowals { get; set; } = new List<Borrowal>();
}