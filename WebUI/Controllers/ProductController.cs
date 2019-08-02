using DomainModels.Model;
using Services;
using Services.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.SessionState;
using WebUI.Exceptions;
using WebUI.Models;

namespace WebUI.Controllers
{
    [Authorize]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;


        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public ActionResult Add(WebUser user, int categoryId)
        {
            ViewBag.CategoryId = categoryId;
            return PartialView(new Product() { UserID = user.Id });
        }

        [HttpPost]
        public async Task<ActionResult> Add(WebUser user, Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _productService.CreateAsync(product);
                }
                catch (ServiceException e)
                {
                    throw new WebUiException($"Ошибка в контроллере {nameof(ProductController)} в методе {nameof(Add)}",
                        e);
                }
                return RedirectToAction("EditableList", new { categoryId = product.CategoryID });
            }
            ViewBag.CategoryId = product.CategoryID;
            return PartialView(product);
        }

        public PartialViewResult List(int categoryId)
        {
            try
            {
                var products = _productService.GetList(p => p.CategoryID == categoryId)
                    .ToList();

                return PartialView(products);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(ProductController)} в методе {nameof(List)}",
                    e);
            }
        }

        public ActionResult EditableList(int categoryId)
        {
            try
            {
                var products = _productService.GetList(p => p.CategoryID == categoryId)
                    .ToList();

                return PartialView(products);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(ProductController)} в методе {nameof(EditableList)}",
                    e);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var product = await _productService.GetItemAsync(id);
                await _productService.DeleteAsync(id);
                return RedirectToAction("EditableList", new { categoryId = product.CategoryID });
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(ProductController)} в методе {nameof(Delete)}",
                    e);
            }
            catch (NullReferenceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(ProductController)} в методе {nameof(Delete)}",
                    e);
            }
        }

        public async Task<ActionResult> Edit(WebUser user, int id)
        {
            try
            {
                var product = await _productService.GetItemAsync(id);
                ProductToEdit ptEdit = new ProductToEdit()
                {
                    Product = product
                };
                return PartialView(ptEdit);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(ProductController)} в методе {nameof(Edit)}",
                    e);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(ProductToEdit ptEdit)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _productService.UpdateAsync(ptEdit.Product);
                }
                catch (ServiceException e)
                {
                    throw new WebUiException($"Ошибка в контроллере {nameof(ProductController)} в методе {nameof(Edit)}",
                        e);
                }
                return RedirectToAction("EditableList", new { categoryId = ptEdit.Product.CategoryID });
            }
            return PartialView(ptEdit);
        }
    }
}