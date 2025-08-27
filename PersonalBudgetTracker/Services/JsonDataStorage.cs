using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace PersonalBudgetTracker.Services;

using PersonalBudgetTracker.Exceptions;
using PersonalBudgetTracker.Models;

/**
 * Implements account data persistence using JSON file storage.
 * Handles serialization/deserialization of Account objects with proper error handling.
 * Automatically locates the project root directory for consistent file placement.
 */
public class JsonDataStorage : IDataStorage
{
    private string _filePath;

    public JsonDataStorage(string fileName = "budget_data.json")
    {
        string projectRoot = FindProjectRoot();
        _filePath = Path.Combine(projectRoot, fileName);
    }
    
    /**
     * Searches up the directory tree to find the project root directory.
     * Looks for a directory containing a .csproj file starting from the executable location.
     * @return The full path to the project root directory or current directory if not found
     */
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

    /**
     * Serializes the account to JSON and writes it to the file.
     * Catches file system exceptions and wraps them in DataStorageException with context.
     * @param account The account object to serialize and save
     * @throws DataStorageException if file operations fail
     */
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

    /**
     * Loads account data from the JSON file and deserializes it.
     * Throws FileNotFoundException if the data file doesn't exist (first-time user).
     * @return The loaded account with all transactions
     * @throws FileNotFoundException if the data file does not exist
     * @throws DataStorageException if file cannot be read or contains invalid JSON
     */
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

    /**
     * Creates JsonSerializerOptions with formatting and naming configuration.
     * Sets up indented output and camelCase property naming.
     * @return Configured JsonSerializerOptions object
     */
    private JsonSerializerOptions GetJsonOptions()
    {
        return new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
    }
}