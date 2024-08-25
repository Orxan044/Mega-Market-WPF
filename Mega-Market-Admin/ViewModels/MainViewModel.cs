using Mega_Market_Admin.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Mega_Market_Admin.ViewModels;

public class MainViewModel : BaseViewModel, INotifyPropertyChanged
{
    private Page? _currentPage;
    public Page? CurrentPage { get => _currentPage; set { _currentPage = value; OnPropertyChanged(); } }

    public MainViewModel()
    {
        _currentPage = App.Container.GetInstance<LoginView>();
        _currentPage.DataContext = App.Container.GetInstance<LoginViewModel>();
    }

}
