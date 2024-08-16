using Mega_Market_Data.Models.Concretes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Mega_Market_App.Services.Manager;

public class BasketManager
{
    private  Basket _basket;

    public BasketManager()
    {
        _basket = new Basket
        {
            ProductItems = [],
            TotalPayment = 0.0,
            ProductTotalPayment = 0.0,
            Discount = 0.0,
            OrderCount = 0
        };
    }

    public Basket GetBasket()
    {
        return _basket;
    }

    public void AddProduct(Product product)
    {
        ProductItem productItem = new(product, 1);

        if (productItem is not null)
        {
            var existingItem = _basket.ProductItems?.FirstOrDefault(p => p.Product == productItem.Product);

            if (existingItem is not null)
            {
                existingItem.ProductCount += productItem.ProductCount;
                _basket.OrderCount += productItem.ProductCount;
            }
            else
            {
                _basket.ProductItems?.Add(productItem);
                _basket.OrderCount++;
            }

            

            double itemTotalPrice = (double)(productItem.Product!.Price * productItem.ProductCount)!;
            _basket.ProductTotalPayment = Math.Round((double)(_basket.ProductTotalPayment + itemTotalPrice)!, 2);
            _basket.TotalPayment = Math.Round((double)(_basket.ProductTotalPayment - _basket.Discount)!, 2);
        }

    }

    public void DeleteProduct(ProductItem productItem)
    {
        if (productItem != null)
        {
            var existingItem = _basket.ProductItems?.FirstOrDefault(p => p.Product == productItem.Product);
            if (existingItem != null)
            {
                double itemTotalPrice = (double)(existingItem.Product!.Price * existingItem.ProductCount)!;
                _basket.ProductItems!.Remove(existingItem);
                _basket.OrderCount -= existingItem.ProductCount;

                _basket.ProductTotalPayment = Math.Round((double)(_basket.ProductTotalPayment - itemTotalPrice)!, 2);
                _basket.TotalPayment = Math.Round((double)(_basket.ProductTotalPayment - _basket.Discount)!, 2);
            }
        }
    }

    public ObservableCollection<ProductItem> GetBasketProducts()
    {
        return _basket.ProductItems!;
    }

    public void IncrementProduct(ProductItem productItem)
    {
        if (productItem != null)
        {
            productItem.ProductCount++;
            UpdatePayments();
        }
    }

    public void DecrementProduct(ProductItem productItem)
    {
        if (productItem != null && productItem.ProductCount > 1)
        {
            productItem.ProductCount--;
            UpdatePayments();
        }
    }

    private void UpdatePayments()
    {
        double totalProductPayment = (double)_basket.ProductItems!.Sum(pi => pi.Product!.Price * pi.ProductCount)!;
        _basket.ProductTotalPayment = totalProductPayment;
        _basket.TotalPayment = totalProductPayment - _basket.Discount;
    }


}
