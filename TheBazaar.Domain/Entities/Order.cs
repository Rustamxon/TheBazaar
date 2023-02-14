using TheBazaar.Domain.Commons;
using TheBazaar.Domain.Enums;

namespace TheBazaar.Domain.Entities;

public class Order : Auditable
{
    public List<Product> Items { get; set; }
    public long UserId { get; set; }
    public string Address { get; set; }
    public OrderProgressType Progress { get; set; }
    public PaymentType PaymentType { get; set; }
}
