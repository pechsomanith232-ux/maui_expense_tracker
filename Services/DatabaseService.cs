using SQLite;
using expense_tracker.Models;

namespace expense_tracker.Services;

public class DatabaseService
{
    private SQLiteAsyncConnection? _database;

    private async Task InitAsync()
    {
        if (_database is not null)
            return;

        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "expenses.db3");
        _database = new SQLiteAsyncConnection(dbPath);
        await _database.CreateTableAsync<Expense>();
    }

    public async Task<List<Expense>> GetExpensesAsync()
    {
        await InitAsync();
        return await _database!.Table<Expense>()
                               .OrderByDescending(e => e.Date)
                               .ToListAsync();
    }

    public async Task<int> AddExpenseAsync(Expense expense)
    {
        await InitAsync();
        return await _database!.InsertAsync(expense);
    }

    public async Task<int> DeleteExpenseAsync(Expense expense)
    {
        await InitAsync();
        return await _database!.DeleteAsync(expense);
    }
}