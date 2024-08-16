using Mega_Market_Data.Models.Abstract;
namespace Mega_Market_Data.Models.Concretes;

public class Admin : BaseEntity
{
    public string? FullName { get; set; }
    public string? AccountName { get; set; }
    public string? AccountPassword { get; set; }
}
