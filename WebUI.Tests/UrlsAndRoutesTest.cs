using System;
using System.Reflection;
using System.Web;
using System.Web.Routing;
using HomeAccountingSystem_WebUI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace WebUI.Tests
{
    [TestClass]
    public class UrlsAndRoutesTest
    {
        private HttpContextBase CreateHttpContextBase(string url = null, string httpMethod = "GET")
        {
            Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Setup(m => m.AppRelativeCurrentExecutionFilePath).Returns(url);
            mockRequest.Setup(m => m.HttpMethod).Returns(httpMethod);
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>();
            mockResponse.Setup(m => m.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s);
            Mock<HttpContextBase> mockContext = new Mock<HttpContextBase>();
            mockContext.Setup(m => m.Request).Returns(mockRequest.Object);
            mockContext.Setup(m => m.Response).Returns(mockResponse.Object);
            return mockContext.Object;
        }

        private void TestRouteMatch(string url, string controller, string action, object routeProperties = null,
            string httpMethod = "GET")
        {
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            RouteData result = routes.GetRouteData(CreateHttpContextBase(url, httpMethod));

            Assert.IsNotNull(result);
            Assert.IsTrue(TestIncomingRouteResults(result,controller,action,routeProperties));
        }

        private bool TestIncomingRouteResults(RouteData routeResult, string controller, string action,
            object propertySet = null)
        {
            Func<object, object, bool> valCompare = (v1, v2) => StringComparer.InvariantCultureIgnoreCase.Compare(v1, v2) == 0;
            bool result = valCompare(routeResult.Values["controller"], controller) &&
                          valCompare(routeResult.Values["action"], action);

            if (propertySet != null)
            {
                PropertyInfo[] propInfo = propertySet.GetType().GetProperties();
                foreach (var pi in propInfo)
                {
                    if (!(routeResult.Values.ContainsKey(pi.Name) && valCompare(routeResult.Values[pi.Name],pi.GetValue(propertySet,null))))
                    {
                        result = false;
                        break;
                    }    
                }
            }

            return result;
        }

        private void TestRouteFail(string url)
        {
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            RouteData result = routes.GetRouteData(CreateHttpContextBase(url));

            Assert.IsTrue(result == null || result.Route == null);
        }

        //[TestMethod]
        public void TestIncomingRoutes()
        {
            TestRouteMatch("~/","PayingItem","List");
            TestRouteMatch("~/Page1", "PayingItem", "List", new { page = 1.ToString() });
            TestRouteMatch("~/Report", "Report", "Index");
            TestRouteMatch("~/Report/SubCategories/TypeOfFlowId","Report","SubCategories",new { typeOfFlowId = "TypeOfFlowId" });
            TestRouteMatch("~/1", "PayingItem", "Add");
            TestRouteMatch("~/Admin/Index/1/1", "Admin", "Index", new { typeOfFlowId = 1.ToString(), page = 1.ToString() });
            TestRouteMatch("~/Admin/Index/1/23ere-3434-dfdf3-34fd/1", "Admin", "Index", new
            {
                id = 1.ToString(),
                userId = "23ere-3434-dfdf3-34fd",
                typeOfFlowId = 1.ToString()
            });
            TestRouteMatch("~/Admin/Index/All", "Admin", "Index", new { id = "All" });
            TestRouteMatch("~/Admin/Index", "Admin", "Index");
            TestRouteMatch("~/One/Two", "One", "Two");

            TestRouteMatch("~/Admin","Admin","Index");
        }
    }
}