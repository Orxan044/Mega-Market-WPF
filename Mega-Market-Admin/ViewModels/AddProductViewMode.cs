using Mega_Market_Admin.Command;
using Mega_Market_Admin.Validations;
using Mega_Market_Data.Data;
using Mega_Market_Data.Models.Concretes;
using Mega_Market_Data.Repositoies;
using System.Collections.ObjectModel;
using ToastNotifications.Messages;

namespace Mega_Market_Admin.ViewModels;

public class AddProductViewModel : BaseViewModel
{
    MenyuViewModel _viewModel;
    private readonly IRepository<Product, MarketDbContext> _productRepository;
    private Product? _newProduct;

    public Product NewProduct
    {
        get => _newProduct!;
        set { _newProduct = value; OnPropertyChanged(); }
    }

    private Category? selectedCategory;
    public Category SelectedCategory { get => selectedCategory!; set { selectedCategory = value; OnPropertyChanged(); } }


    public ObservableCollection<Category> Categories { get; set; }

    public RelayCommand BackCommand { get; set; }
    public RelayCommand AddCommand { get; set; }

    public AddProductViewModel(CategoryViewModel categoryVM, IRepository<Product, MarketDbContext> productRepository, MenyuViewModel viewModel)
    {
        _viewModel = viewModel;
        _productRepository = productRepository;
        NewProduct = new();

        Categories = categoryVM.Categories;

        BackCommand = new RelayCommand(BackClick);
        AddCommand = new RelayCommand(AddClick);


    }

    private void AddClick(object? obj)
    {
        ProductValidation validation = new(NewProduct);

        try
        {
            _newProduct!.Category = SelectedCategory;
            if (validation.IsNull() && validation.IsNegative())
            {
                _productRepository.Add(_newProduct);
                _productRepository.SaveChanges();
                notifier.ShowSuccess("The Category Has Been Adding Successfully");
                _viewModel.ProductsClick(obj);
            }
        }
        catch (Exception)
        {
            notifier.ShowError("The Category Has Been Not Adding");
        }

    }

    private void BackClick(object? obj)
    {
        _viewModel.CategoriesClick(obj);
    }




}

