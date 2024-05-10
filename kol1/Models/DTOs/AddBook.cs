namespace kol1.Models.DTOs;

public class AddBook
{
    public string title { get; set; }
    public IEnumerable<int> genres { get; set; } = new List<int>();
}