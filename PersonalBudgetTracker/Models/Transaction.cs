using System.Text.Json.Serialization;

namespace PersonalBudgetTracker.Models;

using PersonalBudgetTracker.Exceptions;

/**
 * Abstract base class representing a financial transaction.
 * Provides common properties (Amount, Date, Description) and validation logic.
 * Configured with JSON attributes to support polymorphic serialization of Income and Expense subclasses.
 */
[JsonDerivedType(typeof(Income), "Income")]
[JsonDerivedType(typeof(Expense), "Expense")]
public abstract class Transaction
{
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }

    protected Transaction()
    {
        Amount = 0;
        Description = "";
        Date = DateTime.Now;
    }

    protected Transaction(decimal amount, string description, DateTime? date)
    {
        if (amount <= 0) 
            throw new InvalidAmountException(amount);
        Amount = amount;
        Date = date?? DateTime.Now;
        Description = description?? "";
    }
    
    public abstract override string ToString();
}