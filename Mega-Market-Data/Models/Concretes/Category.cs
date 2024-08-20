using Mega_Market_Data.Models.Abstract;


namespace Mega_Market_Data.Models.Concretes;

public class Category : BaseEntity
{
    public string? ImagePath { get; set; } = "pack://application:,,,/Admin;component/Photos/whiteBackGround.jpg";
    public string? Name { get; set; }
    public ICollection<Product>? Products { get; set; }


    public override string ToString() => $"{Name}";

    public Category Clone() => new() { Id = Id, ImagePath = ImagePath, Name = Name, Products = Products };

}
