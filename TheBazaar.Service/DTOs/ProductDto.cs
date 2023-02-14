namespace TheBazaar.Service.DTOs;

public class ProductDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<string> SearchTags { get; set; }
    public decimal Price { get; set; }
    public int Count { get; set; }
    public long CategoryName { get; set; }
}
