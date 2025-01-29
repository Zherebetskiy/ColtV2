namespace Colt.Domain.Common
{
    public class OrderDebtModel
    {
        public decimal Produce { get; set; }
        public decimal Receive { get; set; }
        public decimal Debt => Produce - Receive;
    }
}
