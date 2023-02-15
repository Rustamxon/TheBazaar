namespace TheBazaar.Domain.Enums;

public enum OrderProgressType : byte
{
    Pending = 10,
    Processing = 20,
    Delivered = 30,
    Cancelled = 40
}
