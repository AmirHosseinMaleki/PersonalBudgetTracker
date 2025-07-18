namespace PersonalBudgetTracker;

public class InvalidAmountException : Exception
{
    public decimal AttemptedAmount { get; }
    
    public InvalidAmountException(decimal amount) 
        : base($"Invalid amount: {amount}. Amount must be greater than zero.")
    {
        AttemptedAmount = amount;
    }
    
    public InvalidAmountException(decimal amount, string message) 
        : base(message)
    {
        AttemptedAmount = amount;
    }
    
    public InvalidAmountException(decimal amount, string message, Exception innerException) 
        : base(message, innerException)
    {
        AttemptedAmount = amount;
    }
}