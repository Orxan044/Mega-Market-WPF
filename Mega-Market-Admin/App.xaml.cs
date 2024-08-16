using Mega_Market_Admin.Services;
using Mega_Market_Admin.ViewModels;
using Mega_Market_Admin.Views;
using Mega_Market_Data.Data;
using Mega_Market_Data.Models.Concretes;
using Mega_Market_Data.Repositoies;
using SimpleInjector;
using System.Windows;


namespace Mega_Market_Admin;


public partial class App : Application
{
    public static Container Container { get; set; } = new();
    public App()
    {
        AddOtherServices();
        AddViews();
        AddViewModels();
    }

    private static void AddOtherServices()
    {
        Container.RegisterSingleton<AdminDbContext>();
        Container.RegisterSingleton<MarketDbContext>();

        Container.RegisterSingleton<IRepository<Admin, AdminDbContext>, Repository<Admin, AdminDbContext>>();
        Container.RegisterSingleton<IRepository<Category, MarketDbContext>, Repository<Category, MarketDbContext>>();
        Container.RegisterSingleton<IRepository<Product, MarketDbContext>, Repository<Product, MarketDbContext>>();

        Container.RegisterSingleton<INavigationService, NavigationService>();
    }

    private static void AddViewModels()
    {
        Container.RegisterSingleton<MainViewModel>();
        Container.RegisterSingleton<LoginViewModel>();
        Container.RegisterSingleton<MenyuViewModel>();
        Container.RegisterSingleton<DashBoardViewModel>();
        Container.Register<CategoryViewModel>();
        Container.RegisterSingleton<AddCategoryViewModel>();
        Container.Register<ProductsViewModel>();
        Container.Register<ProductShowViewModel>();
        Container.Register<AddProductViewModel>();
    }

    private static void AddViews()
    {
        Container.RegisterSingleton<MainView>();
        Container.RegisterSingleton<LoginView>();
        Container.RegisterSingleton<MenyuView>();
        Container.RegisterSingleton<DashBoardView>();
        Container.RegisterSingleton<CategoryView>();
        Container.RegisterSingleton<AddCategoryView>();
        Container.RegisterSingleton<ProductsView>();
        Container.RegisterSingleton<ProductShowView>();
        Container.RegisterSingleton<AddProductView>();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        var mainView = Container.GetInstance<MainView>();
        mainView.DataContext = Container.GetInstance<MainViewModel>();
        mainView.Show();
        base.OnStartup(e);
    }
}
