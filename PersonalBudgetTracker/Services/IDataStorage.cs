namespace PersonalBudgetTracker;

public interface IDataStorage
{
    void SaveAccount(Account account);
    Account LoadAccount();
}