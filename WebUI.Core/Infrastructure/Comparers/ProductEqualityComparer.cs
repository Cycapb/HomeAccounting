using DomainModels.Model;
using System.Collections.Generic;

namespace WebUI.Core.Infrastructure.Comparers
{
    public class ProductEqualityComparer : IEqualityComparer<Product>
    {
        public bool Equals(Product x, Product y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            return x.ProductID == y.ProductID;
        }

        public int GetHashCode(Product obj)
        {
            return obj.ProductID + 31.GetHashCode();
        }
    }
}