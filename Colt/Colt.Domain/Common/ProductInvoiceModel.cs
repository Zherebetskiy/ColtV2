namespace Colt.Domain.Common
{
    public class ProductInvoiceModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
        public double ActualWeight { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
