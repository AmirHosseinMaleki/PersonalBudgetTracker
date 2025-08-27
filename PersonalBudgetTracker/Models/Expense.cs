namespace PersonalBudgetTracker.Models;

using PersonalBudgetTracker.Exceptions;

/**
 * Represents an expense transaction with categorization.
 * Extends Transaction with a Category property and validates that category is not empty.
 */
public class Expense : Transaction
{
    public string Category { get; set; }
    
    public Expense() 
    {
        Category = "";
    }

    public Expense(decimal amount, string category, string description, DateTime? date ) : base(amount, description, date)
    {
        if (string.IsNullOrWhiteSpace(category)) 
            throw new EmptyFieldException("Category");
        Category = category.Trim();
    }
    
    public override string ToString()
    {
        return $"{Date:dd/MM/yyyy} | -{Amount:F2} | {Category} | {Description}";
    }
    
}