namespace PersonalBudgetTracker.Models;

using PersonalBudgetTracker.Exceptions;

public class Expense : Transaction
{
    public string Category { get; set; }
    
    public Expense() 
    {
        Category = "";
    }

    public Expense(decimal amount, string category, string description, DateTime? date ) : base(amount, description, date)
    {
        if (string.IsNullOrEmpty(category)) 
            throw new EmptyFieldException("Category");
        Category = category.Trim();
    }
    
    public override string ToString()
    {
        return $"{Date:dd/MM/yyyy} | -{Amount:F2} | {Category} | {Description}";
    }
    
}