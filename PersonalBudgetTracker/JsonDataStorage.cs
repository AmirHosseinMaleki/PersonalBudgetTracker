using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace PersonalBudgetTracker;

public class JsonDataStorage : IDataStorage
{
    private string _filePath;

    public JsonDataStorage(string fileName = "budget_data.json")
    {
        string projectRoot = FindProjectRoot();
        _filePath = Path.Combine(projectRoot, fileName);
    }
    
    private string FindProjectRoot()
    {
        string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        DirectoryInfo directory = new DirectoryInfo(currentDirectory);
        
        while (directory != null && !directory.GetFiles("*.csproj").Any())
        {
            directory = directory.Parent;
        }
        
        return directory?.FullName ?? currentDirectory;
    }

    public void SaveAccount(Account account)
    {
        string json = JsonSerializer.Serialize(account, GetJsonOptions());
        
        File.WriteAllText(_filePath, json);
    }

    public Account LoadAccount()
    {
        if (!File.Exists(_filePath))
        {
            throw new FileNotFoundException($"{_filePath} data not found");
        }
        
        string json = File.ReadAllText(_filePath);
        
        Account? account = JsonSerializer.Deserialize<Account>(json, GetJsonOptions());
        
        return account ?? throw new InvalidOperationException("Failed to deserialize account data");
    }

    private JsonSerializerOptions GetJsonOptions()
    {
        return new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
    }
}