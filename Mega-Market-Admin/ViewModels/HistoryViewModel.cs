using Mega_Market_Admin.Command;
using Mega_Market_Data.Data;
using Mega_Market_Data.Models.Concretes;
using Mega_Market_Data.Repositoies;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Mega_Market_Admin.ViewModels;

public class HistoryViewModel : BaseViewModel
{

    private ObservableCollection<DailyTotal>? _dailyTotals;
    public ObservableCollection<DailyTotal> DailyTotals
    {
        get => _dailyTotals!;
        set
        {
            _dailyTotals = value;
            OnPropertyChanged();
        }
    }

    private int _selectedSortOptionIndex;
    public int SelectedSortOptionIndex
    {
        get { return _selectedSortOptionIndex; }
        set
        {
            _selectedSortOptionIndex = value;
            OnPropertyChanged(nameof(SelectedSortOptionIndex));
            SortDailyTotals();
        }
    }

    private readonly IRepository<History, UserDbContext> _historyRepository;
    private readonly IRepository<DailyTotal,AdminDbContext> _dailyTotalRepository;
    private MenyuViewModel _menyuViewModel;

    public RelayCommand ShowCommand { get; set; }


    public HistoryViewModel(IRepository<History, UserDbContext> historyRepository,
        IRepository<DailyTotal, AdminDbContext> dailyTotalRepository,MenyuViewModel menyuViewModel)
    {
        _historyRepository = historyRepository;
        _dailyTotalRepository = dailyTotalRepository;
        _menyuViewModel = menyuViewModel;

        ShowCommand = new RelayCommand(ShowClick);
        
        InputValue();

        DailyTotals = new ObservableCollection<DailyTotal>(_dailyTotalRepository.GetAll());

    }

    private void ShowClick(object? obj)
    {
        _menyuViewModel.CheckClick(obj);
    }

    private void InputValue()
    {
        var totalsByDate = _historyRepository.GetAll()
            .GroupBy(history => history.Date!.Value.Date)
            .Select(group => new
            {
                Date = group.Key,
                TotalAmount = group.Sum(history => history.TotalAmount),
                UserCount = group.Select(history => history.User!.Mail).Distinct().Count()
            });

        foreach (var total in totalsByDate)
        {
            if (total.Date != DateTime.Now.Date)
            {

                bool dateExistsInDailyTotal = _dailyTotalRepository.GetAll()
                    .Any(dailyTotal => dailyTotal.DateTime!.Value.Date == total.Date);

                var matchTotal = Math.Round((double)(total.TotalAmount!), 2);

                if (!dateExistsInDailyTotal)
                {
                    var dailyTotal = new DailyTotal()
                    {
                        DateTime = total.Date,
                        TotalAmount = matchTotal,
                        UserCount = total.UserCount,
                    };

                    _dailyTotalRepository.Add(dailyTotal);
                    _dailyTotalRepository.SaveChanges();
                }
            }
        }
    }

    private void SortDailyTotals()
    {
        if (DailyTotals == null) return;

        IEnumerable<DailyTotal> sortedList = null!;
        var sortOption = SelectedSortOptionIndex == 0 ? "Ascending" : "Descending";

        if (sortOption == "Ascending")
        {
            sortedList = DailyTotals.OrderBy(d => d.DateTime);
        }
        else
        {
            sortedList = DailyTotals.OrderByDescending(d => d.DateTime);
        }

        DailyTotals = new ObservableCollection<DailyTotal>(sortedList);
    }

}
