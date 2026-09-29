using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using expense_tracker.Models;
using expense_tracker.Services;

namespace expense_tracker.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly DatabaseService _databaseService;

    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private string _amount = string.Empty;

    [ObservableProperty]
    private string _selectedCategory = "General";

    [ObservableProperty]
    private decimal _totalExpenses;

    public ObservableCollection<string> Categories { get; } = new()
    {
        "Food", "Transport", "Bills", "Entertainment", "General"
    };

    public ObservableCollection<Expense> Expenses { get; } = new();

    public MainViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    [RelayCommand]
    public async Task LoadExpensesAsync()
    {
        var items = await _databaseService.GetExpensesAsync();
        Expenses.Clear();
        foreach (var item in items)
        {
            Expenses.Add(item);
        }
        UpdateTotal();
    }

    [RelayCommand]
    public async Task AddExpenseAsync()
    {
        if (string.IsNullOrWhiteSpace(Title) || !decimal.TryParse(Amount, out var parsedAmount))
            return;

        var expense = new Expense
        {
            Title = Title.Trim(),
            Amount = parsedAmount,
            Category = SelectedCategory,
            Date = DateTime.UtcNow
        };

        await _databaseService.AddExpenseAsync(expense);
        Expenses.Insert(0, expense);
        UpdateTotal();

        // Reset inputs
        Title = string.Empty;
        Amount = string.Empty;
        SelectedCategory = "General";
    }

    [RelayCommand]
    public async Task DeleteExpenseAsync(Expense? expense)
    {
        if (expense is null) return;

        await _databaseService.DeleteExpenseAsync(expense);
        Expenses.Remove(expense);
        UpdateTotal();
    }

    private void UpdateTotal()
    {
        TotalExpenses = Expenses.Sum(e => e.Amount);
    }
}