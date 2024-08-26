using Mega_Market_App.Services.Mail;
using Mega_Market_App.Services.Manager;
using Mega_Market_App.Services.Navigate;
using Mega_Market_App.ViewModels;
using Mega_Market_App.Views;
using Mega_Market_Data.Data;
using Mega_Market_Data.Models.Concretes;
using Mega_Market_Data.Repositoies;
using SimpleInjector;
using System.Windows;

namespace Mega_Market_App;

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
        Container.RegisterSingleton<INavigationServices, NavigationServices>();
        Container.RegisterSingleton<MailServices>();
        Container.RegisterSingleton<UserDbContext>();
        Container.RegisterSingleton<MarketDbContext>();
        Container.RegisterSingleton<BasketManager>();


        Container.RegisterSingleton<IRepository<User, UserDbContext>, Repository<User, UserDbContext>>();
        Container.RegisterSingleton<IRepository<CreditCart, UserDbContext>, Repository<CreditCart, UserDbContext>>();
        Container.RegisterSingleton<IRepository<History, UserDbContext>, Repository<History, UserDbContext>>();
        Container.RegisterSingleton<IRepository<Message, UserDbContext>, Repository<Message, UserDbContext>>();
        Container.RegisterSingleton<IRepository<ProductHistory, UserDbContext>, Repository<ProductHistory, UserDbContext>>();

        Container.RegisterSingleton<IRepository<Product, MarketDbContext>, Repository<Product, MarketDbContext>>();
        Container.RegisterSingleton<IRepository<Category, MarketDbContext>, Repository<Category, MarketDbContext>>();
    }

    private static void AddViewModels()
    {
        Container.RegisterSingleton<MainViewModel>();
        Container.RegisterSingleton<SplashViewModel>();
        Container.RegisterSingleton<LoginViewModel>();
        Container.RegisterSingleton<RegistherViewModel>();
        Container.RegisterSingleton<ForgotPasswordViewModel>();
        Container.Register<DashBoradViewModel>();
        Container.RegisterSingleton<MenyuViewModel>();
        Container.RegisterSingleton<CategoriesViewModel>();
        Container.RegisterSingleton<ProductsViewModel>();
        Container.Register<BasketViewModel>();
        Container.Register<CreditCartViewModel>();
        Container.Register<HistoryViewModel>();
        Container.Register<CheckViewModel>();
        Container.Register<SettingsViewModel>();
        Container.Register<MessageViewModel>();
    }

    private static void AddViews()
    {
        Container.RegisterSingleton<MainView>();
        Container.RegisterSingleton<SplashView>();
        Container.RegisterSingleton<LoginView>();
        Container.RegisterSingleton<RegistherView>();
        Container.RegisterSingleton<ForgotPasswordView>();
        Container.RegisterSingleton<MenyuView>();
        Container.RegisterSingleton<CategoriesView>();
        Container.RegisterSingleton<ProductsView>();
        Container.RegisterSingleton<DashBoardView>();
        Container.RegisterSingleton<BasketView>();
        Container.RegisterSingleton<CreditCartView>();
        Container.RegisterSingleton<HistoryView>();
        Container.RegisterSingleton<CheckView>();
        Container.RegisterSingleton<SettingsView>();
        Container.RegisterSingleton<MessageView>();
    }

    protected override void OnStartup(StartupEventArgs e)
    {

        var mainView = Container.GetInstance<SplashView>();
        mainView.DataContext = Container.GetInstance<SplashViewModel>();
        mainView.Show();
        base.OnStartup(e);
    }
}
