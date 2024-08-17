using Mega_Market_Data.Models.Abstract;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;

namespace Mega_Market_Data.Models.Concretes;

public class History : BaseEntity
{
    public ObservableCollection<ProductHistory>? Products { get; set; } = [];
    public DateTime? Date { get; set; }
    public double? TotalAmount { get; set; }
    public int? UserId { get; set; }
    public User? User { get; set; }

}
