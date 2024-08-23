using Mega_Market_App.Command;
using Mega_Market_Data.Data;
using Mega_Market_Data.Models.Concretes;
using Mega_Market_Data.Models.Enum;
using Mega_Market_Data.Repositoies;
using Microsoft.VisualBasic.ApplicationServices;
using System.Collections.ObjectModel;
using ToastNotifications.Messages;
using User = Mega_Market_Data.Models.Concretes.User;

namespace Mega_Market_App.ViewModels;

public class MessageViewModel : BaseViewModel
{
	private MessageType? _selectedMessage;

	public MessageType? SelectedMessage
    {
		get => _selectedMessage; 
		set { _selectedMessage = value; OnPropertyChanged(); }
	}

    private string? _messagesText;

    public string? MessagesText
    {
        get => _messagesText; 
        set { _messagesText = value; OnPropertyChanged(); }
    }

    private ObservableCollection<MessageType>? messageTypeComboBox = [];

    public ObservableCollection<MessageType>? MessageTypeComboBox
    {
        get => messageTypeComboBox;
        set { messageTypeComboBox = value; OnPropertyChanged(); }
    }

    private ObservableCollection<Message>? myMessages = [];

    public ObservableCollection<Message>? MyMessages
    {
        get => myMessages;
        set { myMessages = value; OnPropertyChanged(); }
    }

    public RelayCommand SendMessageCommand {  get; set; } 

    private readonly IRepository<Message,UserDbContext> _messageRepository;
    private readonly LoginViewModel _loginViewModel;
    private User _user;

    public MessageViewModel(IRepository<Message,UserDbContext> messageRepository,LoginViewModel loginViewModel)
    {
		_messageRepository = messageRepository;
        _loginViewModel = loginViewModel;

        SendMessageCommand = new RelayCommand(SendMessageClick);
        _user = new();
        MyMessages = [];
        MessageTypeComboBox = new ObservableCollection<MessageType>(Enum.GetValues(typeof(MessageType)).Cast<MessageType>());

        GetUser();
        InputValue();

    }

    private void SendMessageClick(object? obj)
    {
        if (MessagesText is not null && SelectedMessage is not null)
        {
            var _message = new Message()
            {
                Messages = MessagesText,
                Type = SelectedMessage,
                SendTime = DateTime.Now,
                UserId = _user.Id,
            };

            _messageRepository.Add(_message);
            _messageRepository.SaveChanges();
            MyMessages!.Add(_message);
            notifier.ShowSuccess("New Messages Added Successfully !!!");

            MessagesText = null;
            MessageTypeComboBox = null;
        }
        else notifier.ShowError("Information Not Null !!!");
    }

    private void InputValue()
    {
        //My Messages
        foreach (var message in _messageRepository.GetAll())
            if(message.UserId == _user.Id) MyMessages!.Add(message);        
    }

	private void GetUser()
	{
        foreach (var user in _loginViewModel.UserRepository.GetAll())
            if (_loginViewModel.UserLogin.Mail == user.Mail) _user = user;
    }
}
