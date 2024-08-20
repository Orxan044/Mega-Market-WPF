using Mega_Market_App.Command;
using Mega_Market_App.Views;
using Mega_Market_Data.Data;
using Mega_Market_Data.Models.Concretes;
using Mega_Market_Data.Repositoies;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Mega_Market_App.ViewModels;

public class MenyuViewModel : BaseViewModel , INotifyPropertyChanged
{
    private Page? _currentPage2;

    public Page? CurrentPage2 { get => _currentPage2; set { _currentPage2 = value; OnPropertyChanged(); } }


    public RelayCommand DashBoardCommand { get; set; }
    public RelayCommand CategoryCommand { get; set; }
    public RelayCommand ProductsCommand { get; set; }
    public RelayCommand BasketCommand { get; set; }
    public RelayCommand HistoryCommand { get; set; }
    public RelayCommand CardsCommand { get; set; }
    public RelayCommand SettingsCommand { get; set; }
    public RelayCommand CloseCommand { get; set; }

    private readonly LoginViewModel _loginViewModel;

    private string? _userName;

    public string? UserName
    {
        get => _userName; 
        set { _userName = value; OnPropertyChanged(); }
    }

    public MenyuViewModel(LoginViewModel loginViewModel)
    {
        _loginViewModel = loginViewModel;

        UserName = _loginViewModel.UserLogin.Name;

        DashBoardCommand = new RelayCommand(DashBoardClick);
        CategoryCommand = new RelayCommand(CategoryClik);
        ProductsCommand = new RelayCommand(ProductsClik);
        BasketCommand = new RelayCommand(BasketClik);
        CloseCommand = new RelayCommand(CloseClik);
        HistoryCommand = new RelayCommand(HistoryClik);
        CardsCommand = new RelayCommand(CardsClik);
        SettingsCommand = new RelayCommand(SettingsClik);

        CurrentPage2 = App.Container.GetInstance<DashBoardView>();
        CurrentPage2.DataContext = App.Container.GetInstance<DashBoradViewModel>();

        
    }

    public void DashBoardClick(object? obj)
    {
        CurrentPage2 = App.Container.GetInstance<DashBoardView>();
        CurrentPage2.DataContext = App.Container.GetInstance<DashBoradViewModel>();
    }

    public void CategoryClik(object? obj)
    {
        CurrentPage2 = App.Container.GetInstance<CategoriesView>();
        CurrentPage2.DataContext = App.Container.GetInstance<CategoriesViewModel>();
    }

    public void ProductsClik(object? obj)
    {
        CurrentPage2 = App.Container.GetInstance<ProductsView>();
        CurrentPage2.DataContext = App.Container.GetInstance<ProductsViewModel>();
    }
    public void BasketClik(object? obj)
    {
        CurrentPage2 = App.Container.GetInstance<BasketView>();
        CurrentPage2.DataContext = App.Container.GetInstance<BasketViewModel>();
    }
    public void HistoryClik(object? obj)
    {
        CurrentPage2 = App.Container.GetInstance<HistoryView>();
        CurrentPage2.DataContext = App.Container.GetInstance<HistoryViewModel>();
    }

    public void CardsClik(object? obj)
    {
        CurrentPage2 = App.Container.GetInstance<CreditCartView>();
        CurrentPage2.DataContext = App.Container.GetInstance<CreditCartViewModel>();
    }

    private void SettingsClik(object? obj)
    {
        CurrentPage2 = App.Container.GetInstance<SettingsView>();
        CurrentPage2.DataContext = App.Container.GetInstance<SettingsViewModel>();
    }

    private void CloseClik(object? obj)
    {
        _loginViewModel.UserRepository.SaveChanges();
        Application.Current.MainWindow.Close();
        Environment.Exit(0);
    }
}