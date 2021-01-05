using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Exceptions;
using System.Threading.Tasks;
using WebUI.Core.Exceptions;

namespace WebUI.Core.Components
{
    public class ProductsForCategory : ViewComponent
    {
        private readonly IProductService _productService;

        public ProductsForCategory(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IViewComponentResult> InvokeAsync(int categoryId)
        {
            try
            {
                var products = await _productService.GetListAsync(p => p.CategoryID == categoryId);

                return View(products);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в ViewComponent с названием {nameof(ProductsForCategory)} в методе {nameof(InvokeAsync)}", e);
            }
        }
    }
}
