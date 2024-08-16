using Mega_Market_App.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Mega_Market_App.Services.Navigate;

public interface INavigationServices
{
    void Navigate<TView, TViewModel>() where TView : Page where TViewModel : BaseViewModel;

}
