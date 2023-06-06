using DomainModels.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebUI.Core.Infrastructure.Comparers;

namespace WebUI.Core.Tests.ComparersTests
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
