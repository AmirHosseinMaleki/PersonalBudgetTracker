namespace PersonalBudgetTracker.Tests;

using Xunit;
using Moq;
using PersonalBudgetTracker.Models;
using PersonalBudgetTracker.Services;
using PersonalBudgetTracker.Enums;


public class BudgetManagerTests : IDisposable
{
    private readonly Account _account;
    private readonly Mock<IDataStorage> _mockStorage;
    private readonly BudgetManager _budgetManager;
    
    public BudgetManagerTests()
    {
        _account = new Account("TestUser");
        _mockStorage = new Mock<IDataStorage>();
        _budgetManager = new BudgetManager(_account, _mockStorage.Object);
    }
    
    [Fact]
    public void AddIncome_ValidParameters_AddsToAccount()
    {
        // Act 
        _budgetManager.AddIncome(1500m, "Salary", "Monthly payment");
        
        // Assert
        Assert.Single(_account.Transactions);
        Assert.IsType<Income>(_account.Transactions[0]);
        Assert.Equal(1500m, _account.Transactions[0].Amount);
        
        _mockStorage.Verify(s => s.SaveAccount(_account), Times.Once);
    }
    
    [Fact]
    public void AddExpense_ValidParameters_AddsToAccount()
    {
        // Act
        _budgetManager.AddExpense(50.25m, "Food", "Groceries");
        
        // Assert
        Assert.Single(_account.Transactions);
        Assert.IsType<Expense>(_account.Transactions[0]);
        Assert.Equal(50.25m, _account.Transactions[0].Amount);
        
        _mockStorage.Verify(s => s.SaveAccount(_account), Times.Once);
    }
    
    [Theory]
    [InlineData(2000, 500, 300, 100, 2100)] 
    [InlineData(1000, 0, 200, 300, 500)]   
    [InlineData(0, 0, 0, 0, 0)]             
    public void GetBalance_VariousTransactions_ReturnsCorrectBalance(
        decimal income1, decimal income2, decimal expense1, decimal expense2, decimal expectedBalance)
    {
        // Arrange
        if (income1 > 0) _budgetManager.AddIncome(income1, "Source1");
        if (income2 > 0) _budgetManager.AddIncome(income2, "Source2");
        if (expense1 > 0) _budgetManager.AddExpense(expense1, "Category1");
        if (expense2 > 0) _budgetManager.AddExpense(expense2, "Category2");
        
        // Act
        decimal balance = _budgetManager.GetBalance();
        
        // Assert
        Assert.Equal(expectedBalance, balance);
    }
    
    [Fact]
    public void GetTransactions_FilterByIncome_ReturnsOnlyIncome()
    {
        // Arrange
        _budgetManager.AddIncome(1500m, "Salary");
        _budgetManager.AddExpense(50m, "Food");
        _budgetManager.AddIncome(200m, "Bonus");
        
        // Act
        var incomeTransactions = _budgetManager.GetTransactions(TransactionTypeFilter.Income);
        
        // Assert
        Assert.Equal(2, incomeTransactions.Count);
        Assert.All(incomeTransactions, t => Assert.IsType<Income>(t));
    }
    
    [Fact]
    public void GetCategorySpending_MultipleExpenses_ReturnsCorrectTotals()
    {
        // Arrange
        _budgetManager.AddExpense(100m, "Food", "Groceries");
        _budgetManager.AddExpense(50m, "Food", "Restaurant");
        _budgetManager.AddExpense(30m, "Transport", "Bus");
        _budgetManager.AddIncome(1000m, "Salary"); // Should be ignored
        
        // Act
        var categorySpending = _budgetManager.GetCategorySpending();
        
        // Assert
        Assert.Equal(2, categorySpending.Count);
        Assert.Equal(150m, categorySpending["Food"]);
        Assert.Equal(30m, categorySpending["Transport"]);
        Assert.DoesNotContain("Salary", categorySpending.Keys);
    }
    
    public void Dispose()
    {
    }
}
