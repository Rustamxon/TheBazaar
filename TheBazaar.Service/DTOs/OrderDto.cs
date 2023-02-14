using TheBazaar.Domain.Enums;

namespace TheBazaar.Service.DTOs;

public class OrderDto
{
    public List<ProductDto> Items { get; set; }
    public long UserId { get; set; }
    public string Address { get; set; }
    public PaymentType PaymentType { get; set; }
}
