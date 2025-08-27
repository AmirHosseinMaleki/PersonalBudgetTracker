namespace PersonalBudgetTracker.UI;

/**
 * Manages the console-based user interface for the budget tracking application.
 * Handles user input/output, displays welcome messages and help text, and coordinates with CommandProcessor.
 */
public class ConsoleUI 
{
    private CommandProcessor _commandProcessor;
    
    public ConsoleUI(CommandProcessor commandProcessor) 
    {
        _commandProcessor = commandProcessor;
    }
    
    public void Start()
    {
        ShowWelcome();
        ShowHelp();
    
        while (true)
        {
            Console.Write("Budget Tracker> ");
            string? input = Console.ReadLine();
        
            if (string.IsNullOrWhiteSpace(input))
                continue;
        
            string command = input.ToLower().Trim();
        
            if (command == "help")
            {
                ShowHelp();
                continue;
            }
        
            if (command == "clear")
            {
                Console.Clear();
                ShowWelcome();
                continue;
            }
        
            string result = _commandProcessor.ProcessCommand(input);
            Console.WriteLine(result);
        
            if (command == "exit")
            {
                break;
            }
        
            Console.WriteLine(); 
        }
    }
    
    public void ShowWelcome()
    {
        Console.WriteLine("=================================");
        Console.WriteLine("   Personal Budget Tracker");
        Console.WriteLine("=================================");
        Console.WriteLine();
    }
    
    public void ShowHelp()
    {
        Console.WriteLine("Available Commands:");
        Console.WriteLine("  add income <amount> <source> [description] [dd/MM/yyyy]");
        Console.WriteLine("  add expense <amount> <category> [description] [dd/MM/yyyy]");
        Console.WriteLine("  list [income|expense|all] [current_month|all|dd/MM/yyyy dd/MM/yyyy]");
        Console.WriteLine("  balance");
        Console.WriteLine("  category_summary [current_month|all|dd/MM/yyyy dd/MM/yyyy]");
        Console.WriteLine("  help    - Show this help message");
        Console.WriteLine("  clear   - Clear the screen");
        Console.WriteLine("  exit    - Exit the application");
        Console.WriteLine();
        Console.WriteLine("Examples:");
        Console.WriteLine("  add income 1500 Salary");
        Console.WriteLine("  add expense 50.25 Food Groceries");
        Console.WriteLine("  list expense current_month");
        Console.WriteLine("  category_summary current_month");
        Console.WriteLine();
    }
}