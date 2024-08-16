using Mega_Market_App.Command;
using Mega_Market_App.Validations;
using Mega_Market_Data.Data;
using Mega_Market_Data.Models.Concretes;
using Mega_Market_Data.Repositoies;
using System.Collections.ObjectModel;
using ToastNotifications.Messages;

namespace Mega_Market_App.ViewModels;

public class CreditCartViewModel : BaseViewModel
{

    private readonly LoginViewModel _loginViewModel;
    private readonly UserDbContext _dbContext;
    private readonly MenyuViewModel _menyuViewModel;
    private ObservableCollection<CreditCart>? _cards = [];
    private string? cartNumberUI;
    private CreditCart? newCard;

    public ObservableCollection<CreditCart>? Cards
    {
        get => _cards;
        set { _cards = value; OnPropertyChanged(); }
    }


    public CreditCart NewCard
    {
        get => newCard!; 
        set { newCard = value; OnPropertyChanged(); }
    }

    public string CartNumberUI
    {
        get => cartNumberUI!;
        set { cartNumberUI = value; OnPropertyChanged(); }
    }


    public RelayCommand AddCardCommand { get; set; }
    public RelayCommand RemoveCardCommand { get; set; }
    public RelayCommand CardTypeCommand { get; }


    public CreditCartViewModel(UserDbContext dbContext, LoginViewModel loginViewModel,MenyuViewModel menyuViewModel)
    {
        _dbContext = dbContext;
        _menyuViewModel = menyuViewModel;
        _loginViewModel = loginViewModel;
        Cards = _loginViewModel.UserLogin.CreditCarts;
        NewCard = new();


        AddCardCommand = new RelayCommand(AddCardClick);
        RemoveCardCommand = new RelayCommand(RemoveCardClick);
        CardTypeCommand = new RelayCommand(CardTypeClick);

    }

    private void RemoveCardClick(object? obj)
    {
        var card = obj as CreditCart;
        if (card is not null)
        {
            _loginViewModel.UserLogin.CreditCarts!.Remove(card);
            _dbContext.CreditCarts.Remove(card);
            _dbContext.SaveChanges();
        }
    }

    private void AddCardClick(object? obj)
    {
        if (NewCard.IconName is not null)
        {
            if (Validation.IsCardNumber(NewCard.Code) && Validation.IsCartYear(NewCard.EndYear) &&
                Validation.IsCardCVV(NewCard.CVV))
            {
                NewCard.Code = string.Join(" - ", Enumerable.Range(0, NewCard.Code!.Length / 4)
                                       .Select(i => NewCard.Code!.Substring(i * 4, 4)));

                _loginViewModel.UserLogin.CreditCarts!.Add(NewCard);
                _dbContext.CreditCarts.Add(NewCard);
                _dbContext.SaveChanges();
                
                notifier.ShowSuccess("New Cart Added Successfully !!!");
                NewCard = new();
                _menyuViewModel.CardsClik(obj);
            }
            else notifier.ShowError("Enter the information correctly");
        }
        else notifier.ShowError("Cart type not selected. Please Select It !!!");
    }

    private void CardTypeClick(object? obj)
    {
        if (obj is string cardType) NewCard.IconName = cardType;
    }
}
