namespace TheBazaar.Service.Helpers;

public class GenericResponse<TValue>
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public TValue Value { get; set; }
}
