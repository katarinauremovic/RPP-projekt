using BusinessLogicLayer.Services;
using EntityLayer.DTOs;
using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PresentationLayer.UserControls
{
    public partial class ucTreatmentStatistics : UserControl
    {
        private readonly TreatmentService _treatmentService = new TreatmentService();
       
        public MainWindow Parent { get; set; }
        
        public ucTreatmentStatistics()
        {
            InitializeComponent();
 
            _ = LoadStatistics();
        }

        private async Task LoadStatistics()
        {
            var statistics = await _treatmentService.GetTreatmentStatisticsByGroupAsync();
            if (!statistics.Any()) return;

            var groupedData = statistics.GroupBy(t => t.GroupName).ToList();

            stackPanelGraphs.Children.Clear(); 

            pieChart.Series.Clear();
            foreach (var group in groupedData)
            {
                pieChart.Series.Add(new PieSeries
                {
                    Title = group.Key,
                    Values = new ChartValues<int> { group.Sum(t => t.TotalTimesPerformed) },
                    DataLabels = true
                });
            }

            foreach (var group in groupedData)
            {
               
                var groupTitle = new TextBlock
                {
                    Text = group.Key,
                    FontSize = 18,
                    FontWeight = FontWeights.Bold,
                    Foreground = Brushes.Black,
                    Margin = new Thickness(0, 20, 0, 10)
                };
                stackPanelGraphs.Children.Add(groupTitle);

                var series = new ColumnSeries
                {
                    Title = group.Key,
                    Values = new ChartValues<int>(group.Select(t => t.TotalTimesPerformed)),
                    DataLabels = true
                };

                var labels = group.Select(t => t.TreatmentName).ToList();

                var chart = new CartesianChart
                {
                    Height = 300,
                    Series = new SeriesCollection { series },
                    Margin = new Thickness(0, 10, 0, 20)
                };

                chart.AxisX.Add(new Axis
                {
                    Title = "Treatment Name",
                    Labels = labels,
                    Foreground = Brushes.Black,
                    FontSize = 14,
                    Separator = new LiveCharts.Wpf.Separator 
                    {
                        StrokeThickness = 1,
                        Stroke = Brushes.Gray
                    },
                    Margin = new Thickness(0, 10, 0, 0)
                });

                chart.AxisY.Add(new Axis
                {
                    Title = "Usage Count",
                    Foreground = Brushes.Black,
                    FontSize = 14,
                    MinValue = 0,
                    LabelFormatter = value => ((int)value).ToString(),
                    Separator = new LiveCharts.Wpf.Separator 
                    {
                        StrokeThickness = 1,
                        Stroke = Brushes.Gray
                    }
                });

                stackPanelGraphs.Children.Add(chart);
            }
        }
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadStatistics();
        }
    }
}
