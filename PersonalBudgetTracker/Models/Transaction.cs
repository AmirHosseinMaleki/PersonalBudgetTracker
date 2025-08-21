using System.Text.Json.Serialization;

namespace PersonalBudgetTracker;

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