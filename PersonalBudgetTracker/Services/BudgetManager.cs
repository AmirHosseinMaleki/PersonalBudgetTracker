namespace PersonalBudgetTracker.Services;
using PersonalBudgetTracker.Models;
using PersonalBudgetTracker.Enums;

public class BudgetManager
{
    private Account _account;
    private IDataStorage _dataStorage;

    public BudgetManager(Account account,  IDataStorage dataStorage)
    {
        _account = account;
        _dataStorage = dataStorage;
    }

    /**
     * Adds a new income transaction to the account and saves the data to storage.
     * Creates an Income object and adds it to the account's transaction list.
     * @param amount The income amount
     * @param source The source of the income
     * @param description Optional description of the income
     * @param date Transaction date (defaults to current date if null)
     */
    public void AddIncome(decimal amount, string source, string description = "", DateTime? date = null)
    {
        var income = new Income(amount, source, description, date);
        _account.Transactions.Add(income);
        _dataStorage.SaveAccount(_account);
    }

    public void AddExpense(decimal amount, string category, string description = "", DateTime? date = null)
    {
        var expense = new Expense(amount, category, description, date);
        _account.Transactions.Add(expense);
        _dataStorage.SaveAccount(_account);
    }
    
    /**
     * Calculates the current account balance by subtracting total expenses from total income.
     * Iterates through all transactions and sums income and expense amounts separately.
     * @return The current balance as a decimal value
     */
    public decimal GetBalance()
    {
        decimal totalIncome = 0;
        decimal totalExpenses = 0;

        foreach (var transaction in _account.Transactions)
        {
            if (transaction is Income)
            {
                totalIncome += transaction.Amount;
            }
            else if (transaction is Expense)
            {
                totalExpenses += transaction.Amount;
            }
        }
        return totalIncome - totalExpenses;
    }

    /**
     * Retrieves transactions filtered by type and date range.
     * Uses helper methods to apply type and date filtering logic.
     * @param typeFilter Filter by transaction type (All, Income, Expense)
     * @param dateFilter Filter by date range (All, CurrentMonth, Custom)
     * @param startDate Start date for custom date range
     * @param endDate End date for custom date range
     * @return List of transactions matching the specified filters
     */
    public List<Transaction> GetTransactions(TransactionTypeFilter typeFilter, DateRangeFilter dateFilter = DateRangeFilter.All, DateTime? startDate=null, DateTime? endDate=null)
    {
        List<Transaction> results = new List<Transaction>();

        foreach (Transaction transaction in _account.Transactions)
        {
            if (MatchTypeFilter(transaction, typeFilter) &&
                MatchDateFilter(transaction, dateFilter, startDate, endDate))
            {
                results.Add(transaction);
            }
        }
        return results;
    }

    private bool MatchTypeFilter(Transaction transaction, TransactionTypeFilter typeFilter)
    {
        if (typeFilter == TransactionTypeFilter.All) return true;
        else if (typeFilter == TransactionTypeFilter.Income) return transaction is Income;
        else if (typeFilter == TransactionTypeFilter.Expense) return transaction is Expense;
        else return false;
    }

    private bool MatchDateFilter(Transaction transaction, DateRangeFilter dateFilter, DateTime? startDate=null, DateTime? endDate=null)
    {
        if (dateFilter == DateRangeFilter.All) return true;
        else if (dateFilter == DateRangeFilter.CurrentMonth) return IsCurrentMonth(transaction.Date);
        else if (dateFilter == DateRangeFilter.Custom) return IsInCustomRange(transaction.Date, startDate, endDate);
        else return false;
    }

    private bool IsCurrentMonth(DateTime date)
    {
        DateTime now = DateTime.Now;
        return date.Month == now.Month && date.Year == now.Year;
    }

    private bool IsInCustomRange(DateTime date, DateTime? startDate, DateTime? endDate)
    {
        if (startDate == null || endDate == null) return false;
        return date >= startDate && date <= endDate;
    }
    

    public Dictionary<string, decimal> GetCategorySpending(DateRangeFilter dateFilter = DateRangeFilter.CurrentMonth, DateTime? startDate=null, DateTime? endDate=null)
    {
        Dictionary<string, decimal> categoryTotalSpending = new Dictionary<string, decimal>();

        foreach (Transaction transaction in _account.Transactions)
        {
            if (transaction is Expense expense && MatchDateFilter(expense, dateFilter, startDate, endDate))
            {
                if (categoryTotalSpending.ContainsKey(expense.Category)) categoryTotalSpending[expense.Category] += expense.Amount;
                else categoryTotalSpending[expense.Category] = expense.Amount;
            }
        }
        return categoryTotalSpending;
    }
}