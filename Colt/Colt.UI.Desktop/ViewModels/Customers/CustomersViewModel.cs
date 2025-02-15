using Colt.Application.Interfaces;
using Colt.Application.Services;
using Colt.Domain.Entities;
using Colt.UI.Desktop.Helpers;
using Colt.UI.Desktop.Views;
using Serilog;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Colt.UI.Desktop.ViewModels.Customers
{
    public class CustomersViewModel : BaseViewModel
    {
        private readonly ICustomerService _customerService;
        public ObservableCollection<Customer> Customers { get; }

        public ICommand NavigateToAddCustomerCommand { get; }
        public ICommand EditCustomerCommand { get; }
        public ICommand DeleteCustomerCommand { get; }

        public CustomersViewModel()
        {
            _customerService = ServiceHelper.GetService<ICustomerService>();
            Customers = new ObservableCollection<Customer>();
            NavigateToAddCustomerCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(ModifyCustomerPage)));
            EditCustomerCommand = new Command<Customer>(async (customer) => await NavigateToModifyCustomer(customer));
            DeleteCustomerCommand = new Command<Customer>(async (customer) => await DeleteCustomer(customer));
        }

        public async Task Initialize()
        {
            await LoadCustomers();
        }

        private async Task LoadCustomers()
        {
            try
            {
                var customers = await _customerService.GetAllAsync();
                Customers.Clear();
                foreach (var customer in customers)
                {
                    Customers.Add(customer);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to load customers");
                await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Помилка", $"Виникла критична поилка, звяжіться з розробником!!!\n Помилка: {ex.Message}", "OK");
            }
        }

        private async Task NavigateToModifyCustomer(Customer customer)
        {
            var navigationParameter = new Dictionary<string, object>
            {
                { "Customer", customer }
            };

            await Shell.Current.GoToAsync(nameof(ModifyCustomerPage), navigationParameter);
        }

        private async Task DeleteCustomer(Customer customer)
        {
            try
            {

                bool isConfirmed = await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Підтвердження", "Ви впевнені, що хочете видалити цього оптовика?", "Так", "Ні");
                if (isConfirmed)
                {
                    await _customerService.DeleteAsync(customer.Id);
                    Customers.Remove(customer);
                    await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Успішно", "Оптовика видалено!", "OK");
                }
            }
            catch (Exception ex)
            {
                await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Помилка", $"Виникла критична поилка, звяжіться з розробником!!!\n Помилка: {ex.Message}", "OK");
                Log.Error(ex, "Failed to delete customer"); 
            }
        }
    }
}
