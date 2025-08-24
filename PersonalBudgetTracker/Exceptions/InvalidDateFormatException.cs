namespace PersonalBudgetTracker.Exceptions;

public class InvalidDateFormatException : Exception
{
    public string AttemptedDate { get; }
    
    public InvalidDateFormatException(string dateString) 
        : base($"Invalid date format: '{dateString}'. Please use dd/MM/yyyy format.")
    {
        AttemptedDate = dateString;
    }
    
    public InvalidDateFormatException(string dateString, string message) 
        : base(message)
    {
        AttemptedDate = dateString;
    }
    
    public InvalidDateFormatException(string dateString, string message, Exception innerException) 
        : base(message, innerException)
    {
        AttemptedDate = dateString;
    }
}
