using Mega_Market_App.Command;
using Mega_Market_App.Views;
using Mega_Market_Data.Data;
using Mega_Market_Data.Models.Concretes;
using Mega_Market_Data.Repositoies;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;

namespace Mega_Market_App.ViewModels;

public class HistoryViewModel : BaseViewModel , INotifyPropertyChanged
{
    private readonly CheckViewModel _checkViewModel;
    private Page? _currentPageCheck;

    public Page? CurrentPageCheck { get => _currentPageCheck; set { _currentPageCheck = value; OnPropertyChanged(); } }

    private ObservableCollection<History> ?_histories;
    public ObservableCollection<History> Histories
    {
		get => _histories!; 
		set { _histories = value; OnPropertyChanged(); }
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


    public RelayCommand ShowCheckCommand { get; set; }
        
    public IRepository<History, UserDbContext> HistoryRepository;
    private readonly LoginViewModel _loginViewModel;
    private User _user = new();
    public HistoryViewModel(IRepository<History,UserDbContext> historyRepository, CheckViewModel checkViewModel, LoginViewModel loginViewModel)
    {
        _loginViewModel = loginViewModel;
        _checkViewModel = checkViewModel;
        HistoryRepository = historyRepository;

        Histories = [];

        UserCheck();

        ShowCheckCommand = new RelayCommand(ShowCheckClick);
    }

    void UserCheck()
    {
        foreach (var user in _loginViewModel.UserRepository.GetAll())
        {
            if (_loginViewModel.UserLogin.Mail == user.Mail)
            {
                _user = user;
            }
        }

        foreach (var history in HistoryRepository.GetAll())
        {
            if (history.UserId == _user.Id) Histories!.Add(history);
        }

    }

    private void ShowCheckClick(object? obj)
    {
        _checkViewModel.Products.Clear();
        if (obj is History history)
        {
            _checkViewModel.AddCheckHistory(history);

            CurrentPageCheck = App.Container.GetInstance<CheckView>();
            CurrentPageCheck.DataContext = _checkViewModel;
        }
    }

    public void AddHistory(History history)
    {
        HistoryRepository.Add(history);
        HistoryRepository.SaveChanges();
    }


    private void SortDailyTotals()
    {
        if (Histories == null) return;

        IEnumerable<History> sortedList = null!;
        var sortOption = SelectedSortOptionIndex == 0 ? "Ascending" : "Descending";

        if (sortOption == "Ascending")
        {
            sortedList = Histories.OrderBy(d => d.Date);
        }
        else
        {
            sortedList = Histories.OrderByDescending(d => d.Date);
        }

        Histories = new ObservableCollection<History>(sortedList);
    }


}
