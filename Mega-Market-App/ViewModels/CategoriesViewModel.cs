using Mega_Market_Data.Data;
using Mega_Market_Data.Repositoies;
using Mega_Market_Data.Models.Concretes;
using System.Collections.ObjectModel;
using Mega_Market_App.Command;
using Mega_Market_App.Views;
using System.Windows.Data;
using System.Windows.Input;

namespace Mega_Market_App.ViewModels;

public class CategoriesViewModel : BaseViewModel
{


    public ObservableCollection<Category> Categories { get; set; }
     

    private readonly IRepository<Category,MarketDbContext> _productRepository;

    public RelayCommand ImageClickCommand { get; }

    public CategoriesViewModel(IRepository<Category,MarketDbContext> productRepository)
    {
        _productRepository = productRepository;

        Categories = new ObservableCollection<Category>(_productRepository.GetAll());

        ImageClickCommand = new RelayCommand(ImageClick);

    }

    private void ImageClick(object? obj)
    {
        var category = obj as Category;

        if (category is not null)
        {
            var mainVm = App.Current.MainWindow.DataContext as MainViewModel;
            if (mainVm is not null)
            {
                
                var vm = mainVm.CurrentPage!.DataContext as MenyuViewModel;
                vm!.ProductsClik(category);

                var MainView = vm!.CurrentPage2!.DataContext as ProductsViewModel;
                MainView!.SelectedCategory = category;
                MainView.AddProducts();
            }
        }
    }

}
