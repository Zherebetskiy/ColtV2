using Colt.Application.Interfaces;
using Colt.Domain.Entities;
using Colt.Domain.Enums;
using Colt.UI.Desktop.Helpers;
using Serilog;
using System.Collections.ObjectModel;
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

        public ObservableCollection<MeasurementType> MeasurementTypes { get; }
        public MeasurementType SelectedMeasurementType
        {
            get => Product.MeasurementType;
            set
            {
                Product.MeasurementType = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveProductCommand { get; }

        public AddProductViewModel()
        {
            _productService = ServiceHelper.GetService<IProductService>();
            SaveProductCommand = new Command(async () => await SaveProduct());
            MeasurementTypes = new ObservableCollection<MeasurementType>(Enum.GetValues(typeof(MeasurementType)).Cast<MeasurementType>());
            Product = new Product();
        }

        private async Task SaveProduct()
        {
            try
            {

                if (string.IsNullOrWhiteSpace(Product.Name))
                {
                    await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Помилка", "Назва обов'язкова!", "OK");
                    return;
                }

                Product.MeasurementType = SelectedMeasurementType;

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
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to load products");
                await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Помилка", $"Виникла критична поилка, звяжіться з розробником!!!\n Помилка: {ex.Message}", "OK");
            }
        }
    }
}
