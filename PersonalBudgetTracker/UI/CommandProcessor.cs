using System.Globalization;
using System.Text;

namespace PersonalBudgetTracker;

public class CommandProcessor
{
    private BudgetManager _budgetManager;

    public CommandProcessor(BudgetManager budgetManager)
    {
        _budgetManager = budgetManager;
    }

    public string ProcessCommand(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return "Please enter a command";
        
        string[] parts = input.Trim().Split(' ',  StringSplitOptions.RemoveEmptyEntries);
        string command = parts[0].ToLower();


        try
        {
            if (command == "add") return ProcessAdd(parts);
            else if (command == "list") return ProcessList(parts);
            else if (command == "balance") return ProcessBalance();
            else if (command == "category_summary") return ProcessCategorySpending(parts);
            else if (command == "exit") return ProcessExit();
            else return "Unknown command. Available commands: add, list, balance, category_summary, exit";
        }
        catch (DataStorageException ex)
        {
            return $"Storage Error: {ex.Message}";
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }

    private string ProcessAdd(string[] parts)
    {
        if (parts.Length < 2) 
            return "Usage: add income <amount> <source> OR add expense <amount> <category> <description>";
        
        string subCommand = parts[1].ToLower();
        
        if (subCommand == "income") 
            return ProcessAddIncome(parts);
        else if (subCommand == "expense") 
            return ProcessAddExpense(parts);
        else 
            return "Usage: add income <amount> <source> OR add expense <amount> <category> <description>";
    }

    private string ProcessAddIncome(string[] parts)
    {
        // Expected: add income <amount> <source> [description] [date]
        
        if (parts.Length < 4) 
            return "Usage: add income <amount> <source> [description] [date]";
        
        try
        {
            if (!decimal.TryParse(parts[2], out decimal amount))
                throw new InvalidAmountException(0, $"'{parts[2]}' is not a valid number.");
            
            string source = parts[3];
            if (string.IsNullOrWhiteSpace(source))
                throw new EmptyFieldException("Source");
            
            string description = "";
            DateTime? date = null;
            
            if (parts.Length > 4)
            {
                string lastPart = parts[parts.Length - 1];
                if (DateTime.TryParse(lastPart, out DateTime parsedDate))
                {
                    date = parsedDate;
                    if (parts.Length > 5)
                        description = string.Join(" ", parts, 4, parts.Length - 5);
                }
                else
                {
                    description = string.Join(" ", parts, 4, parts.Length - 4);
                }
            }
            
            _budgetManager.AddIncome(amount, date, description, source);
            string dateStr = date?.ToString("dd/MM/yyyy") ?? DateTime.Now.ToString("dd/MM/yyyy");
            return $"Income of {amount:C} from {source} added for {dateStr}.";
        }
        
        catch (InvalidAmountException ex)
        {
            return $"Error: {ex.Message}";
        }
        catch (EmptyFieldException ex)
        {
            return $"Error: {ex.Message}";
        }
        catch (InvalidDateFormatException ex)
        {
            return $"Error: {ex.Message}";
        }
        catch (DataStorageException ex)
        {
            return $"Storage Error: {ex.Message}";
        }
        catch (Exception ex)
        {
            return $"Unexpected error: {ex.Message}";
        }
    }

    private string ProcessAddExpense(string[] parts)
    {
        // Expected: add expense <amount> <category> [description] [date]
        
        if (parts.Length < 4)
            return "Usage: add expense <amount> <category> <description> [date]";
        
        try
        {
            if (!decimal.TryParse(parts[2], out decimal amount))
                throw new InvalidAmountException(0, $"'{parts[2]}' is not a valid number.");
            
            string category = parts[3];
            if (string.IsNullOrWhiteSpace(category))
                throw new EmptyFieldException("Category");
            
            string description = "";
            DateTime? date = null;
            
            if (parts.Length > 4)
            {
                string lastPart = parts[parts.Length - 1];
                if (DateTime.TryParse(lastPart, out DateTime parsedDate))
                {
                    date = parsedDate;
                    if (parts.Length > 5)
                        description = string.Join(" ", parts, 4, parts.Length - 5);
                }
                else
                {
                    description = string.Join(" ", parts, 4, parts.Length - 4);
                }
            }
            
            _budgetManager.AddExpense(amount, date, description, category);
            string dateStr = date?.ToString("dd/MM/yyyy") ?? DateTime.Now.ToString("dd/MM/yyyy");
            
            string descriptionPart = string.IsNullOrEmpty(description) ? "" : $" ({description})";
            return $"Expense of {amount:C} for {category}{descriptionPart} added for {dateStr}.";
        }
        
        catch (InvalidAmountException ex)
        {
            return $"Error: {ex.Message}";
        }
        catch (EmptyFieldException ex)
        {
            return $"Error: {ex.Message}";
        }
        catch (InvalidDateFormatException ex)
        {
            return $"Error: {ex.Message}";
        }
        catch (DataStorageException ex)
        {
            return $"Storage Error: {ex.Message}";
        }
        catch (Exception ex)
        {
            return $"Unexpected error: {ex.Message}";
        }
    }

    private string ProcessList(string[] parts)
    {
        // Expected: list [type] [dateRange]
        // type: income, expense, all
        // dateRange: current_month, all, or "dd/MM/yyyy dd/MM/yyyy"
        
        
        DateRangeFilter dateFilter = DateRangeFilter.All;
        TransactionTypeFilter typeFilter = TransactionTypeFilter.All;
        DateTime? startDate = null;
        DateTime? endDate = null;
        string typeDescription = "Transactions";
        string periodDescription = "All Time";

        for (int i = 1; i < parts.Length; i++)
        {
            string param =  parts[i].ToLower();
            
            if (param == "income")
            {
                typeFilter = TransactionTypeFilter.Income;
                typeDescription = "Income";
            }
            else if (param == "expense")
            {
                typeFilter = TransactionTypeFilter.Expense;
                typeDescription = "Expenses";
            }
            else if (param == "all" && i == 1)
            {
                typeFilter = TransactionTypeFilter.All;
                typeDescription = "Transactions";
            }
            else if (param == "current_month")
            {
                dateFilter = DateRangeFilter.CurrentMonth;
                periodDescription = "Current Month";
            }
            else if (param == "all" && i > 1)
            {
                dateFilter = DateRangeFilter.All;
                periodDescription = "All Time";
            }
            else
            {
                return "Invalid parameter. Usage: list [income|expense|all] [current_month|all|dd/MM/yyyy dd/MM/yyyy]";
            }
        }

        try
        {
            List<Transaction> transactions = _budgetManager.GetTransactions(typeFilter, dateFilter, startDate, endDate);
            if (transactions.Count == 0)
                return $"No {typeDescription.ToLower()} found for {periodDescription}.";
            
            return FormatTransactionList(transactions, typeDescription, periodDescription);
        }
        catch (Exception e)
        {
            return $"Error retrieving transactions: {e.Message}";
        }
        
    }

    private string FormatTransactionList(List<Transaction> transactions, string typeDescription,
        string periodDescription)
    {
        var result = new StringBuilder();
        result.AppendLine($"--- {typeDescription} ({periodDescription}) ---");
        result.AppendLine("Date       | Amount | Category/Source | Description");
        result.AppendLine("-----------|--------|-----------------|--------------------");
    
        foreach (Transaction transaction in transactions)
        {
            string amount = transaction is Income ? $"+{transaction.Amount:F2}" : $"-{transaction.Amount:F2}";
            string categoryOrSource = transaction is Income income ? income.Source : ((Expense)transaction).Category;
        
            result.AppendLine($"{transaction.Date:dd/MM/yyyy} | {amount,6} | {categoryOrSource,-15} | {transaction.Description}");
        }
    
        result.AppendLine(new string('-', 60));
    
        return result.ToString();
    }

    private string ProcessBalance()
    {
        try
        {
            decimal currentBalance = _budgetManager.GetBalance();
            return $"Current balance: {currentBalance:C}";

        }
        catch (Exception e)
        {
            return $"Error calculating balance: {e.Message}";
        }
        
    }

    private string ProcessCategorySpending(string[] parts)
    {
        // Expected: category_summary [dateRange]
        // dateRange can be: current_month, all, or "dd/MM/yyyy dd/MM/yyyy"
        
        DateRangeFilter dateFilter = DateRangeFilter.All;
        DateTime? startDate = null;
        DateTime? endDate = null;
        string periodDescription = "All Time";

        if (parts.Length > 1)
        {
            string dateRange = parts[1].ToLower();

            if (dateRange == "current_month")
            {
                dateFilter = DateRangeFilter.CurrentMonth;
                periodDescription = "Current Month";
            }
            else if (dateRange == "all")
            {
                dateFilter = DateRangeFilter.All;
                periodDescription = "All Time";
            }
            else if (parts.Length >= 3)
            {
                if (DateTime.TryParseExact(parts[1], "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime start) &&
                    DateTime.TryParseExact(parts[2], "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime end))
                {
                    dateFilter = DateRangeFilter.Custom;
                    startDate = start;
                    endDate = end;
                    periodDescription = $"{start:dd/MM/yyyy} to {end:dd/MM/yyyy}";
                }
                else
                {
                    return "Invalid date format. Use: category_summary [current_month|all|dd/MM/yyyy dd/MM/yyyy]";
                }
            }
            else
            {
                return "Invalid date range. Use: category_summary [current_month|all|dd/MM/yyyy dd/MM/yyyy]";
            }
        }

        try
        {
            Dictionary<string, decimal> categorySpending = _budgetManager.GetCategorySpending(dateFilter, startDate, endDate);
            
            if  (categorySpending.Count == 0)
                return $"No expenses found for {periodDescription}.";

            var result = new StringBuilder();
            result.AppendLine($"--- Category Spending ({periodDescription}) ---");

            foreach (var category in categorySpending)
            {
                result.AppendLine($"{category.Key}: {category.Value:C}");
            }
            
            result.AppendLine(new string('-', 40));
            
            return result.ToString();
        }
        catch (Exception e)
        {
            return $"Error getting category spending: {e.Message}";
        }
    }

    private string ProcessExit()
    {
        return "Goodbye! Your data has been saved.";
    }
}