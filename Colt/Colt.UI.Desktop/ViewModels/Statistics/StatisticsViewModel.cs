using Colt.Application.Interfaces;
using Colt.Domain.Entities;
using Colt.UI.Desktop.Helpers;
using Serilog;
using System.Collections.ObjectModel;

namespace Colt.UI.Desktop.ViewModels.Statistics
{
    public class StatisticsViewModel : BaseViewModel
    {
        private readonly ICustomerService _customerService;
        private readonly IPaymentService _paymentService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;

        public ObservableCollection<CustomerDebtChartEntry> CustomerDebtChartEntries { get; }
        public ObservableCollection<Customer> Customers { get; }
        public ObservableCollection<Product> Products { get; }
        public ObservableCollection<IncomeChartEntry> IncomeChartEntries { get; }
        public ObservableCollection<ProductChartEntry> ProductsChartEntries { get; }

        private decimal _totalDebt;
        public decimal TotalDebt
        {
            get => _totalDebt;
            set
            {
                _totalDebt = value;
                OnPropertyChanged();
            }
        }

        private Customer _selectedCustomer;
        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                _selectedCustomer = value;
                OnPropertyChanged();
            }
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged();
            }
        }

        private decimal _totalIncome;
        public decimal TotalIncome
        {
            get => _totalIncome;
            set
            {
                _totalIncome = value;
                OnPropertyChanged();
            }
        }


        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged();
            }
        }

        private Customer _selectedProductCustomer;
        public Customer SelectedProductCustomer
        {
            get => _selectedProductCustomer;
            set
            {
                _selectedProductCustomer = value;
                OnPropertyChanged();
            }
        }

        private DateTime _productStartDate;
        public DateTime ProductStartDate
        {
            get => _productStartDate;
            set
            {
                _productStartDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime _productEndDate;
        public DateTime ProductEndDate
        {
            get => _productEndDate;
            set
            {
                _productEndDate = value;
                OnPropertyChanged();
            }
        }

        private double _totalProductWeight;
        public double TotalProductWeight
        {
            get => _totalProductWeight;
            set
            {
                _totalProductWeight = value;
                OnPropertyChanged();
            }
        }

        public StatisticsViewModel()
        {
            _customerService = ServiceHelper.GetService<ICustomerService>();
            _productService = ServiceHelper.GetService<IProductService>();
            _paymentService = ServiceHelper.GetService<IPaymentService>();
            _orderService = ServiceHelper.GetService<IOrderService>();
            CustomerDebtChartEntries = new ObservableCollection<CustomerDebtChartEntry>();
            Customers = new ObservableCollection<Customer>();
            IncomeChartEntries = new ObservableCollection<IncomeChartEntry>();
            ProductsChartEntries = new ObservableCollection<ProductChartEntry>();
            Products = new ObservableCollection<Product>();
            _startDate = DateTime.Now.AddMonths(-1);
            _endDate = DateTime.Now;
            _productStartDate = DateTime.Now.AddMonths(-1);
            _productEndDate = DateTime.Now;
        }

        public async Task LoadProducts()
        {
            try
            {
                var products = await _productService.GetAllAsync();
                Products.Clear();

                foreach (var product in products)
                {
                    Products.Add(product);
                }

                if (Products.Any())
                {
                    SelectedProduct = Products.First();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to load products");
                //await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Помилка", $"Виникла критична поилка, звяжіться з розробником!!!\n Помилка: {ex.Message}", "OK");
            }
        }

        public async Task LoadCustomers()
        {
            try
            {
                var customers = await _customerService.GetAllAsync();
                Customers.Clear();

                Customers.Add(new Customer
                {
                    Name = "Всі замовники"
                });

                foreach (var customer in customers)
                {
                    Customers.Add(customer);
                }

                if (Customers.Any())
                {
                    SelectedCustomer = Customers.First();
                    SelectedProductCustomer = Customers.First();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to load customers");
                //await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Помилка", $"Виникла критична поилка, звяжіться з розробником!!!\n Помилка: {ex.Message}", "OK");
            }
        }

        public async Task LoadCustomerDebts()
        {
            try
            {
                var customerDebts = new List<CustomerDebtChartEntry>();

                foreach (var customer in Customers)
                {
                    var debt = await _customerService.GetDebtAsync(customer.Id);
                    customerDebts.Add(new CustomerDebtChartEntry
                    {
                        CustomerName = customer.Name,
                        DebtAmount = debt.Debt
                    });
                }

                CustomerDebtChartEntries.Clear();
                foreach (var entry in customerDebts)
                {
                    CustomerDebtChartEntries.Add(entry);
                }

                TotalDebt = customerDebts.Sum(x => x.DebtAmount);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to load customer debts");
                //await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Помилка", $"Виникла критична поилка, звяжіться з розробником!!!\n Помилка: {ex.Message}", "OK");
            }
        }

        public async Task LoadIncomeChart()
        {
            try
            {
                if (StartDate == default)
                {
                    StartDate = DateTime.Now.AddMonths(-1);
                }

                if (EndDate == default)
                {
                    StartDate = DateTime.Now;
                }

                var customerId = SelectedCustomer?.Id == 0 ? null : SelectedCustomer?.Id;
                var payments = await _paymentService.GetStatisticsAsync(customerId, StartDate, EndDate);

                IncomeChartEntries.Clear();
                foreach (var payment in payments)
                {
                    IncomeChartEntries.Add(new IncomeChartEntry
                    {
                        Date = payment.Date.ToString("dd-MMM-yyyy"),
                        Amount = payment.Amount
                    });
                }

                TotalIncome = payments.Sum(x => x.Amount);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to load incomes");
                //await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Помилка", $"Виникла критична поилка, звяжіться з розробником!!!\n Помилка: {ex.Message}", "OK");
            }
        }

        public async Task LoadProductsChart()
        {
            try
            {
                if (ProductStartDate == default)
                {
                    ProductStartDate = DateTime.Now.AddMonths(-1);
                }

                if (ProductEndDate == default)
                {
                    ProductStartDate = DateTime.Now;
                }

                var customerId = SelectedProductCustomer?.Id == 0 ? null : SelectedProductCustomer?.Id;
                var products = await _orderService.GetStatisticsAsync(customerId, SelectedProduct?.Name, ProductStartDate, ProductEndDate);

                ProductsChartEntries.Clear();
                foreach (var product in products)
                {
                    ProductsChartEntries.Add(new ProductChartEntry
                    {
                        Date = product.Order.Delivery.ToString("dd-MMM-yyyy"),
                        Amount = product.ActualWeight ?? 0.0
                    });
                }

                TotalProductWeight = products.Sum(x => x.ActualWeight ?? 0.0);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to load products");
                //await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Помилка", $"Виникла критична поилка, звяжіться з розробником!!!\n Помилка: {ex.Message}", "OK");
            }
        }
    }

    public class CustomerDebtChartEntry
    {
        public string CustomerName { get; set; }
        public decimal DebtAmount { get; set; }
    }

    public class IncomeChartEntry
    {
        public string Date { get; set; }
        public decimal Amount { get; set; }
    }

    public class ProductChartEntry
    {
        public string Date { get; set; }
        public double Amount { get; set; }
    }
}
