using Mega_Market_Admin.ViewModels;
using System.Windows.Controls;

namespace Mega_Market_Admin.Services;

public interface INavigationService
{
    void Navigate<TView, TViewModel>(Page? CurrentPage) where TView : Page where TViewModel : BaseViewModel;

}
