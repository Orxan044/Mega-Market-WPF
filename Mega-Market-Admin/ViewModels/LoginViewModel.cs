using Mega_Market_Admin.Command;
using Mega_Market_Admin.Services;
using Mega_Market_Admin.Views;
using Mega_Market_Data.Data;
using Mega_Market_Data.Models.Concretes;
using Mega_Market_Data.Repositoies;
using System.Windows;
using ToastNotifications.Messages;

namespace Mega_Market_Admin.ViewModels;

public class LoginViewModel : BaseViewModel
{
    private readonly INavigationService _navigationService;
    private readonly IRepository<Admin, AdminDbContext> _adminRepository;

    public RelayCommand CloseCommand { get; set; }
    public RelayCommand SignInCommand { get; set; }
    public Admin AdminLogin { get; set; } = new();
    public LoginViewModel(IRepository<Admin, AdminDbContext> adminRepository, INavigationService navigationService)
    {
        _adminRepository = adminRepository;
        _navigationService = navigationService;

        CloseCommand = new RelayCommand(CloseClik);
        SignInCommand = new RelayCommand(SignInClick);
    }




    private void SignInClick(object? obj)
    {
        AdminLogin.AccountName = "Orxan";
        AdminLogin.AccountPassword = "OrxanAdmin";

        var Admins = _adminRepository.GetAll();
        bool checking = true;
        foreach (var admin in Admins)
        {
            if (AdminLogin.AccountName == admin.AccountName
               && AdminLogin.AccountPassword == admin.AccountPassword)
            {
                AdminLogin = admin;
                notifier.ShowSuccess("You Are Logged In Correctly");
                checking = false;
                MainViewModel mainVm = new();
                _navigationService.Navigate<MenyuView, MenyuViewModel>(mainVm.CurrentPage);
            }
        }

        if (checking) notifier.ShowError("Please Login Correctly !!!");
    }

    private void CloseClik(object? obj)
    {
        Application.Current.MainWindow.Close();
        Environment.Exit(0);
    }


}
