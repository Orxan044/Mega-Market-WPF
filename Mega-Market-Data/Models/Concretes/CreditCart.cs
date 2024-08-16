using Mega_Market_Data.Models.Abstract;
using Mega_Market_Data.Models.Enum;

namespace Mega_Market_Data.Models.Concretes;

public class CreditCart : BaseEntity
{
    public string? Code { get; set; }
    public int? EndYear { get; set; }
    public string? CVV { get; set; }
    public string? IconName { get; set; }
    public CartType Type { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
}
