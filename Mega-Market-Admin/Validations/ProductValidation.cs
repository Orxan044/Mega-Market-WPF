using Mega_Market_Data.Models.Concretes;


namespace Mega_Market_Admin.Validations;

public class ProductValidation
{
    private readonly Product _product;

    public ProductValidation(Product product)
    {
        _product = product;
    }

    public bool IsNull()
    {
        if (_product.Name is null || _product.Description is null
            || _product.Category is null || _product.Price is null ||
            _product.Quantity is null) return false;
        else return true;
    }

    public bool IsNegative()
    {
        if (_product.Price <= 0 || _product.Quantity < 0) return false;
        else return true;
    }

}
