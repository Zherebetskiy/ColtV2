using Colt.Application.Interfaces;
using Colt.Domain.Entities;
using Colt.UI.Desktop.Helpers;
using Colt.UI.Desktop.Views;
using Serilog;
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
        public ICommand EditProductCommand { get; }
        public ICommand DeleteProductCommand { get; }

        public ProductsViewModel()
        {
            _productService = ServiceHelper.GetService<IProductService>();
            LoadProductsCommand = new Command(async () => await LoadProducts());
            NavigateToAddProductCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(AddProductPage)));
            EditProductCommand = new Command<Product>(async (product) => await NavigateToEditProduct(product));
            DeleteProductCommand = new Command<Product>(async (product) => await DeleteProduct(product));

        }

        public async Task Initialize()
        {
            await LoadProducts();
        }

        private async Task LoadProducts()
        {
            try
            {

                var products = (await _productService.GetAllAsync())
                    .OrderByDescending(x => x.Id)
                    .ToList();
                Products.Clear();
                foreach (var product in products)
                {
                    Products.Add(product);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to load products");
                //await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Помилка", $"Виникла критична поилка, звяжіться з розробником!!!\n Помилка: {ex.Message}", "OK");
            }
        }

        private async Task NavigateToEditProduct(Product product)
        {
            var navigationParameter = new Dictionary<string, object>
            {
                { "Product", product }
            };
            await Shell.Current.GoToAsync($"{nameof(AddProductPage)}", navigationParameter);
        }

        private async Task DeleteProduct(Product product)
        {
            try
            {
                bool isConfirmed = await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Підтвердження", "Ви впевнені, що хочете видалити цей продукт?", "Так", "Ні");
                if (isConfirmed)
                {
                    await _productService.DeleteAsync(product.Id);
                    Products.Remove(product);
                    await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Успішно", "Продукт видалено!", "OK");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to delete product");
                await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Помилка", $"Виникла критична поилка, звяжіться з розробником!!!\n Помилка: {ex.Message}", "OK");
            }
        }
    }
}
