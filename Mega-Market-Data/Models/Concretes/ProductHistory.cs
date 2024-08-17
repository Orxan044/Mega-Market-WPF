using Mega_Market_Data.Models.Abstract;

namespace Mega_Market_Data.Models.Concretes;

public class ProductHistory : BaseEntity
{
    public string? ImagePath { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public double? Price { get; set; }
    public int? Count { get; set; }

    public int HistoryId { get; set; } // Foreign Key
    public History? History { get; set; } // Navigation Property
}
