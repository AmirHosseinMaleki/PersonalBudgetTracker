namespace PersonalBudgetTracker;

public class EmptyFieldException : Exception
{
    public string FieldName { get; }
    
    public EmptyFieldException(string fieldName) 
        : base($"{fieldName} cannot be empty or whitespace.")
    {
        FieldName = fieldName;
    }
    
    public EmptyFieldException(string fieldName, string message) 
        : base(message)
    {
        FieldName = fieldName;
    }
    
    public EmptyFieldException(string fieldName, string message, Exception innerException) 
        : base(message, innerException)
    {
        FieldName = fieldName;
    }
}