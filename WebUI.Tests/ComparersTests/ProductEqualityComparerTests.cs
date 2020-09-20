using System;
using DomainModels.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebUI.Infrastructure.Comparers;

namespace WebUI.Tests.ComparersTests
{
    [TestClass]
    public class ProductEqualityComparerTests
    {
        [TestMethod]
        public void CheckEqualityCorrectness()
        {
            var comparer = new ProductEqualityComparer();
            var p1 = new Product()
            {
                ProductID = 1
            };
            var p2 = new Product()
            {
                ProductID = 1
            };

            var result = comparer.Equals(p1, p2);

            Assert.IsTrue(result);
        }
    }
}
