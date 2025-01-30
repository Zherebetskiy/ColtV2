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
        ProductExpander.HandleHeaderTapped += ProductExpander_Tapped;
        OrderExpander.HandleHeaderTapped += OrderExpander_Tapped;
        PaymentExpander.HandleHeaderTapped += PaymentExpander_Tapped;
    }

    public void ProductExpander_Tapped(TappedEventArgs args)
    {
        ProductArrowImage.Rotation = ProductArrowImage.Rotation == 0 ? 180 : 0;
    }

    public void OrderExpander_Tapped(TappedEventArgs args)
    {
        OrderArrowImage.Rotation = OrderArrowImage.Rotation == 0 ? 180 : 0;
    }

    public void PaymentExpander_Tapped(TappedEventArgs args)
    {
        PaymentArrowImage.Rotation = PaymentArrowImage.Rotation == 0 ? 180 : 0;
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
            await ViewModel.LoadOrdersPage(1);
            await ViewModel.LoadPaymentsPage(1);
            await ViewModel.CalculateDebt();
        }
    }
}