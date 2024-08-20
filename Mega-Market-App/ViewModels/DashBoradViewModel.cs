using Mega_Market_Data.Models.Concretes;
using System.Collections.ObjectModel;

namespace Mega_Market_App.ViewModels;

public class DashBoradViewModel : BaseViewModel
{

    private double? totalShoping = 0;
    public double? TotalShoping
    {
        get => totalShoping!;
        set
        {
            totalShoping = value;
            OnPropertyChanged();
        }
    }

    private int? totalCard;
    public int? TotalCard
    {
        get => totalCard!;
        set
        {
            totalCard = value;
            OnPropertyChanged();
        }
    }

    private double? bonusBalance = 0;
    public double? BonusBalance
    {
        get => bonusBalance!;
        set
        {
            bonusBalance = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<Category>? orders;
    public ObservableCollection<Category>? Orders
    {
        get => new ObservableCollection<Category>(orders?.Take(3) ?? Enumerable.Empty<Category>());
        set
        {
            orders = value;
            OnPropertyChanged();
        }
    }

    private readonly LoginViewModel _loginViewModel;
    private readonly CategoriesViewModel _categoriesViewModel;
    private User _user = new();

    public DashBoradViewModel(LoginViewModel loginViewModel, CategoriesViewModel categoriesViewModel)
    {
        _loginViewModel = loginViewModel;
        _categoriesViewModel = categoriesViewModel;

        UserNew();

        InputValue();

    }



    private void InputValue()
    {
        TotalShoping = 0;
        TotalCard = 0;
        BonusBalance = 0;
 

        // Total Shoping
        foreach (var history in _user.Histories!)
            TotalShoping += history.TotalAmount;

        // All Cards

        TotalCard = _user.CreditCarts!.Count;

        // Bonus Balance

        BonusBalance = _user.BonusBalance;

        // Orders
        
        Orders = new ObservableCollection<Category>(_categoriesViewModel.Categories);
    }

    private void UserNew()
    {
        foreach (var user in _loginViewModel.UserRepository.GetAll())
            if (_loginViewModel.UserLogin.Mail == user.Mail) _user = user;
    }

}
