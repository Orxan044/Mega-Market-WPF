using LiveCharts.Wpf;
using LiveCharts;
using Mega_Market_Data.Data;
using Mega_Market_Data.Models.Concretes;
using Mega_Market_Data.Repositoies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mega_Market_Admin.Command;
using ToastNotifications.Messages;

namespace Mega_Market_Admin.ViewModels;

public class StaticViewModel : BaseViewModel
{

    private DateTime? _startDate;
    public DateTime? StartDate
    {
        get => _startDate; 
        set
        {
            if (_startDate != value)
            {
                _startDate = value;
                OnPropertyChanged();
            }
        }
    }

    private DateTime? _endDate;
    public DateTime? EndDate
    {
        get => _endDate; 
        set
        {
            if (_endDate != value)
            {
                _endDate = value;
                OnPropertyChanged();
            }
        }
    }


    public RelayCommand GetDateTimeCommand { get; set; }
    public SeriesCollection? SeriesCollection { get; set; }
    public Axis? XAxis { get; set; }
    public Axis? YAxis { get; set; }

    private readonly IRepository<DailyTotal, AdminDbContext> _dailyTotalRepository;
    public StaticViewModel(IRepository<DailyTotal, AdminDbContext> dailyTotalRepository)
    {
        _dailyTotalRepository = dailyTotalRepository;

        GetDateTimeCommand = new RelayCommand(GetDateTimeClick);

        LoadChartData(DateTime.Now.AddMonths(-1), DateTime.Now); // Örneğin, son bir ay için veri yükleme
    }

    private void GetDateTimeClick(object? obj)
    {
        try
        {
            if (StartDate is not null && EndDate is not null && StartDate.Value.Date < DateTime.Now.Date
                && EndDate.Value.Date <= DateTime.Now.Date && StartDate.Value.Date < EndDate.Value.Date)
            {
                LoadChartData(StartDate, EndDate);
            }
            else notifier.ShowError("StartDate and EndDate Enter Correctly !!!");
        }
        catch (Exception)
        {

            notifier.ShowError("StartDate and EndDate Enter Correctly !!!");
        }
    }

    public void LoadChartData(DateTime? startDate, DateTime? endDate)
    {
        var data = _dailyTotalRepository.GetAll()
            .Where(d => d.DateTime >= startDate && d.DateTime <= endDate)
            .OrderBy(d => d.DateTime) // Tarihe göre sıralama
            .ToList();

        var values = data.Select(d => new { Date = d.DateTime!.Value, Amount = d.TotalAmount!.Value }).ToList();
        var maxAmount = values.Max(v => v.Amount);

        SeriesCollection = new SeriesCollection
        {
            new LineSeries
            {
                Title = "Total Amount",
                Values = new ChartValues<double>(values.Select(v => v.Amount)),
                PointGeometrySize = 0
            }
        };

        // X ekseni etiketlerini günlük olarak ayarlamak için formatı değiştiriyoruz
        XAxis = new Axis
        {
            Title = "Date",
            Labels = values.Select(v => v.Date.ToString("dd MMM yyyy")).ToArray()
        };

        YAxis = new Axis
        {
            Title = "Total Amount",
            MinValue = 0,
            MaxValue = maxAmount
        };

        // UI'nın güncellenmesi için property değişiklik bildirimi
        OnPropertyChanged(nameof(SeriesCollection));
        OnPropertyChanged(nameof(XAxis));
        OnPropertyChanged(nameof(YAxis));
    }
}
