using Colt.Application.Interfaces;
using Colt.Domain.Entities;
using Colt.Domain.Enums;
using Colt.UI.Desktop.Helpers;
using Colt.UI.Desktop.Views;
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
        public ICommand CreateOrderCommand { get; }
        public ICommand EditOrderCommand { get; }
        public ICommand DeleteOrderCommand { get; }

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
            RemoveProductCommand = new Command<CustomerProduct>(async (product) => await RemoveProductAsync(product));
            DeleteOrderCommand = new Command<Order>(async (order) => await RemoveOrderAsync(order));
            CreateOrderCommand = new Command(async () => await NavigateToCreateOrderPage());
            EditOrderCommand = new Command<Order>(async (order) => await NavigateToEditOrderPage(order));
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
        }

        private async Task RemoveProductAsync(CustomerProduct product)
        {
            var isConfirmed = await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Підтвердження", "Ви впевнені, що хочете видалити цeй продукт?", "Так", "Ні");
            if (isConfirmed && SelectedProducts.Contains(product))
            {
                SelectedProducts.Remove(product);
            }
        }

        private async Task RemoveOrderAsync(Order order)
        {
            if(order.Status == OrderStatus.Delivered)
            {
                await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Помилка", "Не можна видалити доствлене замовлення!", "OK");
                return;
            }

            bool isConfirmed = await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Підтвердження", "Ви впевнені, що хочете видалити цe замовлення?", "Так", "Ні");
            if (isConfirmed)
            {
                Customer.Orders.Remove(order);
            }
        }

        private async Task NavigateToCreateOrderPage()
        {
            var navigationParameter = new Dictionary<string, object>
            {
                { "Customer", Customer }
            };

            await Shell.Current.GoToAsync(nameof(OrderPage), navigationParameter);
        }

        private async Task NavigateToEditOrderPage(Order order)
        {
            var navigationParameter = new Dictionary<string, object>
            {
                { "Customer", Customer },
                { "OrderId", order.Id }
            };

            await Shell.Current.GoToAsync(nameof(OrderPage), navigationParameter);
        }
    }
}
