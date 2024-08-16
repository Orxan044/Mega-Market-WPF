using Mega_Market_Admin.Command;
using Mega_Market_Data.Data;
using Mega_Market_Data.Models.Concretes;
using Mega_Market_Data.Repositoies;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using ToastNotifications.Messages;

namespace Mega_Market_Admin.ViewModels;

public class CategoryViewModel : BaseViewModel
{
    private string? _searchText;
    private ICollectionView _categoriesView;

    public string SearchText
    {
        get => _searchText!;
        set
        {
            _searchText = value;
            OnPropertyChanged();
            _categoriesView.Refresh();
        }
    }

    private Category? _selectedCategory;
    public Category SelectedCategory { get => _selectedCategory!; set { _selectedCategory = value; OnPropertyChanged(); } }

    private ObservableCollection<Category>? _categories;
    private MenyuViewModel _viewModel;

    public ObservableCollection<Category> Categories { get => _categories!; set { _categories = value; OnPropertyChanged(); } }

    private readonly IRepository<Category, MarketDbContext> _categoryRepository;

    public RelayCommand ShowCategoryCommand { get; set; }
    public RelayCommand DeleteCategoryCommand { get; set; }
    public RelayCommand AddCategoryCommand { get; set; }

    public CategoryViewModel(IRepository<Category, MarketDbContext> categoryRepository, MenyuViewModel viewModel)
    {

        _categoryRepository = categoryRepository;
        _viewModel = viewModel;

        Categories = new ObservableCollection<Category>(_categoryRepository.GetAll());

        ShowCategoryCommand = new RelayCommand(ShowCategoryClick);
        DeleteCategoryCommand = new RelayCommand(DeleteCategoryClick);
        AddCategoryCommand = new RelayCommand(AddCategoryClick);

        _categoriesView = CollectionViewSource.GetDefaultView(Categories);
        _categoriesView.Filter = FilterCategories;
    }

    private void AddCategoryClick(object? obj)
    {
        _viewModel.AddCategoryClick(obj);
    }

    private void DeleteCategoryClick(object? id)
    {
        if (SelectedCategory is not null)
        {
            _categoryRepository.Delete(SelectedCategory);
            _categoryRepository.SaveChanges();
            _viewModel.CategoriesClick(id);
            notifier.ShowSuccess("The Category Has Been Removed Successfully");
        }
        else notifier.ShowWarning("Please Selected Category !!!");
    }

    private void ShowCategoryClick(object? id)
    {
        SelectedCategory = _categoryRepository.Get(Convert.ToInt32(id))!;
    }

    private bool FilterCategories(object obj)
    {
        if (obj is Category category)
        {
            return string.IsNullOrEmpty(SearchText) || category.Name!.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
            category.Id.ToString()!.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
        }
        return false;
    }
}

