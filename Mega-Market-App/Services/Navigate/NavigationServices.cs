using Mega_Market_App.ViewModels;
using System.Windows.Controls;

namespace Mega_Market_App.Services.Navigate;

public class NavigationServices : INavigationServices
{
    public void Navigate<TView, TViewModel>() where TView : Page where TViewModel : BaseViewModel
    {
        var mainVm = System.Windows.Application.Current.MainWindow.DataContext as MainViewModel;
        if (mainVm is not null)
        {
            mainVm!.CurrentPage = App.Container.GetInstance<TView>();
            mainVm.CurrentPage.DataContext = App.Container.GetInstance<TViewModel>();
        }
    }
}
