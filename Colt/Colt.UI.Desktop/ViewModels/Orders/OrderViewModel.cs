using Colt.Application.Interfaces;
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
            var customerProducts = await _customerService.GetProductsAsync(Customer.Id);

            if (OrderId == 0)
            {
                Order = new Order
                {
                    CustomerId = Customer.Id,
                    Date = DateTime.Now,
                    Status = OrderStatus.Created,
                    Delivery = DateTime.Now
                };

                foreach (var cp in customerProducts)
                {
                    var productViewModel = new OrderProductViewModel
                    {
                        ProductName = cp.Product.Name,
                        ProductType = cp.Product.MeasurementType,
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
                    var productViewModel = new OrderProductViewModel
                    {
                        Id = op.Id,
                        ProductName = op.ProductName,
                        ProductPrice = op.ProductPrice,
                        ProductType = op.ProductType,
                        ActualWeight = op.ActualWeight,
                        OrderedWeight = op.OrderedWeight,
                        OrderId = op.OrderId
                    };

                    Products.Add(productViewModel);
                    productViewModel.TotalPriceChanged += OnProductTotalPriceChanged;
                }

                var missedProducts = customerProducts
                    .Where(x => !Products.Any(y => y.ProductName == x.Product.Name))
                    .ToList();

                foreach (var mp in missedProducts)
                {
                    var productViewModel = new OrderProductViewModel
                    {
                        ProductName = mp.Product.Name,
                        ProductType = mp.Product.MeasurementType,
                        ProductPrice = mp.Price
                    };

                    Products.Add(productViewModel);
                    productViewModel.TotalPriceChanged += OnProductTotalPriceChanged;
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
            bool isConfirmed = true;
            if (Order.Status == OrderStatus.Delivered)
            {
                isConfirmed = await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Підтвердження", "Замовлення вже доставлено, ви впевнені що бажаєте змінити його?", "Так", "Ні");
            }

            if (!isConfirmed)
            {
                return;
            }

            Order.TotalPrice = TotalOrderPrice;
            Order.TotalWeight = Products.Sum(x => x.ActualWeight);
            Order.Products = Products.Select(p => new OrderProduct
            {
                Id = p.Id,
                ProductName = p.ProductName,
                ProductPrice = p.ProductPrice,
                ProductType = p.ProductType,
                OrderedWeight = p.OrderedWeight,
                ActualWeight = p.ActualWeight,
                TotalPrice = p.TotalPrice
            }).ToList();

            Order.Status = Products.Any(x => x.ActualWeight.HasValue && x.ActualWeight != 0)
                ? OrderStatus.Calculated
                : OrderStatus.Created;

            if (OrderId == default)
            {
                await _orderService.InsertAsync(Order);

                await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Успішно", "Замовлення створено!", "OK");
            }
            else
            {
                await _orderService.UpdateAsync(Order);

                await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Успішно", "Замовлення змінено!", "OK");
            }

            await Shell.Current.GoToAsync("..");
        }
    }
}
