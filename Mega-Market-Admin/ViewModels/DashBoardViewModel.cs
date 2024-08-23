using Mega_Market_Data.Data;
using Mega_Market_Data.Models.Concretes;
using Mega_Market_Data.Repositoies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega_Market_Admin.ViewModels;

public class DashBoardViewModel : BaseViewModel
{
	private int? _totalUser;

	public int? TotalUser
	{
		get => _totalUser; 
		set { _totalUser = value; OnPropertyChanged(); }
	}

    private int? _totalOrder;

    public int? TotalOrder
    {
        get => _totalOrder;
        set { _totalOrder = value; OnPropertyChanged(); }
    }

    private int? _totalMessage;

    public int? TotalMessage
    {
        get => _totalMessage;
        set { _totalMessage = value; OnPropertyChanged(); }
    }

    private double? _totalRevenue = 0;

    public double? TotalRevenue
    {
        get => _totalRevenue;
        set { _totalRevenue = value; OnPropertyChanged(); }
    }

    private readonly IRepository<Product, MarketDbContext> _productRepository;
    private readonly IRepository<User, UserDbContext> _userRepository;
    private readonly IRepository<Message, UserDbContext> _messageRepository;
    private readonly IRepository<History, UserDbContext> _historyRepository;


    public DashBoardViewModel(IRepository<Product,MarketDbContext> productRepository,IRepository<User,UserDbContext> userRepository,
        IRepository<Message,UserDbContext> messageRepository,IRepository<History,UserDbContext> historyRepository)
    {
        _productRepository = productRepository;
        _userRepository = userRepository;
        _messageRepository = messageRepository;
        _historyRepository = historyRepository;

        TotalUser = _userRepository.GetAll().Count();
        TotalOrder = _productRepository.GetAll().Count();
        TotalMessage = _messageRepository.GetAll().Count();
        TotalRevenueCal();
    }


    private void TotalRevenueCal()
    {
        foreach (var total in _historyRepository.GetAll())
        {
            var math = Math.Round((double)(total.TotalAmount!), 2) ;
            TotalRevenue += math;
        }

    }
}
