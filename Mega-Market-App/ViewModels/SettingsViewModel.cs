using Mega_Market_App.Command;
using Mega_Market_App.Services.Mail;
using Mega_Market_App.Validations;
using Mega_Market_Data.Models.Concretes;
using Mega_Market_Data.Repositoies;
using Microsoft.Identity.Client;
using ToastNotifications.Messages;

namespace Mega_Market_App.ViewModels;

public class SettingsViewModel : BaseViewModel
{
    private User? _userSettings = new();

	public User UserSettings
	{
		get => _userSettings!; 
		set { _userSettings = value; OnPropertyChanged(); }
	}


	private string? _newPassword;

	public string NewPassword
	{
		get => _newPassword!; 
		set { _newPassword = value; OnPropertyChanged(); }
	}

    private string? _newMail;

    public string NewMail
    {
        get => _newMail!;
        set { _newMail = value; OnPropertyChanged(); }
    }

    private int? _sendSms;

    public int? SendSms
    {
        get => _sendSms!;
        set { _sendSms = value; OnPropertyChanged(); }
    }

    public RelayCommand SendCodePasswordCommand { get; set; }
    public RelayCommand SendCodeMailCommand { get; set; }
    public RelayCommand ChangeCommand { get; set; }

    private readonly LoginViewModel _loginViewModel;
    private int? _check;
    private int? _randomCode;

    
	public SettingsViewModel(LoginViewModel loginViewModel)
    {
		_loginViewModel = loginViewModel;
        UserNew();

        SendCodePasswordCommand = new RelayCommand(SendCodePasswordClick);
        SendCodeMailCommand = new RelayCommand(SendCodeMailClick);
        ChangeCommand = new RelayCommand(ChangeClick);
    }

    private void UserNew()
    {
        foreach (var user in _loginViewModel.UserRepository.GetAll())
        {
            if (_loginViewModel.UserLogin.Mail == user.Mail) UserSettings = user;
        }
    }

    private void ChangeClick(object? obj)
    {
        if (_randomCode == SendSms)
        {
            if (_check == 0) 
            {
                if (_loginViewModel.UserRepository.GetAll().Any(u => u.Mail == NewMail))
                    notifier.ShowError("This email address is already in use.");

                _loginViewModel.UserLogin.Mail = NewMail;
            }

            else if (_check == 1)  
                _loginViewModel.UserLogin.Password = NewPassword;
            

            notifier.ShowSuccess("Successfully Changed !!!");
            SettingsUser();
            _loginViewModel.UserRepository.SaveChanges();
        }
        else if (_randomCode is null)
        {
            SettingsUser();
            _loginViewModel.UserRepository.SaveChanges();
        }
        else
            notifier.ShowError("Code was not entered correctly !!!");
        
    }

    private void SendCodeMailClick(object? obj)
    {
        if (Validation.IsMail(NewMail)) { SendSmsMethod(); _check = 0; }

        else notifier.ShowError("Something went wrong with the mail. Please try again.");
    }

    private void SendCodePasswordClick(object? obj)
    {
        if (Validation.IsPassword(NewPassword))
        {
            SendSmsMethod();
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(NewPassword);
            NewPassword = passwordHash;
            _check = 1; 
        }

        else notifier.ShowError("Something went wrong with the password. Please try again.");
    }

    private void SettingsUser()
    {
        _loginViewModel.UserLogin.ImagePath = UserSettings.ImagePath;
        _loginViewModel.UserLogin.Name = UserSettings.Name;
        _loginViewModel.UserLogin.Surname = UserSettings.Surname;
        _loginViewModel.UserLogin.PhoneNumber = UserSettings.PhoneNumber;
        _loginViewModel.UserLogin.DateOfBrithday = UserSettings.DateOfBrithday;
    }

    private void SendSmsMethod()
    {
        try
        {
            Random random = new();
            _randomCode = random.Next(1000, 9999);


            string emailBody = $@"
                <div style='font-family: Arial, sans-serif; color: #333; padding: 20px;'>
                    <h2 style='color: #007BFF;'>Mega Market</h2>
                    <p>Code to make a change:</p>
                    <div style='font-size: 24px; font-weight: bold; color: #FF5733;'>
                        {_randomCode}
                    </div>
                    <p>Enter this code to complete your change process.</p>
                </div>";

            MailServices.SendMail(UserSettings.Mail!, "Mega Market Change Code", emailBody);
            notifier.ShowSuccess("Email Code Sent Successfully");

        }
        catch (Exception)
        {
            notifier.ShowError("Something went wrong with the mail. Please try again.");
        }
    }
}
