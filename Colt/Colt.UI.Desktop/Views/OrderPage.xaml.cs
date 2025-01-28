using Colt.Domain.Entities;
using Colt.UI.Desktop.ViewModels.Orders;

namespace Colt.UI.Desktop.Views;

[QueryProperty(nameof(Customer), "Customer")]
[QueryProperty(nameof(OrderId), "OrderId")]
public partial class OrderPage : ContentPage
{
    private OrderViewModel ViewModel => BindingContext as OrderViewModel;

    public OrderPage()
    {
        InitializeComponent();
        BindingContext = new OrderViewModel();
    }

    public Customer Customer
    {
        set
        {
            if (BindingContext is OrderViewModel viewModel)
            {
                viewModel.Customer = value ?? new Customer();
            }
        }
    }

    public int OrderId
    {
        set
        {
            if (BindingContext is OrderViewModel viewModel)
            {
                viewModel.OrderId = value;
            }
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (ViewModel != null)
        {
            await ViewModel.LoadOrder();
        }
    }
}