using Mega_Market_Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Mega_Market_Admin.Services;

public class NavigationService : INavigationService
{
    public void Navigate<TView, TViewModel>(Page? CurrentPage) where TView : Page where TViewModel : BaseViewModel
    {
        var mainVm = System.Windows.Application.Current.MainWindow.DataContext as MainViewModel;
        if (mainVm is not null)
        {
            mainVm!.CurrentPage = App.Container.GetInstance<TView>();
            mainVm.CurrentPage.DataContext = App.Container.GetInstance<TViewModel>();
        }
    }
}
