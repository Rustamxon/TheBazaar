namespace TheBazaar.Service.Helpers;

public class GenericResponse<TValue>
{
    public int StatudCode { get; set; }
    public string Message { get; set; }
    public TValue Value { get; set; }
}
