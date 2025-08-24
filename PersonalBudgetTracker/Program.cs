using System;
using PersonalBudgetTracker.Models;
using PersonalBudgetTracker.Services;
using PersonalBudgetTracker.UI;

namespace PersonalBudgetTracker
{
    public class Program 
    {
        static void Main(string[] args)
        {
            try 
            {
                IDataStorage dataStorage = new JsonDataStorage();
                
                Account account;
                try 
                {
                    account = dataStorage.LoadAccount();
                    Console.WriteLine($"Welcome back, {account.Username}!");
                }
                catch (FileNotFoundException) 
                {
                    Console.Write("Enter your username: ");
                    string? username = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(username))
                        username = "User";
                        
                    account = new Account(username);
                    dataStorage.SaveAccount(account);
                    Console.WriteLine($"New account created for {username}!");
                }
                
                Console.WriteLine();
                
                BudgetManager budgetManager = new BudgetManager(account, dataStorage);
                CommandProcessor commandProcessor = new CommandProcessor(budgetManager);
                ConsoleUI consoleUI = new ConsoleUI(commandProcessor);
                
                consoleUI.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Application error: {ex.Message}");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}