using Colt.UI.Desktop.ViewModels.Products;

namespace Colt.UI.Desktop.Views;

public partial class ProductsPage : ContentPage
{
    private ProductsViewModel ViewModel => BindingContext as ProductsViewModel;

    public ProductsPage()
    {
        InitializeComponent();
        BindingContext = new ProductsViewModel();
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