﻿using Colt.UI.Desktop.Views;

namespace Colt.UI.Desktop
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(StatisticsPage), typeof(StatisticsPage));
            Routing.RegisterRoute(nameof(ProductsPage), typeof(ProductsPage));
            Routing.RegisterRoute(nameof(AddProductPage), typeof(AddProductPage));
            Routing.RegisterRoute(nameof(CustomersPage), typeof(CustomersPage));
            Routing.RegisterRoute(nameof(ModifyCustomerPage), typeof(ModifyCustomerPage));
            Routing.RegisterRoute(nameof(OrderPage), typeof(OrderPage));
        }
    }
}
