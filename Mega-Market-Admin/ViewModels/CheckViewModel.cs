using System.Collections.ObjectModel;
using Mega_Market_Data.Data;
using Mega_Market_Data.Models.Concretes;
using Mega_Market_Data.Repositoies;

namespace Mega_Market_Admin.ViewModels;

public class CheckViewModel : BaseViewModel
{
    private ObservableCollection<ProductHistory> _products;
    public ObservableCollection<ProductHistory> Products
    {
        get => _products;
        set
        {
            _products = value;
            OnPropertyChanged();
        }
    }

    private readonly IRepository<ProductHistory, UserDbContext> _historyRepository;

    public CheckViewModel(IRepository<ProductHistory, UserDbContext> historyRepository)
    {
        _historyRepository = historyRepository;
        Products = new ObservableCollection<ProductHistory>();
        LoadUserHistories("orxantt044@gmail.com");
    }

    private void LoadUserHistories(string userEmail)
    {
        var userHistories = _historyRepository.GetAll()
            .Where(h => h.History!.User!.Mail == userEmail);

        Products.Clear(); // Önceki verileri temizle

        foreach (var history in userHistories)
        {
            Products.Add(history);
        }
    }
}
