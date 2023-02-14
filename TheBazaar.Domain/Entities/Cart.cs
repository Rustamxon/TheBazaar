using TheBazaar.Domain.Commons;
using TheBazaar.Domain.Enums;

namespace TheBazaar.Domain.Entities;

public class Cart : Auditable
{
    public List<Product> Items { get; set; }
    public long UserId { get; set; }

}
