using Colt.Application.Interfaces;
using Colt.UI.Desktop.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colt.UI.Desktop.ViewModels.Statistics
{
    public class StatisticsViewModel : BaseViewModel
    {
        private readonly ICustomerService _customerService;

        public ObservableCollection<CustomerDebtChartEntry> CustomerDebtChartEntries { get; }

        public StatisticsViewModel()
        {
            _customerService = ServiceHelper.GetService<ICustomerService>();
            CustomerDebtChartEntries = new ObservableCollection<CustomerDebtChartEntry>();
        }

        public async Task LoadCustomerDebts()
        {
            var customers = await _customerService.GetAllAsync();
            var customerDebts = new List<CustomerDebtChartEntry>();

            foreach (var customer in customers)
            {
                var debt = await _customerService.GetDebtAsync(customer.Id);
                customerDebts.Add(new CustomerDebtChartEntry
                {
                    CustomerName = customer.Name,
                    DebtAmount = debt.Debt
                });
            }

            CustomerDebtChartEntries.Clear();
            foreach (var entry in customerDebts)
            {
                CustomerDebtChartEntries.Add(entry);
            }
        }
    }

    public class CustomerDebtChartEntry
    {
        public string CustomerName { get; set; }
        public decimal DebtAmount { get; set; }
    }
}
