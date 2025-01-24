using Microsoft.Maui.Controls;
using Colt.UI.Desktop.ViewModels.Products;
using Colt.Domain.Entities;

namespace Colt.UI.Desktop.Views
{
    [QueryProperty(nameof(Product), "Product")]
    public partial class AddProductPage : ContentPage
    {
        public AddProductPage()
        {
            InitializeComponent();
        }

        public Product Product
        {
            set
            {
                if (BindingContext is AddProductViewModel viewModel)
                {
                    viewModel.Product = value ?? new Product();
                }
            }
        }
    }
}
