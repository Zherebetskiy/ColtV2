using Colt.Application.Interfaces;
using Colt.Domain.Entities;
using Colt.UI.Desktop.Helpers;
using System.Windows.Input;

namespace Colt.UI.Desktop.ViewModels.Products
{
    public class AddProductViewModel : BaseViewModel
    {
        private readonly IProductService _productService;

        public string Name { get; set; }
        public string Description { get; set; }

        public ICommand AddProductCommand { get; }

        public AddProductViewModel()
        {
            _productService = ServiceHelper.GetService<IProductService>();
            AddProductCommand = new Command(async () => await AddProduct());
        }

        private async Task AddProduct()
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Description))
            {
                await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Помилка", "Всі поля обов'язкові!", "OK");
                return;
            }

            var product = new Product { Name = Name, Description = Description };
            await _productService.AddProductAsync(product);

            await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Успішно", "Продукт додано!", "OK");
            await Shell.Current.GoToAsync("..");
        }
    }
}
