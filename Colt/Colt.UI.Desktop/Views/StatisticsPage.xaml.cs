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
            await ViewModel.LoadCustomerDebts();

            var entries = ViewModel.CustomerDebtChartEntries.Select(entry => new ChartEntry((float)entry.DebtAmount)
            {
                Label = entry.CustomerName,
                ValueLabel = entry.DebtAmount.ToString("C"),
                Color = GetRandomColor()
            }).ToArray();

            CustomerDebtChart.Chart = new BarChart
            { 
                Entries = entries,
                ValueLabelOption = ValueLabelOption.TopOfChart
            };
        }
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
