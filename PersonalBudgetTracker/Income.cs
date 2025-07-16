namespace PersonalBudgetTracker;

public class Income : Transaction
{
    public string Source { get; set; }
    
    public Income() 
    {
        Source = "";
    }

    public Income(decimal amount, string source, string description, DateTime? date) : base(amount, description, date)
    {
        if (string.IsNullOrEmpty(source)) throw new ArgumentException("Source cannot be null or empty");
        Source = source;
    }
    
    public override string ToString()
    {
        return $"{Date:dd/MM/yyyy} | +{Amount:F2} | {Source} | {Description}";
    }
}