using Colt.Application.Interfaces;
using Colt.Domain.Entities;
using Colt.UI.Desktop.Helpers;
using Colt.UI.Desktop.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Colt.UI.Desktop.ViewModels.Customers
{
    public class CustomersViewModel : BaseViewModel
    {
        private readonly ICustomerService _customerService;
        public ObservableCollection<Customer> Customers { get; }

        public ICommand LoadCustomersCommand { get; }
        public ICommand NavigateToModifyCustomerCommand { get; }

        public CustomersViewModel()
        {
            _customerService = ServiceHelper.GetService<ICustomerService>();
            Customers = new ObservableCollection<Customer>();
            LoadCustomersCommand = new Command(async () => await LoadCustomers());
            NavigateToModifyCustomerCommand = new Command(async () => await NavigateToModifyCustomer());
        }

        public async Task Initialize()
        {
            await LoadCustomers();
        }

        private async Task LoadCustomers()
        {
            var customers = await _customerService.GetAllAsync();
            Customers.Clear();
            foreach (var customer in customers)
            {
                Customers.Add(customer);
            }
        }

        private async Task NavigateToModifyCustomer()
        {
            await Shell.Current.GoToAsync(nameof(ModifyCustomerPage));
        }
    }
}
