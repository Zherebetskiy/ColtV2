using Colt.Application.Interfaces;
using Colt.Application.Services;
using Colt.Domain.Entities;
using Colt.Domain.Enums;
using Colt.UI.Desktop.Helpers;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Colt.UI.Desktop.ViewModels.Orders
{
    public class OrderViewModel : BaseViewModel
    {
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;

        private Order _order;
        public Order Order
        {
            get => _order;
            set => SetProperty(ref _order, value);
        }

        private Customer _customer;
        public Customer Customer
        {
            get => _customer;
            set => SetProperty(ref _customer, value);
        }

        private int _orderId;
        public int OrderId
        {
            get => _orderId;
            set => SetProperty(ref _orderId, value);
        }

        private decimal? _totalOrderPrice;
        public decimal? TotalOrderPrice
        {
            get => _totalOrderPrice;
            set
            {
                _totalOrderPrice = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<OrderProductViewModel> Products { get; }

        public ICommand SaveOrderCommand { get; }

        public OrderViewModel()
        {
            Products = new ObservableCollection<OrderProductViewModel>();
            _orderService = ServiceHelper.GetService<IOrderService>();
            _customerService = ServiceHelper.GetService<ICustomerService>();
            SaveOrderCommand = new Command(async () => await SaveOrder());
        }

        public async Task LoadOrder()
        {
            if (OrderId == 0)
            {
                var customerProducts = await _customerService.GetProductsAsync(Customer.Id);

                Order = new Order
                {
                    CustomerId = Customer.Id,
                    Date = DateTime.Now,
                    Status = OrderStatus.Created
                };

                foreach (var cp in customerProducts)
                {
                    var productViewModel = new OrderProductViewModel
                    {
                        ProductName = cp.Product.Name,
                        ProductPrice = cp.Price
                    };
                    Products.Add(productViewModel);
                    productViewModel.TotalPriceChanged += OnProductTotalPriceChanged;
                }
            }
            else
            {
                Order = await _orderService.GetByIdAsync(OrderId);

                foreach (var op in Order.Products)
                {
                    Products.Add(new OrderProductViewModel
                    {
                        Id = op.Id,
                        ProductName = op.ProductName,
                        ProductPrice = op.ProductPrice,
                        ActualWeight = op.ActualWeight,
                        OrderedWeight = op.OrderedWeight,
                        OrderId = op.OrderId
                    });
                }
            }

            CalculateTotalOrderPrice();
        }

        private void OnProductTotalPriceChanged(object sender, EventArgs e)
        {
            CalculateTotalOrderPrice();
        }

        private void CalculateTotalOrderPrice()
        {
            TotalOrderPrice = Products.Sum(p => p.TotalPrice);
        }

        private async Task SaveOrder()
        {
            if (Order.Status == OrderStatus.Delivered)
            {
                await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Помилка", "Не можна змінити доставлене замовлення!", "OK");
            }

            if (OrderId == default)
            {
                Order.TotalPrice = TotalOrderPrice;
                Order.TotalWeight = Products.Sum(x => x.ActualWeight);
                Order.Products = Products.Select(p => new OrderProduct
                {
                    ProductName = p.ProductName,
                    ProductPrice = p.ProductPrice,
                    OrderedWeight = p.OrderedWeight,
                    ActualWeight = p.ActualWeight,
                    TotalPrice = p.TotalPrice
                }).ToList();

                Order.Status = Products.Any(x => x.ActualWeight.HasValue && x.ActualWeight != 0)
                    ? OrderStatus.Calculated
                    : OrderStatus.Created;

                await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Успішно", "Замовлення створено!", "OK");
            }
            else
            {
                Order.TotalPrice = TotalOrderPrice;
                Order.TotalWeight = Products.Sum(x => x.ActualWeight);
                Order.Products = Products.Select(p => new OrderProduct
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    ProductPrice = p.ProductPrice,
                    OrderedWeight = p.OrderedWeight,
                    ActualWeight = p.ActualWeight,
                    TotalPrice = p.TotalPrice
                }).ToList();

                Order.Status = Products.Any(x => x.ActualWeight.HasValue && x.ActualWeight != 0)
                    ? OrderStatus.Calculated
                    : OrderStatus.Created;

                await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Успішно", "Замовлення змінено!", "OK");
            }

            await Shell.Current.GoToAsync("..");
        }

        private 
    }
}
