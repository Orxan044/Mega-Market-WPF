using Mega_Market_App.Command;
using Mega_Market_App.Services.Manager;
using Mega_Market_Data.Models.Concretes;
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
    private readonly BasketManager _basketManager;
    private Basket? _basket;
    private double? totalPayment;
    private double? productTotalPayment;
    private double? discount;
    private int? orderCount;
    private ObservableCollection<ProductItem>? productItems = [];

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

    public Basket Basket { get => _basket!; set { _basket = value; OnPropertyChanged(); } }

    public BasketViewModel(BasketManager basketManager)
    {
        _basketManager = basketManager;

        Basket = _basketManager.GetBasket();
        ProductItems = _basket!.ProductItems;

        ProductTotalPayment = _basket!.ProductTotalPayment;
        Discount = _basket.Discount;
        TotalPayment = _basket!.TotalPayment;
        OrderCount = _basket!.OrderCount;
    
        DeleteCommand = new RelayCommand(DeleteClick);
        IncrementCommand = new RelayCommand(IncrementValue!);
        DecrementCommand = new RelayCommand(DecrementValue!);

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


}
