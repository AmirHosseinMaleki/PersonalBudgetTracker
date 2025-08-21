namespace PersonalBudgetTracker;

public class DataStorageException : Exception
{
    public string FilePath { get; }
    
    public DataStorageException(string filePath, string message) 
        : base($"Data storage error for '{filePath}': {message}")
    {
        FilePath = filePath;
    }
    
    public DataStorageException(string filePath, string message, Exception innerException) 
        : base($"Data storage error for '{filePath}': {message}", innerException)
    {
        FilePath = filePath;
    }
}