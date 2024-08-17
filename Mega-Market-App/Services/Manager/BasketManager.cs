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
    public  Basket Basket;

    public BasketManager()
    {
        NewBasket();
    }

    public Basket NewBasket()
    {
        return Basket = new Basket
        {
            ProductItems = [],
            TotalPayment = 0.0,
            ProductTotalPayment = 0.0,
            Discount = 0.0,
            OrderCount = 0
        }; 
    }

    public void AddProduct(Product product)
    {
        ProductItem productItem = new()
        {
            Product = product,
            ProductCount = 1
        };

        if (productItem is not null)
        {
            var existingItem = Basket.ProductItems?.FirstOrDefault(p => p.Product == productItem.Product);

            if (existingItem is not null)
            {
                existingItem.ProductCount += productItem.ProductCount;
                Basket.OrderCount += productItem.ProductCount;
            }
            else
            {
                Basket.ProductItems?.Add(productItem);
                Basket.OrderCount++;
            }

            

            double itemTotalPrice = (double)(productItem.Product!.Price * productItem.ProductCount)!;
            Basket.ProductTotalPayment = Math.Round((double)(Basket.ProductTotalPayment + itemTotalPrice)!, 2);
            Basket.TotalPayment = Math.Round((double)(Basket.ProductTotalPayment - Basket.Discount)!, 2);
        }

    }

    public void DeleteProduct(ProductItem productItem)
    {
        if (productItem != null)
        {
            var existingItem = Basket.ProductItems?.FirstOrDefault(p => p.Product == productItem.Product);
            if (existingItem != null)
            {
                double itemTotalPrice = (double)(existingItem.Product!.Price * existingItem.ProductCount)!;
                Basket.ProductItems!.Remove(existingItem);
                Basket.OrderCount -= existingItem.ProductCount;

                Basket.ProductTotalPayment = Math.Round((double)(Basket.ProductTotalPayment - itemTotalPrice)!, 2);
                Basket.TotalPayment = Math.Round((double)(Basket.ProductTotalPayment - Basket.Discount)!, 2);
            }
        }
    }

    public ObservableCollection<ProductItem> GetBasketProducts()
    {
        return Basket.ProductItems!;
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
        double totalProductPayment = (double)Basket.ProductItems!.Sum(pi => pi.Product!.Price * pi.ProductCount)!;
        Basket.ProductTotalPayment = totalProductPayment;
        Basket.TotalPayment = totalProductPayment - Basket.Discount;
    }


}
