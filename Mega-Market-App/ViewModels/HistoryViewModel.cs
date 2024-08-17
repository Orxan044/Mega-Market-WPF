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


    public RelayCommand ShowCheckCommand { get; set; }
        
    public IRepository<History, UserDbContext> HistoryRepository;
    public HistoryViewModel(IRepository<History,UserDbContext> historyRepository,CheckViewModel checkViewModel)
    {
        HistoryRepository = historyRepository;
        _checkViewModel = checkViewModel;

        Histories = new ObservableCollection<History>(HistoryRepository.GetAll());
        ShowCheckCommand = new RelayCommand(ShowCheckClick);
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





}
