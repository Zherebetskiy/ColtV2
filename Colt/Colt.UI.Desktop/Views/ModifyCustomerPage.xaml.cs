using Colt.Domain.Entities;
using Colt.UI.Desktop.ViewModels.Customers;

namespace Colt.UI.Desktop.Views;

[QueryProperty(nameof(Customer), "Customer")]
public partial class ModifyCustomerPage : ContentPage
{
    private ModifyCustomerViewModel ViewModel => BindingContext as ModifyCustomerViewModel;

    public ModifyCustomerPage()
	{
		InitializeComponent();
        BindingContext = new ModifyCustomerViewModel();
    }

    public Customer Customer
    {
        set
        {
            if (BindingContext is ModifyCustomerViewModel viewModel)
            {
                viewModel.Customer = value ?? new Customer();
            }
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (ViewModel != null)
        {
            await ViewModel.LoadProducts();
            await ViewModel.LoadOrders();
            await ViewModel.LoadPayments();
            ViewModel.CalculateDebt();
        }
    }
}