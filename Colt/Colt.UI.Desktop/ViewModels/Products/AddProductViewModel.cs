using Colt.Application.Interfaces;
using Colt.Domain.Entities;
using Colt.UI.Desktop.Helpers;
using System.Windows.Input;

namespace Colt.UI.Desktop.ViewModels.Products
{
    public class AddProductViewModel : BaseViewModel
    {
        private readonly IProductService _productService;
        private Product _product;
        public Product Product
        {
            get => _product;
            set => SetProperty(ref _product, value);
        }
        public ICommand SaveProductCommand { get; }

        public AddProductViewModel()
        {
            _productService = ServiceHelper.GetService<IProductService>();
            SaveProductCommand = new Command(async () => await SaveProduct());
            Product = new Product();
        }

        private async Task SaveProduct()
        {
            if (string.IsNullOrWhiteSpace(Product.Name))
            {
                await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Помилка", "Назва обов'язкова!", "OK");
                return;
            }

            if (Product.Id == 0)
            {
                await _productService.InsertAsync(Product);
                await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Успішно", "Продукт додано!", "OK");
            }
            else
            {
                await _productService.UpdateAsync(Product);
                await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Успішно", "Продукт змінено!", "OK");
            }
            await Shell.Current.GoToAsync("..");
        }
    }
}
