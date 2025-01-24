using Colt.Application.Interfaces;
using Colt.Domain.Entities;
using Colt.UI.Desktop.Helpers;
using Colt.UI.Desktop.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Colt.UI.Desktop.ViewModels.Products
{
    public class ProductsViewModel : BaseViewModel
    {
        private readonly IProductService _productService;
        public ObservableCollection<Product> Products { get; set; } = new();
        public ICommand LoadProductsCommand { get; }
        public ICommand NavigateToAddProductCommand { get; }

        public ProductsViewModel()
        {
            _productService = ServiceHelper.GetService<IProductService>();
            LoadProductsCommand = new Command(async () => await LoadProducts());
            NavigateToAddProductCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(AddProductPage)));
        }

        public async Task Initialize()
        {
            await LoadProducts();
        }

        private async Task LoadProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            Products.Clear();
            foreach (var product in products)
            {
                Products.Add(product);
            }
        }
    }
}
