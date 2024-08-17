using Mega_Market_Data.Data;
using Mega_Market_Data.Models.Concretes;
using Mega_Market_Data.Repositoies;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Mega_Market_App.ViewModels;

public class CheckViewModel : BaseViewModel , INotifyPropertyChanged
{
    private History? _history;
    public History History
    {
        get => _history!;
        set { _history = value; OnPropertyChanged(); }
    }

    private DateTime? _Time;

    public DateTime? Time
    {
        get => _Time; 
        set { _Time = value; OnPropertyChanged(); }
    }

    private double? _total;

    public double? Total
    {
        get => _total; 
        set { _total = value; OnPropertyChanged(); }
    }


    private ObservableCollection<ProductHistory>? products;
    public ObservableCollection<ProductHistory> Products { get => products!; set { products = value; OnPropertyChanged(); } }

    private IRepository<ProductHistory,UserDbContext> _productHistoryRepository;
    public CheckViewModel(IRepository<ProductHistory,UserDbContext> productHistoryRepository)
    {
        _productHistoryRepository = productHistoryRepository;
        History = new();
        Time = History.Date;
        Total = History.TotalAmount;
        Products = [];
    }

    public void AddCheckHistory(History history)
    {
        History = history;
        Time = history.Date;
        Total = history.TotalAmount;
        foreach (var productHistory in _productHistoryRepository.GetAll())
            if (productHistory.History == history) Products.Add(productHistory);
        
    }
}
