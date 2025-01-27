using System.ComponentModel;

namespace Colt.UI.Desktop.ViewModels.Orders
{
    public class OrderProductViewModel : BaseViewModel
    {
        private double? _actualWeight;

        public int Id { get; set; }
        public int OrderId { get; set; }
        public string ProductName { get; set; }

        public decimal? ProductPrice { get; set; }

        public double? OrderedWeight{ get; set; }

        public double? ActualWeight
        {
            get => _actualWeight;
            set
            {
                _actualWeight = value;
                OnPropertyChanged(nameof(ActualWeight));
                OnPropertyChanged(nameof(TotalPrice));
                TotalPriceChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public decimal? TotalPrice => (decimal?)ActualWeight * ProductPrice;

        public event EventHandler TotalPriceChanged;
    }
}


