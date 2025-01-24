using Colt.Application.Interfaces;
using Colt.Domain.Entities;
using Colt.UI.Desktop.Helpers;
using System.Windows.Input;

namespace Colt.UI.Desktop.ViewModels.Customers
{
    public class ModifyCustomerViewModel : BaseViewModel
    {
        private readonly ICustomerService _customerService;
        private Customer _customer;
        public Customer Customer
        {
            get => _customer;
            set
            {
                _customer = value;
                OnPropertyChanged();
            }
        }
        public ICommand SaveCustomerCommand { get; }

        public ModifyCustomerViewModel()
        {
            _customerService = ServiceHelper.GetService<ICustomerService>();
            SaveCustomerCommand = new Command(async () => await SaveCustomer());
            Customer = new Customer();
        }

        private async Task SaveCustomer()
        {
            if (string.IsNullOrWhiteSpace(Customer.Name) || string.IsNullOrWhiteSpace(Customer.PhoneNumber))
            {
                await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Помилка", "Назва обов'язкова!", "OK");
                return;
            }

            if (Customer.Id == 0)
            {
                await _customerService.InsertAsync(Customer);
                await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Успішно", "Клієнт додано!", "OK");
            }
            else
            {
                await _customerService.UpdateAsync(Customer);
                await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Успішно", "Клієнт змінено!", "OK");
            }
            await Shell.Current.GoToAsync("..");
        }
    }
}
