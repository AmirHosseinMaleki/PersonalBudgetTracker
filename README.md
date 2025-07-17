# Personal Budget Tracker

A C# console application for managing personal finances, allowing users to track income and expenses, categorize spending, and view financial summaries.

## Features

- ✅ **Add Income & Expenses** - Record financial transactions with descriptions and dates
- ✅ **Transaction Categories** - Organize expenses by category (Food, Transport, Utilities, etc.)
- ✅ **Balance Calculation** - View current financial balance (total income - total expenses)
- ✅ **Transaction Filtering** - List transactions by type and date range
- ✅ **Category Summaries** - View spending breakdown by category
- ✅ **Data Persistence** - All data saved to JSON file for persistence between sessions
- ✅ **Date Support** - Add transactions with specific dates or use current date
- ✅ **Flexible Commands** - User-friendly command-line interface

## Requirements

- .NET 6.0 or higher
- Windows, macOS, or Linux

## Installation & Setup

1. **Clone the repository:**
   ```bash
   git clone https://github.com/AmirHosseinMaleki/PersonalBudgetTracker.git
   cd PersonalBudgetTracker
   ```

2. **Build the application:**
   ```bash
   dotnet build
   ```

3. **Run the application:**
   ```bash
   dotnet run
   ```

## Usage

### Available Commands

| Command | Description | Example |
|---------|-------------|---------|
| `add income <amount> <source> [description] [date]` | Add income transaction | `add income 1500 Salary Monthly payment` |
| `add expense <amount> <category> [description] [date]` | Add expense transaction | `add expense 50.25 Food Groceries at Tesco` |
| `list [type] [dateRange]` | List transactions with optional filters | `list expense current_month` |
| `balance` | Show current balance | `balance` |
| `category_summary [dateRange]` | Show spending by category | `category_summary current_month` |
| `help` | Show help message | `help` |
| `clear` | Clear screen | `clear` |
| `exit` | Exit application | `exit` |

### Transaction Types
- **Income:** Money received (salary, freelance, gifts, etc.)
- **Expense:** Money spent (food, transport, utilities, etc.)

### Date Formats
- **Current date:** Omit date parameter to use current date/time
- **Specific date:** Use `dd/MM/yyyy` format (e.g., `15/07/2025`)
- **Date ranges:** Use `dd/MM/yyyy dd/MM/yyyy` format (e.g., `01/07/2025 31/07/2025`)

### Filtering Options
- **By Type:** `income`, `expense`, `all`
- **By Date:** `current_month`, `all`, or custom date range

## Usage Examples

```bash
# Add income
Budget Tracker> add income 2500 Salary
Budget Tracker> add income 500 Freelance Web development project 10/07/2025

# Add expenses
Budget Tracker> add expense 45.50 Food
Budget Tracker> add expense 120 Transport Monthly bus pass 01/07/2025

# View transactions
Budget Tracker> list                    # All transactions
Budget Tracker> list expense            # Only expenses
Budget Tracker> list income current_month   # Income for current month
Budget Tracker> list all 01/07/2025 31/07/2025  # All transactions in date range

# Check balance
Budget Tracker> balance

# View category spending
Budget Tracker> category_summary current_month
```

## Project Structure

```
PersonalBudgetTracker/
├── Program.cs              # Main entry point
├── Models/
│   ├── Transaction.cs      # Abstract base class for transactions
│   ├── Income.cs          # Income transaction implementation
│   ├── Expense.cs         # Expense transaction implementation
│   └── Account.cs         # User account data model
├── Services/
│   ├── BudgetManager.cs   # Core business logic
│   ├── IDataStorage.cs    # Data persistence interface
│   └── JsonDataStorage.cs # JSON file persistence implementation
├── UI/
│   ├── ConsoleUI.cs       # User interface handling
│   └── CommandProcessor.cs # Command parsing and execution
├── Enums/
│   ├── TransactionTypeFilter.cs  # Transaction type filtering
│   └── DateRangeFilter.cs        # Date range filtering
└── budget_data.json       # Data file (created automatically)
```

## Technical Details

### Architecture
- **Clean Architecture:** Separation of concerns with distinct layers
- **Interface-driven design:** Loose coupling with dependency injection
- **Command Pattern:** Extensible command processing system
- **Repository Pattern:** Abstract data storage for future extensibility

### Data Storage
- **Format:** JSON with type discriminators for inheritance
- **Location:** Same directory as source files
- **Persistence:** Automatic saving after each transaction

### Design Patterns Used
- **Repository Pattern:** Data access abstraction
- **Command Pattern:** User command processing
- **Factory Pattern:** Transaction creation
- **Strategy Pattern:** Different date filtering strategies

## Sample Output

```
=================================
   Personal Budget Tracker
=================================

Available Commands:
  add income <amount> <source> [description] [dd/MM/yyyy]
  add expense <amount> <category> [description] [dd/MM/yyyy]
  list [income|expense|all] [current_month|all|dd/MM/yyyy dd/MM/yyyy]
  balance
  category_summary [current_month|all|dd/MM/yyyy dd/MM/yyyy]
  help    - Show this help message
  clear   - Clear the screen
  exit    - Exit the application

Budget Tracker> add income 1500 Salary
Income of $1,500.00 from Salary added for 12/07/2025.

Budget Tracker> balance
Current Balance: $1,500.00

Budget Tracker> category_summary current_month
--- Category Spending (Current Month) ---
Food: $200.50
Transport: $150.00
Utilities: $80.00
----------------------------------------
```

## Development

### Building
```bash
dotnet build
```

### Running
```bash
dotnet run
```