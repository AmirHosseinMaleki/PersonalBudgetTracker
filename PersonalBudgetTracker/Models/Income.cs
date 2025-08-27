namespace PersonalBudgetTracker.Models;

using PersonalBudgetTracker.Exceptions;

/**
 * Represents an income transaction with source identification.
 * Extends Transaction with a Source property and validates that source is not empty.
 */
public class Income : Transaction
{
    public string Source { get; set; }
    
    public Income() 
    {
        Source = "";
    }
    
    public Income(decimal amount, string source, string description, DateTime? date) : base(amount, description, date)
    {
        if (string.IsNullOrWhiteSpace(source)) 
            throw new EmptyFieldException("Source");
        Source = source;
    }
    
    public override string ToString()
    {
        return $"{Date:dd/MM/yyyy} | +{Amount:F2} | {Source} | {Description}";
    }
}