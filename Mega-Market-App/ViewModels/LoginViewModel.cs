﻿using Mega_Market_App.Command;
using Mega_Market_App.Services.Navigate;
using Mega_Market_App.Validations;
using Mega_Market_App.Views;
using Mega_Market_Data.Data;
using Mega_Market_Data.Models.Concretes;
using Mega_Market_Data.Repositoies;
using System.Windows;
using ToastNotifications.Messages;

namespace Mega_Market_App.ViewModels;


public class LoginViewModel : BaseViewModel
{
    private User? _userLogin;

    public User UserLogin
    {
        get => _userLogin!;
        set { _userLogin = value; OnPropertyChanged(); }
    }


    public RelayCommand CloseCommand { get; set; }
    public RelayCommand SignInCommand { get; set; }
    public RelayCommand SignUpCommand { get; set; }

    private readonly INavigationServices _navigationServices;
    private IRepository<User, UserDbContext> _userRepository;


    public LoginViewModel(INavigationServices navigationServices, IRepository<User, UserDbContext> userRepository)
    {
        _navigationServices = navigationServices;
        _userRepository = userRepository;

        UserLogin = new();

        CloseCommand = new RelayCommand(CloseClik);
        SignInCommand = new RelayCommand(SignInClik);
        SignUpCommand = new RelayCommand(SignUpClik);
    }

    private void SignInClik(object? obj)
    {
        if (Validation.IsMail(UserLogin.Mail) && Validation.IsPassword(UserLogin.Password))
        {
            var Users = _userRepository.GetAll();
            foreach (var user in Users)
            {
                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(UserLogin.Password, user.Password);
                if (user.Mail == UserLogin.Mail && isPasswordValid)
                {
                    notifier.ShowSuccess("You Are Logged In Correctly");
                    _navigationServices.Navigate<MenyuView, MenyuViewModel>();
                }
            }
        }
    }

    private void SignUpClik(object? obj)
    {
        _navigationServices.Navigate<RegistherView, RegistherViewModel>();
    }

    private void CloseClik(object? obj)
    {
        Application.Current.MainWindow.Close();
        Environment.Exit(0);
    }
}

