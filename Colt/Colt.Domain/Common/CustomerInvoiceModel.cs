namespace Colt.Domain.Common
{
    public class CustomerInvoiceModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public decimal Debt { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public decimal TotalPrice { get; set; }
        public List<ProductInvoiceModel> Products { get; set; }
    }
}
