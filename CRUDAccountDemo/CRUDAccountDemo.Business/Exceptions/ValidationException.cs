namespace CRUDAccountDemo.Business.Exceptions;

public class ValidationException(string field, string message) : Exception(message)
{
    public string Field { get; } = field;
}
