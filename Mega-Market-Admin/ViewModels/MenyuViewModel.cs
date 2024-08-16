using Mega_Market_Admin.Command;
using Mega_Market_Admin.Views;
using Mega_Market_Data.Data;
using Mega_Market_Data.Models.Concretes;
using Mega_Market_Data.Repositoies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Mega_Market_Admin.ViewModels;

public class MenyuViewModel : BaseViewModel
{
    private Page? _currentPage2;

    public Page? CurrentPage2 { get => _currentPage2; set { _currentPage2 = value; OnPropertyChanged(); } }

    private readonly IRepository<Category, MarketDbContext> _categoryRepository;

    #region RelayCommand
    public RelayCommand DashBoardCommand { get; set; }
    public RelayCommand CategoriesCommand { get; set; }
    public RelayCommand ProductsCommand { get; set; }
    public RelayCommand ExitCommand { get; set; }
    #endregion



    public MenyuViewModel(IRepository<Category, MarketDbContext> categoryRepository)
    {
        _categoryRepository = categoryRepository;

        #region RelayCommand
        DashBoardCommand = new RelayCommand(DashBoardClick);
        CategoriesCommand = new RelayCommand(CategoriesClick);
        ProductsCommand = new RelayCommand(ProductsClick);
        ExitCommand = new RelayCommand(ExitClick);
        #endregion

        CurrentPage2 = App.Container.GetInstance<DashBoardView>();
        CurrentPage2.DataContext = App.Container.GetInstance<DashBoardViewModel>();
    }
    private void DashBoardClick(object? obj)
    {
        CurrentPage2 = App.Container.GetInstance<DashBoardView>();
        CurrentPage2.DataContext = App.Container.GetInstance<DashBoardViewModel>();
    }

    public void CategoriesClick(object? obj)
    {
        CurrentPage2 = App.Container.GetInstance<CategoryView>();
        CurrentPage2.DataContext = App.Container.GetInstance<CategoryViewModel>();
    }

    public void AddCategoryClick(object? obj)
    {
        CurrentPage2 = App.Container.GetInstance<AddCategoryView>();
        CurrentPage2.DataContext = App.Container.GetInstance<AddCategoryViewModel>();
    }

    public void ShowProductClick(object? obj)
    {
        CurrentPage2 = App.Container.GetInstance<ProductShowView>();
        CurrentPage2.DataContext = App.Container.GetInstance<ProductShowViewModel>();
    }

    public void ProductsClick(object? obj)
    {
        CurrentPage2 = App.Container.GetInstance<ProductsView>();
        CurrentPage2.DataContext = App.Container.GetInstance<ProductsViewModel>();
    }

    public void AddProductsClick(object? obj)
    {
        CurrentPage2 = App.Container.GetInstance<AddProductView>();
        CurrentPage2.DataContext = App.Container.GetInstance<AddProductViewModel>();
    }

    private void ExitClick(object? obj)
    {
        _categoryRepository.SaveChanges();
        Application.Current.MainWindow.Close();
        Environment.Exit(0);
    }


}

