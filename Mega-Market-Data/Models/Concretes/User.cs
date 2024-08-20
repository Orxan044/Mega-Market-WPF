using Mega_Market_Data.Models.Abstract;
using System.Collections.ObjectModel;

namespace Mega_Market_Data.Models.Concretes;

public class User : BaseEntity
{
    public string? ImagePath { get; set; } = "https://i.imgur.com/eZWS6wW.jpg";
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? PhoneNumber { get; set; }
    public string? DateOfBrithday { get; set; }
    public string? Mail { get; set; }
    public string? Password { get; set; }
    public double? BonusBalance { get; set; } = 0;
    public ObservableCollection<CreditCart>? CreditCarts { get; set; } = [];
    public ObservableCollection<History>? Histories { get; set; } = [];

}
