namespace kol1.Models;

public class BookInfo
{
    public int id { get; set; }
    public string title { get; set; }
    public IEnumerable<string> genres { get; set; } = new List<string>();
}