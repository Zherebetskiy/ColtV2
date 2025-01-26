using Colt.Application.Interfaces;
using Colt.Domain.Entities;
using Colt.UI.Desktop.Helpers;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Colt.UI.Desktop.ViewModels.Customers
{
    public class ModifyCustomerViewModel : BaseViewModel
    {
        private readonly ICustomerService _customerService;
        private readonly IProductService _productService;

        public ObservableCollection<CustomerProduct> Products { get; }
        public ObservableCollection<CustomerProduct> SelectedProducts { get; }

        public ICommand SaveCustomerCommand { get; }
        public ICommand AddProductCommand { get; }
        public ICommand RemoveProductCommand { get; }

        private CustomerProduct _selectedProduct;
        public CustomerProduct SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged();
            }
        }

        private Customer _customer;
        public Customer Customer
        {
            get => _customer;
            set
            {
                _customer = value;
                OnPropertyChanged();
            }
        }

        public ModifyCustomerViewModel()
        {
            _customerService = ServiceHelper.GetService<ICustomerService>();
            _productService = ServiceHelper.GetService<IProductService>();
            SaveCustomerCommand = new Command(async () => await SaveCustomer());
            AddProductCommand = new Command(AddProduct);
            RemoveProductCommand = new Command<CustomerProduct>(RemoveProduct);
            Customer = new Customer();
            Products = new ObservableCollection<CustomerProduct>();
            SelectedProducts = new ObservableCollection<CustomerProduct>();
        }

        public async Task LoadProducts()
        {
            var products = await _productService.GetAllAsync();

            Products.Clear();
            foreach (var product in products)
            {
                Products.Add(new CustomerProduct
                {
                    Product = product,
                    ProductId = product.Id,
                    CustomerId = Customer.Id
                });
            }

            var customerProducts = await _customerService.GetProductsAsync(Customer.Id);
            
            SelectedProducts.Clear();
            foreach (var customerProduct in customerProducts)
            {
                SelectedProducts.Add(customerProduct);
            }
        }

        private void AddProduct()
        {
            if (SelectedProduct != null && !SelectedProducts.Any(x => x.ProductId == SelectedProduct.ProductId))
            {
                SelectedProducts.Add(SelectedProduct);
            }
        }

        private async Task SaveCustomer()
        {
            if (string.IsNullOrWhiteSpace(Customer.Name) || string.IsNullOrWhiteSpace(Customer.PhoneNumber))
            {
                await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Помилка", "Назва і телефон обов'язкові!", "OK");
                return;
            }

            Customer.Products = SelectedProducts.ToList();

            if (Customer.Id == 0)
            {
                await _customerService.InsertAsync(Customer);
                await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Успішно", "Клієнт додано!", "OK");
            }
            else
            {
                await _customerService.UpdateAsync(Customer);
                await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Успішно", "Клієнт змінено!", "OK");
            }
            await Shell.Current.GoToAsync("..");
        }

        private void RemoveProduct(CustomerProduct product)
        {
            if (SelectedProducts.Contains(product))
            {
                SelectedProducts.Remove(product);
            }
        }
    }
}
