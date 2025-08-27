namespace PersonalBudgetTracker.Services;

using PersonalBudgetTracker.Models;

/**
 * Abstract base class for all financial transactions.
 * Contains common properties and validation for amount.
 */
public interface IDataStorage
{
    void SaveAccount(Account account);
    Account LoadAccount();
}