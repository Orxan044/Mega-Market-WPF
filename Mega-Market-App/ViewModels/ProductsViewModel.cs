using Mega_Market_App.Command;
using Mega_Market_App.Services.Manager;
using Mega_Market_Data.Data;
using Mega_Market_Data.Models.Concretes;
using Mega_Market_Data.Repositoies;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using ToastNotifications.Messages;

namespace Mega_Market_App.ViewModels;

public class ProductsViewModel : BaseViewModel , INotifyPropertyChanged
{


    //public ICollectionView _categoriesView { get; set; }

    //private string? _searchText;
    //public string SearchText
    //{
    //    get => _searchText!;
    //    set
    //    {
    //        _searchText = value;
    //        OnPropertyChanged();
    //        _categoriesView.Refresh();
    //    }
    //}

    //private double? _min;
    //public double? MinPrice
    //{
    //    get => _min!;
    //    set
    //    {
    //        if (value <= GetMaxProductPrice()) 
    //        {
    //            _min = value;
    //            OnPropertyChanged();

    //        }
    //        else
    //        {
    //            _min = GetMaxProductPrice();
    //        }
    //    }
    //}

    //private double? _max;
    //public double? MaxPrice
    //{
    //    get => GetMaxProductPrice();
    //    set
    //    {
    //        if (value >= GetMinProductPrice())
    //        {
    //            _max = value;
    //            OnPropertyChanged();
    //        }
    //        else
    //        {
    //            _max = GetMinProductPrice();
    //        }
    //    }
    //}

    //private bool _showSpecialOnly;
    //public bool ShowSpecialOnly
    //{
    //    get => _showSpecialOnly;
    //    set { _showSpecialOnly = value; OnPropertyChanged(); _categoriesView.Refresh(); }
    //}

    //private bool _isFilterExpanded;
    //public bool IsFilterExpanded
    //{
    //    get => _isFilterExpanded;
    //    set
    //    {
    //        if (_isFilterExpanded != value)
    //        {
    //            _isFilterExpanded = value;
    //            OnPropertyChanged(nameof(_isFilterExpanded));

    //            if (_isFilterExpanded!)
    //            {

    //            }
    //            //else
    //            //{
    //            //    // Expander kapandığında yapılacak işlemler
    //            //}
    //        }
    //    }
    //}


    //public RelayCommand MinMaxOkCommand { get; set; }


    public RelayCommand AddBasketCommand {get; set;}


    private Category? _selectedCategory;
    public Category SelectedCategory
    {
        get => _selectedCategory!;
        set { _selectedCategory = value; OnPropertyChanged(); }
    }

    private readonly BasketManager _basketManager;
    private ObservableCollection<Product>? products;
    public ObservableCollection<Product> Products { get => products!; set { products = value; OnPropertyChanged(); } }

    private readonly IRepository<Product, MarketDbContext> _productRepository;
    private readonly MenyuViewModel _menyuViewModel;
    public ProductsViewModel(IRepository<Product, MarketDbContext> productRepository,MenyuViewModel menyuViewModel,BasketManager basketManager)
    {
        Products = [];
        _basketManager = basketManager;
        _productRepository = productRepository;
        _menyuViewModel = menyuViewModel;
        //_basketViewModel = basketViewModel;
        
        AddBasketCommand = new RelayCommand(AddBasketClick);
        //MinMaxOkCommand = new RelayCommand(MinMaxOkClick);
    }


    public void AddProducts()
    {
        Products.Clear();
        if (SelectedCategory is not null)
        {
            foreach (var product in _productRepository.GetAll())
                if (product.Category == SelectedCategory && product.Quantity > 0)
                    Products.Add(product);
        }
    }




    private void AddBasketClick(object? obj)
    {
        var product = _productRepository.Get(Convert.ToInt32(obj));

        if (product is not null)
        {
            _basketManager.AddProduct(product);
            notifier.ShowSuccess("Product successfully added to cart");
        }
    }

    //private void MinMaxOkClick(object? obj)
    //{
    //    ObservableCollection<Product>? _saveProducts = Products;

    //    try
    //    {
    //        var ProductsFilter = new ObservableCollection<Product>();

    //        if (IsFilterExpanded)
    //        {
    //            if (MaxPrice > MinPrice && MaxPrice is not null && MinPrice is not null)
    //            {
    //                foreach (var product in Products)
    //                {
    //                    if (product.Price >= MinPrice && product.Price <= MaxPrice)
    //                    {
    //                        ProductsFilter.Add(product);
    //                    }
    //                }
    //            }
    //        }
    //        else
    //        {
    //            ProductsFilter = new ObservableCollection<Product>(Products);
    //        }

    //        Products = ProductsFilter;
    //    }
    //    catch (Exception)
    //    {

    //    }

    //}
    //private double GetMinProductPrice()
    //{
    //    return (double)(Products.Any() ? Products.Min(p => p.Price) : 0)!;
    //}

    //private double GetMaxProductPrice()
    //{
    //    return (double)(Products.Any() ? Products.Max(p => p.Price) : double.MaxValue)!;
    //}
}
