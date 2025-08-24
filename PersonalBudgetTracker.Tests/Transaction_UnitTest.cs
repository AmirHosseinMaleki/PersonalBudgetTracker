namespace PersonalBudgetTracker.Tests;

using PersonalBudgetTracker.Models;
using PersonalBudgetTracker.Exceptions;

public class TransactionUnitTest
{
    [Fact]
    public void Income_ValidParameters_CreatesSuccessfully()
    {
        // Arrange & Act
        var income = new Income(1500m, "Salary", "Monthly payment", null);
        
        // Assert
        Assert.Equal(1500m, income.Amount);
        Assert.Equal("Salary", income.Source);
        Assert.Equal("Monthly payment", income.Description);
        Assert.True(income.Date <= DateTime.Now);
    }
    
    [Fact]
    public void Income_NegativeAmount_ThrowsInvalidAmountException()
    {
        // Arrange, Act & Assert
        Assert.Throws<InvalidAmountException>(() => 
            new Income(-100m, "Invalid", "Test", null));
    }
    
    [Fact]
    public void Income_EmptySource_ThrowsEmptyFieldException()
    {
        // Arrange, Act & Assert
        Assert.Throws<EmptyFieldException>(() => 
            new Income(100m, "", "Test", null));
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100.50)]
    public void Income_InvalidAmounts_ThrowsInvalidAmountException(decimal amount)
    {
        // Act & Assert
        Assert.Throws<InvalidAmountException>(() => 
            new Income(amount, "Source", "Test", null));
    }
    
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Income_InvalidSource_ThrowsEmptyFieldException(string source)
    {
        // Act & Assert
        Assert.Throws<EmptyFieldException>(() => 
            new Income(100m, source, "Test", null));
    }
    
    [Fact]
    public void Income_NullSource_ThrowsEmptyFieldException()
    {
        // Act & Assert
        Assert.Throws<EmptyFieldException>(() => 
            new Income(100m, null!, "Test", null));
    }
    
    [Fact]
    public void Expense_ValidParameters_CreatesSuccessfully()
    {
        // Arrange & Act
        var expense = new Expense(50.25m, "Food", "Groceries", null);
        
        // Assert
        Assert.Equal(50.25m, expense.Amount);
        Assert.Equal("Food", expense.Category);
        Assert.Equal("Groceries", expense.Description);
    }
    
    [Fact]
    public void Expense_EmptyCategory_ThrowsEmptyFieldException()
    {
        // Arrange, Act & Assert
        Assert.Throws<EmptyFieldException>(() => 
            new Expense(50m, "   ", "Test", null));
    }
    
    [Fact]
    public void Income_ToString_ContainsExpectedValues()
    {
        // Arrange
        var income = new Income(1500m, "Salary", "Monthly", null);
        
        // Act
        string result = income.ToString();
        
        // Assert
        Assert.Contains("+1500", result);
        Assert.Contains("Salary", result);
        Assert.Contains("Monthly", result);
    }
}

