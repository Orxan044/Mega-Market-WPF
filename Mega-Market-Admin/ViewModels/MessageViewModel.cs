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
using System.Windows.Data;

namespace Mega_Market_Admin.ViewModels;

public class MessageViewModel : BaseViewModel
{

    private string? _searchText;
    private ICollectionView _messagesView;

    public string SearchText
    {
        get => _searchText!;
        set
        {
            _searchText = value;
            OnPropertyChanged();
            _messagesView.Refresh();
        }
    }

    private ObservableCollection<Message>? _messages = [];

    public ObservableCollection<Message>? Messages
    {
        get => _messages;
        set { _messages = value; OnPropertyChanged(); }
    }

    private readonly IRepository<Message, UserDbContext> _messageRepository;
    private readonly IRepository<User, UserDbContext> _userRepository;


    public MessageViewModel(IRepository<Message, UserDbContext> messageRepository, IRepository<User, UserDbContext> userRepository)
    {
        _messageRepository = messageRepository;
        _userRepository = userRepository;


        LoadMessages();

        _messagesView = CollectionViewSource.GetDefaultView(Messages);
        _messagesView.Filter = FilterCategories;
    }

    private void LoadMessages()
    {
        var messages = _messageRepository.GetAll().ToList();

        foreach (var message in messages)
        {
            if (message.UserId != null)
            {
                message.User = _userRepository.Get(message.UserId.Value);
            }
        }

        Messages = new ObservableCollection<Message>(messages);
    }

    private bool FilterCategories(object obj)
    {
        if (obj is Message message)
        {
            return string.IsNullOrEmpty(SearchText) || message.User!.Mail!.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
            message.Id.ToString()!.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
        }
        return false;
    }
}
