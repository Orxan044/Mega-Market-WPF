using Mega_Market_Data.Models.Abstract;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Mega_Market_Data.Models.Concretes;

public class ProductItem : BaseEntity , INotifyPropertyChanged
{
    public Product? Product { get; set; }
 
    private int? _productCount = 1;
    public int? ProductCount 
    { 
        get => _productCount;
        
        set
        {
            if(value.HasValue && Product is not null)
            {

                if (value.Value >= 1 && value.Value <= Product.Quantity) _productCount = value;
                else if (value.Value > Product.Quantity) _productCount = Product.Quantity;
                else _productCount = 1;
                OnPropertyChanged();
            }
        }
    }

    #region INotifyPropertyChanged event
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? paramName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(paramName));
    #endregion
}
