using Colt.Application.Interfaces;
using Colt.Domain.Common;
using Colt.Domain.Entities;
using Colt.Domain.Enums;
using Colt.Domain.Repositories;
using System.Reflection;

namespace Colt.Application.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IDocumentService _documentService;

        public InvoiceService(
            IOrderRepository orderRepository,
            IDocumentService documentService)
        {
            _orderRepository = orderRepository;
            _documentService = documentService;
        }

        public async Task<string> GenerateInvoiceAsync(Customer customer, Order order, OrderDebtModel debt)
        {
            var orderProducts = await _orderRepository.GetProductsAsync(order.Id, CancellationToken.None);

            var model = GetInvoiceModel(customer, order, debt, orderProducts);

            var inputPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Resources", "InvoiceTemplate.docx");
            var docName = $"{customer.Name} - {order.Id}.docx";
            var outputPath = $"C:\\Users\\zhere\\OneDrive\\Робочий стіл\\Invoices\\{order.Delivery:dd.MMM yyyy}\\{docName}";

            _documentService.ProcessFile(model, inputPath, outputPath);

            return outputPath;
        }

        private CustomerInvoiceModel GetInvoiceModel(
            Customer customer,
            Order order,
            OrderDebtModel debt,
            List<OrderProduct> orderProducts)
        {
            return new CustomerInvoiceModel
            {
                Id = customer.Id,
                CustomerName = customer.Name,
                CustomerPhone = customer.PhoneNumber,
                Debt = debt.Debt,
                DeliveryDate = order.Delivery,
                OrderDate = order.Date,
                TotalPrice = order.TotalPrice ?? 0.0m,
                Products = orderProducts.Select(x => new ProductInvoiceModel
                {
                    Name = x.ProductName,
                    ActualWeight = x.ActualWeight ?? 0.0,
                    Price = x.ProductPrice ?? 0.0m,
                    TotalPrice = x.TotalPrice ?? 0.0m,
                    Type = x.ProductType.HasValue ? GetType(x.ProductType.Value) : "за кілограм"
                }).ToList()
            };
        }

        private string GetType(MeasurementType productType)
            => productType switch
                {
                    MeasurementType.Weight => "за кілограм",
                    MeasurementType.Quantity => "за одиницю",
                    _ => "за кілограм"
                };
    }
}
