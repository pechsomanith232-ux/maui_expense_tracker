using expense_tracker.ViewModels;

namespace expense_tracker;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _viewModel;

    public MainPage(MainViewModel vm)
    {
        InitializeComponent();
        BindingContext = _viewModel = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Load stored items every time the page appears
        await _viewModel.LoadExpensesCommand.ExecuteAsync(null);
    }
}