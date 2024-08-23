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

    public RelayCommand AddBasketCommand {get; set;}
    public RelayCommand ProductsCommand {get; set;}
    public RelayCommand MinMaxOkCommand { get; set; }

    private Category? _selectedCategory;
    public Category SelectedCategory
    {
        get => _selectedCategory!;
        set { _selectedCategory = value; OnPropertyChanged(); AddProducts(); }
    }

    private string? _searchText;
    private ICollectionView _productsView;

    public string SearchText
    {
        get => _searchText!;
        set
        {
            _searchText = value;
            OnPropertyChanged();
            _productsView.Refresh();
        }
    }

    private double? _minPrice;
    public double? MinPrice
    {
        get => _minPrice;
        set
        {
            if (value < 0)
                _minPrice = 0;
            else if (value > MaxPrice)
                _minPrice = MaxPrice; // MinPrice, MaxPrice'dan büyük olamaz
            else
                _minPrice = value;

            OnPropertyChanged();
        }
    }

    private double? _maxPrice;
    public double? MaxPrice
    {
        get => _maxPrice;
        set
        {
            var maxProductPrice = Products.Any() ? Products.Max(p => p.Price) : 0;
            if (value > maxProductPrice)
                _maxPrice = maxProductPrice;
            else
                _maxPrice = value;
            OnPropertyChanged();
        }
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
        
        AddBasketCommand = new RelayCommand(AddBasketClick);
        ProductsCommand = new RelayCommand(ProductsClick);
        MinMaxOkCommand = new RelayCommand(FilterPriceProducts);

        _productsView = CollectionViewSource.GetDefaultView(Products);
        _productsView.Filter = FilterCategories;
    }

    private void ProductsClick(object? obj)
    {
        AddProducts();
    }

    private void FilterPriceProducts(object? obj)
    {
        Products.Clear();
        if (MinPrice is not null && MaxPrice is not null && MinPrice < MaxPrice)
        {
            foreach (var product in _productRepository.GetAll())
            {
                if(product.Price >= MinPrice) Products.Add(product);
            }
        }
        else notifier.ShowError("Filter Information Not Correctly !!! ");
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

    private bool FilterCategories(object obj)
    {
        if (obj is Product product)
        {
            return string.IsNullOrEmpty(SearchText) || product.Name!.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
            product.Id.ToString()!.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
        }

        return false;
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

}
