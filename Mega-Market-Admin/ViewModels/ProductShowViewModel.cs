using Mega_Market_Admin.Command;
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
using ToastNotifications.Messages;

namespace Mega_Market_Admin.ViewModels;

public class ProductShowViewModel : BaseViewModel, INotifyPropertyChanged
{
    public object? Id { get; set; }
    private Product? _selectedProduct;
    private Category? selectedCategory;
    private MenyuViewModel _viewModel;
    private IRepository<Product, MarketDbContext> _productRepository;

    public Product SelectedProduct { get => _selectedProduct!; set { _selectedProduct = value; OnPropertyChanged(); } }
    public Category SelectedCategory { get => selectedCategory!; set { selectedCategory = value; OnPropertyChanged(); } }


    public ObservableCollection<Category> Categories { get; set; }

    public RelayCommand BackCommand { get; set; }
    public RelayCommand UpdateCommand { get; set; }
    public RelayCommand DeleteCommand { get; set; }


    public ProductShowViewModel(CategoryViewModel categoryVM, MenyuViewModel viewModel, IRepository<Product, MarketDbContext> productRepository)
    {
        _productRepository = productRepository;
        _viewModel = viewModel;
        Categories = categoryVM.Categories;
        BackCommand = new RelayCommand(BackClick);
        UpdateCommand = new RelayCommand(UpdateClick);
        DeleteCommand = new RelayCommand(DeleteClick);
    }

    private void DeleteClick(object? obj)
    {
        try
        {
            var product = _productRepository.Get(Convert.ToInt32(Id));
            _productRepository.Delete(product!);
            _productRepository.SaveChanges();
            notifier.ShowSuccess("The Product Has Been Remove Successfully !!!");
            _viewModel.ProductsClick(obj);
        }
        catch (Exception)
        {
            notifier.ShowSuccess("The Not Found Product !!!");
        }

    }

    private void UpdateClick(object? obj)
    {

        try
        {
            if (_selectedProduct is null)
            {
                notifier.ShowError("No product selected to update.");
                return;
            }

            var existingProduct = _productRepository.Get(Convert.ToInt32(Id));
            if (existingProduct is null)
            {
                notifier.ShowError("Product not found in the database.");
                return;
            }
            if (_selectedProduct.Quantity < 0 && _selectedProduct.Price < 0)
            {
                notifier.ShowError("Negative Number Cannot Be Entered");
                return;
            }
            existingProduct.ImagePath = _selectedProduct.ImagePath;
            existingProduct.Name = _selectedProduct.Name;
            existingProduct.Description = _selectedProduct.Description;
            existingProduct.Quantity = _selectedProduct.Quantity;
            existingProduct.Price = _selectedProduct.Price;
            existingProduct.IsSpecial = _selectedProduct.IsSpecial;
            existingProduct.CategoryId = _selectedProduct.CategoryId;
            existingProduct.Category = _selectedProduct.Category;
            notifier.ShowSuccess("The product has been updated successfully!");
            _viewModel.ProductsClick(obj);
        }
        catch (Exception)
        {
            notifier.ShowWarning("The Elements Are All The Same !!!");
        }
    }

    private void BackClick(object? obj)
    {
        _viewModel.ProductsClick(obj);
    }
}