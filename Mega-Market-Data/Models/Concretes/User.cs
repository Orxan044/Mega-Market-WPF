using Mega_Market_Data.Models.Abstract;
using System.Collections.ObjectModel;

namespace Mega_Market_Data.Models.Concretes;

public class User : BaseEntity
{
    public string? ImagePath { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? DateOfBrithday { get; set; }
    public string? Mail { get; set; }
    public string? Password { get; set; }
    public ObservableCollection<CreditCart>? CreditCarts { get; set; } = [];
}
