using Colt.UI.Desktop.ViewModels.Customers;

namespace Colt.UI.Desktop.Views;

public partial class CustomersPage : ContentPage
{
    private CustomersViewModel ViewModel => BindingContext as CustomersViewModel;

    public CustomersPage()
    {
        InitializeComponent();
        BindingContext = new CustomersViewModel();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (ViewModel != null)
        {
            await ViewModel.Initialize();
        }
    }
}