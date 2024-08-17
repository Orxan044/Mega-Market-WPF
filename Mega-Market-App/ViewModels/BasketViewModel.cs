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

//public class BasketViewModel : BaseViewModel, INotifyPropertyChanged
//{

//    public RelayCommand IncrementCommand { get; }
//    public RelayCommand DecrementCommand { get; }
//    public RelayCommand DeleteCommand { get; }



//    private ObservableCollection<Basket2>? _basketProducts;
//    public ObservableCollection<Basket2> BasketProducts
//    {
//        get => _basketProducts ??= _basketManager.GetBasketProducts();
//        private set
//        {
//            if (_basketProducts != value)
//            {
//                _basketProducts = value;
//                OnPropertyChanged();
//            }
//        }
//    }

//    private int? _orderCount;

//    public int? OrderCount
//    {
//        get => _orderCount ??= _basketManager.GetProductsCount();
//        private set
//        {
//            if (_orderCount != value)
//            {
//                _orderCount = value;
//                OnPropertyChanged();
//            }
//        }
//    }

//    private double? _discount;

//    public double Discount
//    {
//        get => _discount ??= 0.0;
//        private set
//        {
//            if (_discount != value)
//            {
//                _discount = value;
//                OnPropertyChanged();
//            }
//        }
//    }

//    private double? _productsPrice;

//    public double? ProductsPrice
//    {
//        get => _productsPrice ??= Math.Round(_basketManager.GetOrdersTotalPayment()!, 2);
//        private set
//        {
//            if (_productsPrice != value)
//            {
//                _productsPrice = value;
//                OnPropertyChanged();
//            }
//        }
//    }

//    private double? _totalPayment;

//    public double? TotalPayment
//    {
//        get => _totalPayment ??= Math.Round(_basketManager.GetTotalPayment(Discount), 2);
//        private set
//        {
//            if (_totalPayment != value)
//            {
//                _totalPayment = value;
//                OnPropertyChanged();
//            }
//        }
//    }


//    private readonly BasketManager _basketManager;
//    public BasketViewModel(BasketManager basketManager)
//    {
//        _basketManager = basketManager;

//        Discount = 0.0;
//        _basketProducts = _basketManager.GetBasketProducts();
//        OrderCount = _basketManager.GetProductsCount();
//        TotalPayment = Math.Round(_basketManager.GetTotalPayment(Discount), 2);
//        ProductsPrice = Math.Round(_basketManager.GetOrdersTotalPayment()!, 2);


//        IncrementCommand = new RelayCommand(IncrementValue);
//        DecrementCommand = new RelayCommand(DecrementValue);
//        DeleteCommand = new RelayCommand(DeleteClick);

//    }

//    private void DeleteClick(object? obj)
//    {
//        var basketProduct = obj as Basket2; 
//        _basketManager.DeleteProduct(basketProduct!);
//        OrderCount -= basketProduct!.ProductCount;
//        notifier.ShowSuccess("Product Removed from Cart Successfully");


//        ////double roundedNumber = Math.Round(basketProduct!.Product!.Price, 2);
//        //ProductsPrice -= (basketProduct!.Product!.Price * basketProduct.ProductCount);
//        //TotalPayment -= (basketProduct!.Product!.Price * basketProduct.ProductCount);

//        ProductsPrice -= _basketManager.GetPaymentNumber(basketProduct);
//        TotalPayment -= _basketManager.GetPaymentNumber(basketProduct);


//        OnPropertyChanged(nameof(OrderCount));
//        OnPropertyChanged(nameof(ProductsPrice));
//        OnPropertyChanged(nameof(TotalPayment));
//    }

//    private void IncrementValue(object? obj)
//    {
//        var product = obj as Basket2;  
//        if (product is not null)
//        {
//            if (product.ProductCount < product.Product!.Quantity)
//            {
//                product.ProductCount++;
//                OrderCount++;

//                ////double roundedNumber = Math.Round(product.Product.Price, 2);
//                //TotalPayment += (product!.Product!.Price * product.ProductCount);
//                //ProductsPrice += (product!.Product!.Price * product.ProductCount);

//                ProductsPrice += _basketManager.GetPaymentNumber(product);
//                TotalPayment += _basketManager.GetPaymentNumber(product);

//                OnPropertyChanged();
//                OnPropertyChanged(nameof(OrderCount));
//                OnPropertyChanged(nameof(ProductsPrice));
//                OnPropertyChanged(nameof(TotalPayment));


//            }
//            else product.ProductCount = product.Product.Quantity;

//        }
//    }

//    private void DecrementValue(object? obj)
//    {
//        var product = obj as Basket2;
//        if (product is not null)
//        {
//            if (product.ProductCount > 1)
//            {
//                product.ProductCount--;
//                OrderCount--;

//                ////double roundedNumber = Math.Round(product.Product!.Price, 2);
//                //TotalPayment -= (product!.Product!.Price * product.ProductCount);
//                //ProductsPrice -= (product!.Product!.Price * product.ProductCount);

//                ProductsPrice -= _basketManager.GetPaymentNumber(product);
//                TotalPayment -= _basketManager.GetPaymentNumber(product);

//                OnPropertyChanged();
//                OnPropertyChanged(nameof(OrderCount));
//                OnPropertyChanged(nameof(ProductsPrice));
//                OnPropertyChanged(nameof(TotalPayment));
//            }
//            else product.ProductCount = 1;
//        }
//    }

//}



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
    private CreditCart? _selectedCard;
    private ObservableCollection<CreditCart>? _cardsComboBox;
    private ObservableCollection<ProductItem>? productItems = [];


    
    public ObservableCollection<CreditCart> CardsComboBox { get => _cardsComboBox!; set {  _cardsComboBox = value; OnPropertyChanged(); } }

    public CreditCart? SelectedCard
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
        CardsComboBox = _creditCartViewModel.Cards!;

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
                notifier.ShowSuccess("The products were received with great effort. Thank you for choosing us, Good luck !!!");

                // 1. Save History first
                History newHistory = new()
                {
                    Date = DateTime.Now,
                    TotalAmount = _basket!.TotalPayment,
                    User = _creditCartViewModel.NewCard.User
                };

                _historyViewModel.AddHistory(newHistory);

                CreateProductHistories(ProductItems, newHistory);

                BuyProductIncrementQuantity(ProductItems);
                _basketManager.NewBasket();
                _menyuViewModel.BasketClik(obj);
            }
            else
            {
                notifier.ShowError("Please select Credit Card !!!");
            }
        }
        else
        {
            notifier.ShowError("Basket is empty. Order Now !!!");
        }
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
        // Ensure history is saved and the newHistory.Id is assigned before creating product histories
        //_historyViewModel.Save(); // Method to save history if not already done

        foreach (var productItem in productItems)
        {
            var newHistoryProduct = new ProductHistory
            {
                ImagePath = productItem.Product!.ImagePath,
                Name = productItem.Product.Name,
                Description = productItem.Product.Description,
                Price = productItem.Product.Price,
                Count = productItem.ProductCount,
                HistoryId = newHistory.Id, // Ensure this is set correctly
                History = newHistory
            };
            _productHistoryRepository.Add(newHistoryProduct);
            newHistory.Products!.Add(newHistoryProduct); 
            _productHistoryRepository.SaveChanges();
        }
    }



}
