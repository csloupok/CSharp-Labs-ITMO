namespace Reports.BLL.Utils;

public class ApplicationLogicException : Exception
{
    public ApplicationLogicException() { }
    public ApplicationLogicException(string message) : base(message) { }
    public ApplicationLogicException(string message, Exception innerException) : base(message, innerException) { }
}