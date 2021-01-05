using DomainModels.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Core.Exceptions;
using WebUI.Core.Models;

namespace WebUI.Core.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;


        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Add(WebUser user, int categoryId)
        {
            ViewBag.CategoryId = categoryId;

            return PartialView(new Product() { UserID = user.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Add(WebUser user, Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _productService.CreateAsync(product);

                    return RedirectToAction("EditableList", new { categoryId = product.CategoryID });
                }
                catch (ServiceException e)
                {
                    throw new WebUiException($"Ошибка в контроллере {nameof(ProductController)} в методе {nameof(Add)}", e);
                }                
            }

            ViewBag.CategoryId = product.CategoryID;

            return PartialView(product);
        }

        public IActionResult List(int categoryId)
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

        public IActionResult EditableList(int categoryId)
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var product = await _productService.GetItemAsync(id);
                await _productService.DeleteAsync(id);

                return RedirectToAction("EditableList", new { categoryId = product.CategoryID });
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(ProductController)} в методе {nameof(Delete)}", e);
            }
            catch (NullReferenceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(ProductController)} в методе {nameof(Delete)}", e);
            }
        }

        public async Task<IActionResult> Edit(WebUser user, int id)
        {
            try
            {
                var product = await _productService.GetItemAsync(id);
                var productEditModel = new ProductEditModel()
                {
                    Product = product
                };

                return PartialView(productEditModel);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(ProductController)} в методе {nameof(Edit)}", e);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductEditModel ptEdit)
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

        protected override void Dispose(bool disposing)
        {
            _productService.Dispose();

            base.Dispose(disposing);
        }
    }
}