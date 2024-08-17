using Mega_Market_Data.Models.Abstract;
using Mega_Market_Data.Models.Enum;
using Microsoft.VisualBasic;

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

    public override string ToString() => $"*{Code!.Substring(Code.Length - 4)} - {Type}";
}
