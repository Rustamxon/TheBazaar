using TheBazaar.Domain.Commons;

namespace TheBazaar.Domain.Entities;

public class Category : Auditable
{
    public string Name { get; set; }
    public string Description { get; set; }
}
