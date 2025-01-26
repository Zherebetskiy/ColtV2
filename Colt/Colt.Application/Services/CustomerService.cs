using Colt.Application.Interfaces;
using Colt.Domain.Entities;
using Colt.Domain.Repositories;
namespace Colt.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
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

            await _customerRepository.AddAsync(customer, CancellationToken.None);
        }

        public async Task<Customer> UpdateAsync(Customer customer)
        {
            var existingCustomer = await _customerRepository.GetByIdAsync(customer.Id, CancellationToken.None);

            existingCustomer.Name = customer.Name;
            existingCustomer.PhoneNumber = customer.PhoneNumber;
            existingCustomer.Notes = customer.Notes;

            return await _customerRepository.UpdateAsync(existingCustomer, CancellationToken.None);
        }
    }
}
