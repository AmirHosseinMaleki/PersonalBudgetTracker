namespace PersonalBudgetTracker.Models;

public class Account
{
    public string Username { get; set; }
    public List<Transaction> Transactions { get; set; }

    public Account(string username)
    {
        Username = username;
        Transactions = new List<Transaction>();
    }
}