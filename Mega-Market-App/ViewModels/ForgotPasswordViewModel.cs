using Mega_Market_App.Command;
using Mega_Market_App.Services.Mail;
using Mega_Market_App.Services.Navigate;
using Mega_Market_App.Validations;
using Mega_Market_App.Views;
using Mega_Market_Data.Data;
using Mega_Market_Data.Models.Concretes;
using Mega_Market_Data.Repositoies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ToastNotifications.Messages;

namespace Mega_Market_App.ViewModels;

public class ForgotPasswordViewModel : BaseViewModel
{

	private string? _mail;

	public string?  Mail
	{
		get => _mail; 
		set { _mail = value; OnPropertyChanged(); }
	}

    private string? _newPassword;

    public string? NewPassword
    {
        get => _newPassword;
        set { _newPassword = value; OnPropertyChanged(); }
    }

    private string? _confrimPassword;

    public string? ConfrimPassword
    {
        get => _confrimPassword;
        set { _confrimPassword = value; OnPropertyChanged(); }
    }

    private string? _sendCodeSms;

    public string? SendCodeSms
    {
        get => _sendCodeSms;
        set { _sendCodeSms = value; OnPropertyChanged(); }
    }


    public RelayCommand SigInCommand {  get; set; }
    public RelayCommand SendMailCodeCommand {  get; set; }
    public RelayCommand ChangePasswordCommand {  get; set; }
    public RelayCommand CloseCommand {  get; set; }

    private int _randomCode;
    private readonly IRepository<User, UserDbContext> _userRepository;
    private readonly INavigationServices _navigationService;

    public ForgotPasswordViewModel(IRepository<User,UserDbContext> userRepository,INavigationServices navigationServices)
    {
        _userRepository = userRepository;
        _navigationService = navigationServices;

        SigInCommand = new RelayCommand(SigInClick);
        SendMailCodeCommand = new RelayCommand(SendMailClick);
        ChangePasswordCommand = new RelayCommand(ChangePasswordClick);
        CloseCommand = new RelayCommand(CloseClick);
    }

    private void ChangePasswordClick(object? obj)
    {

        if (Validation.IsMail(Mail) && Validation.IsPassword(NewPassword) && Validation.IsPassword(ConfrimPassword)
            && NewPassword == ConfrimPassword)
        {
            var user = _userRepository.GetAll().FirstOrDefault(user => user.Mail == Mail);

            if (user != null) 
            {
                if (SendCodeSms is not null && Convert.ToInt32(SendCodeSms) == _randomCode)
                {
                    string passwordHash = BCrypt.Net.BCrypt.HashPassword(NewPassword);
                    NewPassword = passwordHash;

                    user.Password = NewPassword;

                    _userRepository.Update(user);
                    _userRepository.SaveChanges();  
                    
                }
                else notifier.ShowError("Code was not entered correctly !!!");
            }
            else notifier.ShowError("Incoming Mail Have Already Been Registered !!!");

        }
        else notifier.ShowError("Something went wrong with the mail or password. Please try again.");
    }

    private void SendMailClick(object? obj)
    {
        try
        {
            Random random = new();
            _randomCode = random.Next(1000, 9999);

            string emailBody = $@"
                <div style='font-family: Arial, sans-serif; color: #333; padding: 20px;'>
                    <h2 style='color: #007BFF;'>Mega Market</h2>
                    <p>Here is your change Password verification code:</p>
                    <div style='font-size: 24px; font-weight: bold; color: #FF5733;'>
                        {_randomCode}
                    </div>
                    <p>Please enter this code to complete your Change Password process.</p>
                    <p style='color: #888;'>Thank you for joining us!</p>
                </div>";

            if (Validation.IsMail(Mail) && Validation.IsPassword(NewPassword) && Validation.IsPassword(ConfrimPassword)
                && NewPassword == ConfrimPassword)
            {
                MailServices.SendMail(Mail!, "Mega Market Change Password Code", emailBody);
                notifier.ShowSuccess("Email Code Sent Successfully");
            }
            else notifier.ShowError("Something went wrong with the mail or password. Please try again.");

        }
        catch (Exception)
        {
            notifier.ShowError("Something went wrong with the mail or password. Please try again.");
        }
    }

    private void SigInClick(object? obj)
    {
        _navigationService.Navigate<LoginView, LoginViewModel>();
    }

    private void CloseClick(object? obj)
    {
        Application.Current.MainWindow.Close();
        Environment.Exit(0);
    }
}
