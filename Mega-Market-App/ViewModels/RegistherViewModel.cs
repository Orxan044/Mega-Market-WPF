using Mega_Market_App.Command;
using Mega_Market_App.Services.Mail;
using Mega_Market_App.Services.Navigate;
using Mega_Market_App.Validations;
using Mega_Market_App.Views;
using Mega_Market_Data.Data;
using Mega_Market_Data.Models.Concretes;
using Mega_Market_Data.Repositoies;
using System.Windows;
using ToastNotifications.Messages;

namespace Mega_Market_App.ViewModels;


public class RegistherViewModel : BaseViewModel
{
    private User _newUser = new();
    public User NewUser { get => _newUser; set { _newUser = value; OnPropertyChanged(); } }

    private string? _sendCodesms;

    public string? SendCodeSms { get => _sendCodesms; set { _sendCodesms = value; OnPropertyChanged(); } }

    public RelayCommand SignInCommand { get; set; }
    public RelayCommand SignUpCommand { get; set; }
    public RelayCommand SendCodeCommand { get; set; }
    public RelayCommand CloseCommand { get; set; }

    private int _randomCode;
    private readonly INavigationServices _navigationServices;
    private IRepository<User, UserDbContext> _userRepository;

    public RegistherViewModel(IRepository<User, UserDbContext> userRepository, INavigationServices navigationServices)
    {
        _navigationServices = navigationServices;
        _userRepository = userRepository;
        SignInCommand = new RelayCommand(SigInClick);
        SignUpCommand = new RelayCommand(SigUpClick);
        SendCodeCommand = new RelayCommand(SendCodeClick);
        CloseCommand = new RelayCommand(CloseClick);
    }


    private void SendCodeClick(object? obj)
    {
        try
        {
            Random random = new();
            _randomCode = random.Next(1000, 9999);

            string emailBody = $@"
                <div style='font-family: Arial, sans-serif; color: #333; padding: 20px;'>
                    <h2 style='color: #007BFF;'>Mega Market</h2>
                    <p>Welcome! Here is your sign-up verification code:</p>
                    <div style='font-size: 24px; font-weight: bold; color: #FF5733;'>
                        {_randomCode}
                    </div>
                    <p>Please enter this code to complete your sign-up process.</p>
                    <p style='color: #888;'>Thank you for joining us!</p>
                </div>";

            if (Validation.IsMail(NewUser.Mail))
            {
                MailServices.SendMail(NewUser.Mail!, "Mega Market Sign Up Code", emailBody);
                notifier.ShowSuccess("Email Code Sent Successfully");
            }
            else notifier.ShowError("Something went wrong with the mail. Please try again.");

        }
        catch (Exception)
        {
            notifier.ShowError("Something went wrong with the mail. Please try again.");
        }
    }

    private void SigUpClick(object? obj)
    {
        try
        {
            if (Validation.IsText(NewUser.Name) && Validation.IsText(NewUser.Surname))
            {
                if (Validation.IsPassword(NewUser.Password!))
                {
                    string passwordHash = BCrypt.Net.BCrypt.HashPassword(NewUser.Password);
                    NewUser.Password = passwordHash;

                    if (Validation.IsMail(NewUser.Mail))
                    {
                        bool checkMail = _userRepository.GetAll().Any(user => user.Mail == NewUser.Mail);

                        if (!checkMail)
                        {
                            if (SendCodeSms is not null && Convert.ToInt32(SendCodeSms) == _randomCode)
                            {
                                notifier.ShowSuccess("You Have Successfully Registered");
                                _userRepository.Add(NewUser);
                                _userRepository.SaveChanges();
                                NewUser = new();
                                _navigationServices.Navigate<MenyuView, MenyuViewModel>();
                            }
                            else notifier.ShowError("Code was not entered correctly !!!");
                        }
                        else notifier.ShowError("Incoming Mail Have Already Been Registered !!!");
                    }
                    else notifier.ShowError("Mail was not entered correctly !!!");
                }
                else notifier.ShowError("Password not entered correctly (8 characters, numbers and letters) !!!");
            }
            else notifier.ShowError("Name and Surname Are Not Entered Correctly !!!");
        }
        catch (Exception)
        {
            notifier.ShowError("Something went wrong with the mail. Please try again.");
        }

    }

    private void SigInClick(object? obj)
    {
        _navigationServices.Navigate<LoginView, LoginViewModel>();
    }
    private void CloseClick(object? obj)
    {
        Application.Current.MainWindow.Close();
        Environment.Exit(0);
    }

}
