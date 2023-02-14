using TheBazaar.Domain.Entities;
using TheBazaar.Domain.Enums;

namespace TheBazaar.Service.DTOs;

public class OrderDto
{
    public List<Product> Items { get; set; }
    public long UserId { get; set; }
    public string Address { get; set; }
    public PaymentType PaymentType { get; set; }
    public OrderProgressType Progress { get; set; }
}
