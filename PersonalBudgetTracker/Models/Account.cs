namespace PersonalBudgetTracker.Models;

/**
 * Represents a user account containing personal budget information.
 * Holds the username and maintains a collection of all financial transactions.
 */
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