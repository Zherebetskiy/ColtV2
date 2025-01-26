using Colt.Domain.Entities;
using Colt.UI.Desktop.ViewModels.Customers;

namespace Colt.UI.Desktop.Views;

[QueryProperty(nameof(Customer), "Customer")]
public partial class ModifyCustomerPage : ContentPage
{
    public ModifyCustomerPage()
	{
		InitializeComponent();
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
}