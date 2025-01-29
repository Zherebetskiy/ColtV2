using Colt.Application.Interfaces;
using Colt.Domain.Common;
using Colt.Domain.Entities;
using Colt.Domain.Repositories;
namespace Colt.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IPaymentRepository _paymentRepository;

        public CustomerService(
            ICustomerRepository customerRepository,
            IPaymentRepository paymentRepository)
        {
            _customerRepository = customerRepository;
            _paymentRepository = paymentRepository;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _customerRepository.GetByIdAsync(id, CancellationToken.None);
            return await _customerRepository.DeleteAsync(product, CancellationToken.None);
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            return await _customerRepository.GetAsync(CancellationToken.None);
        }

        public async Task InsertAsync(Customer customer)
        {
            if (string.IsNullOrWhiteSpace(customer.Name) || string.IsNullOrWhiteSpace(customer.PhoneNumber))
                throw new ArgumentException("Name and PhoneNumber are required.");

            if (customer.Products != null && customer.Products.Count != 0)
            {
                foreach (var product in customer.Products)
                {
                    product.Product = null;
                }
            }

            await _customerRepository.AddAsync(customer, CancellationToken.None);
        }

        public Task<List<CustomerProduct>> GetProductsAsync(int customerId)
        {
            return _customerRepository.GetProductsByCustomerIdAsync(customerId, CancellationToken.None);
        }

        public async Task<Customer> UpdateAsync(Customer customer)
        {
            var existingCustomer = await _customerRepository.GetByIdAsync(customer.Id, CancellationToken.None);

            existingCustomer.Name = customer.Name;
            existingCustomer.PhoneNumber = customer.PhoneNumber;
            existingCustomer.Notes = customer.Notes;

            await HandleProductsAsync(existingCustomer, customer);
            await HandlePaymentsAsync(existingCustomer, customer);

            return await _customerRepository.UpdateAsync(existingCustomer, CancellationToken.None);
        }

        public Task<OrderDebtModel> GetDebtAsync(int id)
        {
            return _customerRepository.GetDebtAsync(id, CancellationToken.None);
        }

        private async Task HandleProductsAsync(Customer existingCustomer, Customer customer)
        {
            var addedProducts = customer.Products.Where(x => x.Id == 0).ToList();
            var updatedProducts = customer.Products.Where(x => x.Id != 0).ToList();
            var removedProducts = existingCustomer.Products
                .Where(x => !customer.Products.Any(y => x.ProductId == y.ProductId))
                .ToList();

            await _customerRepository.DeleteProductsAsync(removedProducts, CancellationToken.None);

            foreach (var product in addedProducts)
            {
                product.CustomerId = existingCustomer.Id;
                product.Product = null;
                existingCustomer.Products.Add(product);
            }

            foreach (var product in updatedProducts)
            {
                var existingProduct = existingCustomer.Products.First(x => x.ProductId == product.ProductId);
                existingProduct.Price = product.Price;
            }
        }

        private async Task HandlePaymentsAsync(Customer existingCustomer, Customer customer)
        {
            var addedPayments = customer.Payments.Where(x => x.Id == 0).ToList();
            var updatedPayments = customer.Payments.Where(x => x.Id != 0).ToList();
            var removedPayments = existingCustomer.Payments
                .Where(x => !customer.Payments.Any(y => x.Id == y.Id))
                .ToList();

            await _paymentRepository.DeletePaymentsAsync(removedPayments, CancellationToken.None);

            foreach (var payment in addedPayments)
            {
                existingCustomer.Payments.Add(payment);
            }

            foreach (var payment in updatedPayments)
            {
                var existingPayment = existingCustomer.Payments.First(x => x.Id == payment.Id);
                existingPayment.Amount = payment.Amount;
                existingPayment.Date = payment.Date;
            }
        }
    }
}
