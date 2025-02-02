using Colt.UI.Desktop.ViewModels.Statistics;
using Microcharts;
using SkiaSharp;

namespace Colt.UI.Desktop.Views;

public partial class StatisticsPage : ContentPage
{
    private StatisticsViewModel ViewModel => BindingContext as StatisticsViewModel;

    public StatisticsPage()
	{
		InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (ViewModel != null)
        {
            await ViewModel.LoadCustomers();
            await ViewModel.LoadProducts();
            await LoadDebtChart();
            await LoadIncomeChart();
            await LoadProductsChart();
        }
    }

    private async Task LoadDebtChart()
    {
        await ViewModel.LoadCustomerDebts();

        var entries = ViewModel.CustomerDebtChartEntries.Select(entry => new ChartEntry((float)entry.DebtAmount)
        {
            Label = entry.CustomerName,
            ValueLabel = entry.DebtAmount.ToString("C"),
            Color = GetRandomColor()
        }).ToArray();

        if (entries.Length == 0)
        {
            entries = new ChartEntry[]
            {
                new ChartEntry(0)
                {
                    Label = "Немає даних",
                    ValueLabel = "0",
                    Color = SKColor.Parse("#FF1414")
                }
            };
        }

        CustomerDebtChart.Chart = new BarChart
        {
            Entries = entries,
            ValueLabelOption = ValueLabelOption.TopOfChart
        };
    }

    private async Task LoadIncomeChart()
    {
        await ViewModel.LoadIncomeChart();

        var incomeEntries = ViewModel.IncomeChartEntries.Select(entry => new ChartEntry((float)entry.Amount)
        {
            Label = entry.Date,
            ValueLabel = entry.Amount.ToString("C"),
            Color = GetRandomColor()
        }).ToArray();

        if(incomeEntries.Length == 0)
        {
            incomeEntries = new ChartEntry[]
            {
                new ChartEntry(0)
                {
                    Label = "Немає даних",
                    ValueLabel = "0",
                    Color = SKColor.Parse("#FF1414")
                }
            };
        }

        IncomeChart.Chart = new LineChart
        {
            Entries = incomeEntries,
            ValueLabelOption = ValueLabelOption.TopOfElement
        };
    }

    private async Task LoadProductsChart()
    {
        await ViewModel.LoadProductsChart();

        var productEntries = ViewModel.ProductsChartEntries.Select(entry => new ChartEntry((float)entry.Amount)
        {
            Label = entry.Date,
            ValueLabel = entry.Amount.ToString(),
            Color = GetRandomColor()
        }).ToArray();

        if (productEntries.Length == 0)
        {
            productEntries = new ChartEntry[]
            {
                new ChartEntry(0)
                {
                    Label = "Немає даних",
                    ValueLabel = "0",
                    Color = SKColor.Parse("#FF1414")
                }
            };
        }

        ProductsChart.Chart = new LineChart
        {
            Entries = productEntries,
            ValueLabelOption = ValueLabelOption.TopOfElement
        };
    }

    private async void OnLoadIncomeChartClicked(object sender, EventArgs e)
    {
        LoadIncomeChart();
    }

    private async void OnLoadProductChartClicked(object sender, EventArgs e)
    {
        LoadProductsChart();
    }

    private SKColor GetRandomColor()
    {
        Random random = new Random();
        return new SKColor(
            (byte)random.Next(256),
            (byte)random.Next(256),
            (byte)random.Next(256)
        );
    }
}
