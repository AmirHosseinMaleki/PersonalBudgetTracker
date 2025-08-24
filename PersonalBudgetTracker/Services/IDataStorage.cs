namespace PersonalBudgetTracker.Services;

using PersonalBudgetTracker.Models;

public interface IDataStorage
{
    void SaveAccount(Account account);
    Account LoadAccount();
}