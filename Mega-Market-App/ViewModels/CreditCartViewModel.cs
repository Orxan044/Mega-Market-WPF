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
    private readonly MenyuViewModel _menyuViewModel;
    private readonly LoginViewModel _loginViewModel;
    private ObservableCollection<CreditCart>? _cards = [];
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


    public RelayCommand AddCardCommand { get; set; }
    public RelayCommand RemoveCardCommand { get; set; }
    public RelayCommand CardTypeCommand { get; }

    private readonly IRepository<CreditCart, UserDbContext> _cartRepository;


    public CreditCartViewModel(IRepository<CreditCart,UserDbContext> cartRepository, LoginViewModel loginViewModel,MenyuViewModel menyuViewModel)
    {
        _cartRepository = cartRepository;
        _menyuViewModel = menyuViewModel;
        _loginViewModel = loginViewModel;

        Cards = new ObservableCollection<CreditCart>(_cartRepository.GetAll());

        NewCard = new();


        GetUserInNewCard();
        AddCardCommand = new RelayCommand(AddCardClick);
        RemoveCardCommand = new RelayCommand(RemoveCardClick);
        CardTypeCommand = new RelayCommand(CardTypeClick);

    }

    private void RemoveCardClick(object? obj)
    {

        var card = obj as CreditCart;
        if (card is not null)
        {
            _cartRepository.Delete(card);
            _cartRepository.SaveChanges();
            _menyuViewModel.CardsClik(obj);
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

                GetUserInNewCard();
                _cartRepository.Add(NewCard);
                _cartRepository.SaveChanges();
                
                notifier.ShowSuccess("New Cart Added Successfully !!!");
                NewCard = new();
                _menyuViewModel.CardsClik(obj);
            }
            else notifier.ShowError("Enter the information correctly");
        }
        else notifier.ShowError("Cart type not selected. Please Select It !!!");
    }

    private void GetUserInNewCard()
    {
        foreach (var user  in _loginViewModel.UserRepository.GetAll())
        {
            if (_loginViewModel.UserLogin.Mail == user.Mail) NewCard.User = user;
        }
    }

    private void CardTypeClick(object? obj)
    {
        if (obj is string cardType) NewCard.IconName = cardType;
    }
}
