using Colt.Application.Interfaces;
using Colt.Domain.Common;
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
        private readonly IOrderService _orderService;
        private readonly IPaymentService _paymentService;
        private readonly IInvoiceService _invoiceService;

        public ObservableCollection<Order> Orders { get; }
        public ObservableCollection<Payment> Payments { get; }
        public ObservableCollection<CustomerProduct> Products { get; }
        public ObservableCollection<CustomerProduct> SelectedProducts { get; }

        public ICommand SaveCustomerCommand { get; }
        public ICommand AddProductCommand { get; }
        public ICommand RemoveProductCommand { get; }

        public ICommand CreateOrderCommand { get; }
        public ICommand EditOrderCommand { get; }
        public ICommand DeleteOrderCommand { get; }
        public ICommand DeliverOrderCommand { get; }
        public ICommand PrintOrderCommand { get; }

        public ICommand CreatePaymentCommand { get; }
        public ICommand DeletePaymentCommand { get; }

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

        private OrderDebtModel _debt;
        public OrderDebtModel Debt
        {
            get => _debt;
            set
            {
                _debt = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadOrdersPageCommand { get; }
        public ICommand LoadPaymentsPageCommand { get; }

        private int _currentOrderPage = 1;
        private bool _notLastOrderPage;
        private int _currentPaymentPage = 1;
        private bool _notLastPaymentPage;
        private const int PageSize = 10;

        public int CurrentOrderPage
        {
            get => _currentOrderPage;
            set
            {
                _currentOrderPage = value;
                OnPropertyChanged();
            }
        }

        public bool NotLastOrderPage
        {
            get => _notLastOrderPage;
            set
            {
                _notLastOrderPage = value;
                OnPropertyChanged();
            }
        }

        public int CurrentPaymentPage
        {
            get => _currentPaymentPage;
            set
            {
                _currentPaymentPage = value;
                OnPropertyChanged();
            }
        }

        public bool NotLastPaymentPage
        {
            get => _notLastPaymentPage;
            set
            {
                _notLastPaymentPage = value;
                OnPropertyChanged();
            }
        }

        public ModifyCustomerViewModel()
        {
            _customerService = ServiceHelper.GetService<ICustomerService>();
            _productService = ServiceHelper.GetService<IProductService>();
            _orderService = ServiceHelper.GetService<IOrderService>();
            _paymentService = ServiceHelper.GetService<IPaymentService>();
            _invoiceService = ServiceHelper.GetService<IInvoiceService>();
            SaveCustomerCommand = new Command(async () => await SaveCustomer());
            AddProductCommand = new Command(AddProduct);
            RemoveProductCommand = new Command<CustomerProduct>(async (product) => await RemoveProductAsync(product));
            DeleteOrderCommand = new Command<Order>(async (order) => await RemoveOrderAsync(order));
            CreateOrderCommand = new Command(async () => await NavigateToCreateOrderPage());
            EditOrderCommand = new Command<Order>(async (order) => await NavigateToEditOrderPage(order));
            DeliverOrderCommand = new Command<Order>(async (order) => await DeliverOrder(order));
            PrintOrderCommand = new Command<Order>(async (order) => await PrintOrder(order));
            DeletePaymentCommand = new Command<Payment>(async (payment) => await DeletePaymentAsync(payment));
            CreatePaymentCommand = new Command(CreatePayment);
            LoadOrdersPageCommand = new Command<int>(async (page) => await LoadOrdersPage(page));
            LoadPaymentsPageCommand = new Command<int>(async (page) => await LoadPaymentsPage(page));
            Customer = new Customer();
            Debt = new OrderDebtModel();
            Orders = new ObservableCollection<Order>();
            Products = new ObservableCollection<CustomerProduct>();
            SelectedProducts = new ObservableCollection<CustomerProduct>();
            Payments = new ObservableCollection<Payment>();
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

            if (Customer.Id == default)
            {
                return;
            }

            var customerProducts = await _customerService.GetProductsAsync(Customer.Id);
            
            SelectedProducts.Clear();
            foreach (var customerProduct in customerProducts)
            {
                SelectedProducts.Add(customerProduct);
            }
        }

        public async Task DeliverOrder(Order order)
        {
            if (Customer.Id == default)
            {
                return;
            }

            if (order.Status == OrderStatus.Delivered)
            {
                await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Успішно", "Замовлення вже доставлено!", "OK");
                return;
            }

            if (order.Status == OrderStatus.Created)
            {
                var isConfirmed = await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Підтвердження", "Ще не виконувалась операція зважування. Ви впевнені, що хочете позначити замовлення як доставлене?", "Так", "Ні");
                if (isConfirmed)
                {
                    await _orderService.DeliverAsync(order);
                }
            }
            else
            {
                await _orderService.DeliverAsync(order);
            }

            await LoadOrdersPage(CurrentOrderPage);
            OnPropertyChanged(nameof(Orders));

            await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Успішно", "Замовлення доставлено!", "OK");
        }

        public async Task PrintOrder(Order order)
        {
            if (Customer.Id == default)
            {
                return;
            }

            try
            {
                var outputPath = await _invoiceService.GenerateInvoiceAsync(Customer, order, Debt);

                await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Успішно", $"Накладна збережена в папці: {outputPath}", "OK");
            }
            catch (Exception ex)
            {
                await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Помилка", $"Помилка під час створення накладної: {ex.Message}", "OK");
            }
        }

        public async Task CalculateDebt()
        {
            if (Customer.Id == default)
            {
                return;
            }

            Debt = await _customerService.GetDebtAsync(Customer.Id);
        }

        public async Task LoadOrdersPage(int page)
        {
            if (Customer.Id == default)
            {
                return;
            }

            var paginationResult = await _orderService.GetPaginatedAsync(Customer.Id, (page - 1) * PageSize, PageSize);

            Orders.Clear();
            foreach (var order in paginationResult.Collection)
            {
                Orders.Add(order);
            }

            CurrentOrderPage = page;
            NotLastOrderPage = paginationResult.TotalCount != 0 && (int)Math.Ceiling((double)paginationResult.TotalCount / PageSize) != page;
        }

        public async Task LoadPaymentsPage(int page)
        {
            if (Customer.Id == default)
            {
                return;
            }

            var paginationResult = await _paymentService.GetPaginatedAsync(Customer.Id, (page - 1) * PageSize, PageSize);

            Payments.Clear();
            foreach (var payment in paginationResult.Collection)
            {
                Payments.Add(payment);
            }

            CurrentPaymentPage = page;
            NotLastPaymentPage = paginationResult.TotalCount != 0 && (int)Math.Ceiling((double)paginationResult.TotalCount / PageSize) != page;
        }

        private void AddProduct()
        {
            if (SelectedProduct != null && !SelectedProducts.Any(x => x.ProductId == SelectedProduct.ProductId))
            {
                SelectedProducts.Add(SelectedProduct);
            }
        }

        private void CreatePayment()
        {
            var newPayment = new Payment
            {
                Date = DateTime.Now,
                Amount = 0,
                CustomerId = Customer.Id
            };

            Payments.Insert(0, newPayment);
        }

        private async Task SaveCustomer()
        {
            if (string.IsNullOrWhiteSpace(Customer.Name) || string.IsNullOrWhiteSpace(Customer.PhoneNumber))
            {
                await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Помилка", "Назва і телефон обов'язкові!", "OK");
                return;
            }

            Customer.Products = SelectedProducts.ToList();
            Customer.Orders = Orders.ToList();
            Customer.Payments = Payments.ToList();

            if (Customer.Id == 0)
            {
                await _customerService.InsertAsync(Customer);
                await CalculateDebt();
                await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Успішно", "Клієнт додано!", "OK");
            }
            else
            {
                await _customerService.UpdateAsync(Customer);
                await CalculateDebt();
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

        private async Task DeletePaymentAsync(Payment payment)
        {
            var isConfirmed = await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Підтвердження", "Ви впевнені, що хочете видалити цю оплату?", "Так", "Ні");
            if (isConfirmed && Payments.Contains(payment))
            {
                Payments.Remove(payment);
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
                Orders.Remove(order);

                await _orderService.DeleteAsync(order.Id);
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
