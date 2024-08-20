using MaterialDesignThemes.Wpf;
using Mega_Market_App.Command;
using Mega_Market_App.Services.Manager;
using Mega_Market_Data.Data;
using Mega_Market_Data.Models.Concretes;
using Mega_Market_Data.Repositoies;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ToastNotifications.Messages;

namespace Mega_Market_App.ViewModels;

public class BasketViewModel : BaseViewModel, INotifyPropertyChanged
{
    private readonly CreditCartViewModel _creditCartViewModel;
    private readonly MenyuViewModel _menyuViewModel;
    private readonly BasketManager _basketManager;
    private Basket? _basket;
    private double? totalPayment;
    private double? productTotalPayment;
    private double? discount;
    private int? orderCount;
    private string? _selectedCard;
    private ObservableCollection<string>? _cardsComboBox = [];
    private ObservableCollection<ProductItem>? productItems = [];


    
    public ObservableCollection<string> CardsComboBox { get => _cardsComboBox!; set {  _cardsComboBox = value; OnPropertyChanged(); } }

    public string? SelectedCard
    {
        get => _selectedCard;
        set
        {
            _selectedCard = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<ProductItem>? ProductItems
    {
        get => productItems;
        set { productItems = value; OnPropertyChanged(); }
    }

    public int? OrderCount
    {
        get => orderCount;
        set { orderCount = value; OnPropertyChanged(); }
    }


    public double? TotalPayment
    {
        get => totalPayment;
        set { totalPayment = value; OnPropertyChanged(); }
    }

    public double? ProductTotalPayment
    {
        get => productTotalPayment;
        set { productTotalPayment = value; OnPropertyChanged(); }
    }

    public double? Discount
    {
        get => discount;
        set { discount = value; OnPropertyChanged(); }
    }

    public RelayCommand IncrementCommand { get; }
    public RelayCommand DecrementCommand { get; }
    public RelayCommand DeleteCommand { get; set; }
    public RelayCommand GetCardsCommand { get; set; }
    public RelayCommand BuyProductsCommand { get; set; }

    public Basket Basket { get => _basket!; set { _basket = value; OnPropertyChanged(); } }

    private readonly IRepository<Product, MarketDbContext> _productRepository;
    private readonly IRepository<ProductHistory, UserDbContext> _productHistoryRepository;
    private readonly HistoryViewModel _historyViewModel;
    public BasketViewModel(IRepository<Product,MarketDbContext> productRepository, IRepository<ProductHistory, UserDbContext> productHistoryRepository,HistoryViewModel historyViewModel,MenyuViewModel menyuViewModel,CreditCartViewModel creditCartViewModel,BasketManager basketManager)
    {
        _basketManager = basketManager;
        _creditCartViewModel = creditCartViewModel;
        _menyuViewModel = menyuViewModel;
        _productRepository = productRepository;
        _historyViewModel = historyViewModel;
        _productHistoryRepository = productHistoryRepository;

        Basket = _basketManager.Basket;
        ProductItems = _basket!.ProductItems;

        //Adding ComboBox
        foreach (var item in _creditCartViewModel.Cards!) CardsComboBox.Add(item.ToString());
        CardsComboBox.Add("*Bonus Card");


        ProductTotalPayment = _basket!.ProductTotalPayment;
        Discount = _basket.Discount;
        TotalPayment = _basket!.TotalPayment;
        OrderCount = _basket!.OrderCount;
    
        DeleteCommand = new RelayCommand(DeleteClick);
        IncrementCommand = new RelayCommand(IncrementValue!);
        DecrementCommand = new RelayCommand(DecrementValue!);
        GetCardsCommand = new RelayCommand(GetCardsClick);
        BuyProductsCommand = new RelayCommand(BuyProductsClick);

    }

    private void BuyProductsClick(object? obj)
    {
        if (ProductItems is not null && _basket!.TotalPayment > 0)
        {
            if (SelectedCard is not null)
            {
                var bonusBalanceToTotal = _creditCartViewModel.NewCard.User!.BonusBalance >= _basket.TotalPayment;
                
                if (SelectedCard is "*Bonus Card" )
                {
                    if (bonusBalanceToTotal) { Payment(); _menyuViewModel.BasketClik(obj); }
                    else notifier.ShowError("There is not enough money on the Bonus Card !!!");
                }
                else 
                {
                    Payment();
                    _menyuViewModel.BasketClik(obj);
                }
            }
            else notifier.ShowError("Please select Credit Card !!!");
            
        }
        else notifier.ShowError("Basket is empty. Order Now !!!"); 
    }

    private void Payment()
    {
        notifier.ShowSuccess("The products were received with great effort. Thank you for choosing us, Good luck !!!");

        History newHistory = new()
        {
            Date = DateTime.Now,
            TotalAmount = _basket!.TotalPayment,
            User = _creditCartViewModel.NewCard.User,
            PayMethod = SelectedCard
        };

        double? bonusCal = (_basket.TotalPayment * 2 ) / 100;
        _creditCartViewModel.NewCard.User!.BonusBalance += bonusCal;


        _historyViewModel.AddHistory(newHistory);

        CreateProductHistories(ProductItems!, newHistory);

        BuyProductIncrementQuantity(ProductItems!);
        _basketManager.NewBasket();
    }


    private void GetCardsClick(object? obj)
    {
        _menyuViewModel.CardsClik(obj); 
    }

    private void IncrementValue(object obj)
    {
        if (obj is ProductItem productItem)
        {
            _basketManager.IncrementProduct(productItem);
            UpdateTotals();
        }
    }

    private void DecrementValue(object obj)
    {
        if (obj is ProductItem productItem)
        {
            _basketManager.DecrementProduct(productItem);
            UpdateTotals();
        }
    }


    private void DeleteClick(object? obj)
    {
        var productItem = obj as ProductItem;
        _basketManager.DeleteProduct(productItem!);
        UpdateTotals();
    }

    private void UpdateTotals()
    {
        ProductTotalPayment = _basket!.ProductTotalPayment ?? 0.0;
        TotalPayment = _basket!.TotalPayment ?? 0.0;
        OnPropertyChanged(nameof(ProductItems));
    }

    private void BuyProductIncrementQuantity(ObservableCollection<ProductItem> ProductItems)
    {
        foreach (var productItem in ProductItems)
        {

            var product = _productRepository.Get(productItem.Product!.Id);
            if (product is not null)
            {
                product.Quantity -= productItem.ProductCount;
                _productRepository.Update(product);
            }
        }
        _productRepository.SaveChanges();
    }

    private void CreateProductHistories(ObservableCollection<ProductItem> productItems, History newHistory)
    {

        foreach (var productItem in productItems)
        {
            var newHistoryProduct = new ProductHistory
            {
                ImagePath = productItem.Product!.ImagePath,
                Name = productItem.Product.Name,
                Description = productItem.Product.Description,
                Price = productItem.Product.Price,
                Count = productItem.ProductCount,
                HistoryId = newHistory.Id, 
                History = newHistory
            };
            _productHistoryRepository.Add(newHistoryProduct);
            newHistory.Products!.Add(newHistoryProduct); 
            _productHistoryRepository.SaveChanges();
        }
    }



}
