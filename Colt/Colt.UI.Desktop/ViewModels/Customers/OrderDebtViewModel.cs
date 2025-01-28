namespace Colt.UI.Desktop.ViewModels.Customers
{
    public class OrderDebtViewModel
    {
        public decimal Produce { get; set; }
        public decimal Receive { get; set; }
        public decimal Debt => Produce - Receive;
    }
}
