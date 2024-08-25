using Mega_Market_Data.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Mega_Market_Data.Models.Concretes;

public class Basket : BaseEntity , INotifyPropertyChanged
{
    private double? totalPayment;
    private double? productTotalPayment;
    private double? discount;
    private int? orderCount;
    private ObservableCollection<ProductItem>? productItems = [];

    public ObservableCollection<ProductItem>? ProductItems { get => productItems; 
        set { productItems = value; OnPropertyChanged(); } }

    public double? TotalPayment { get => totalPayment; set { totalPayment = value; OnPropertyChanged(); } }
    public double? ProductTotalPayment { get => productTotalPayment; set { productTotalPayment = value; OnPropertyChanged(); }}
    public double? Discount { get => discount; set { discount = value; OnPropertyChanged(); }}
    public int? OrderCount { get => orderCount; set { orderCount = value; OnPropertyChanged(); }}
        
    
    
    
    
    #region INotifyPropertyChanged event
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? paramName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(paramName));
    #endregion
}
