using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace PersonalBudgetTracker.Services;

using PersonalBudgetTracker.Exceptions;
using PersonalBudgetTracker.Models;


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
        try
        {
            string json = JsonSerializer.Serialize(account, GetJsonOptions());
            File.WriteAllText(_filePath, json);
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new DataStorageException(_filePath, "Access denied. Check file permissions.", ex);
        }
        catch (DirectoryNotFoundException ex)
        {
            throw new DataStorageException(_filePath, "Directory not found.", ex);
        }
        catch (IOException ex)
        {
            throw new DataStorageException(_filePath, "File input/output error.", ex);
        }
        catch (JsonException ex)
        {
            throw new DataStorageException(_filePath, "Failed to serialize account data to JSON.", ex);
        }
    }

    public Account LoadAccount() 
    {
        if (!File.Exists(_filePath))
            throw new FileNotFoundException($"Account data file not found: {_filePath}");
        
        try
        {
            string json = File.ReadAllText(_filePath);
            Account? account = JsonSerializer.Deserialize<Account>(json, GetJsonOptions());
        
            return account ?? throw new DataStorageException(_filePath, "Deserialized account is null - file may be corrupted.");
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new DataStorageException(_filePath, "Access denied. Check file permissions.", ex);
        }
        catch (IOException ex)
        {
            throw new DataStorageException(_filePath, "File input/output error.", ex);
        }
        catch (JsonException ex)
        {
            throw new DataStorageException(_filePath, "Invalid JSON format - file may be corrupted.", ex);
        }
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