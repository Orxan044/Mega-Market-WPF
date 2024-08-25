using Mega_Market_Data.Models.Abstract;
using System.Collections.ObjectModel;

namespace Mega_Market_Data.Models.Concretes;

public class DailyTotal : BaseEntity
{
    public DateTime? DateTime { get; set; }
    public double? TotalAmount { get; set; }
    public int? UserCount { get; set; }

}
