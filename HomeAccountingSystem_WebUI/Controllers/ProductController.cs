using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.SessionState;
using HomeAccountingSystem_DAL.Model;
using HomeAccountingSystem_WebUI.Models;
using Services;

namespace HomeAccountingSystem_WebUI.Controllers
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
            return PartialView(new Product() {UserID = user.Id});
        }

        [HttpPost]
        public async Task<ActionResult> Add(WebUser user,Product product)
        {
            if (ModelState.IsValid)
            {
                await _productService.CreateAsync(product);
                return RedirectToAction("EditableList", new { categoryId = product.CategoryID });
            }
            ViewBag.CategoryId = product.CategoryID;
            return PartialView(product);
        }

        public PartialViewResult List(int categoryId)
        {
            var products = _productService.GetList()
                .Where(p => p.CategoryID == categoryId)
                .ToList();
            return PartialView(products);
        }

        public ActionResult EditableList(int categoryId)
        {
            var products = _productService.GetList()
                .Where(p => p.CategoryID == categoryId)
                .ToList();
            return PartialView(products);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            var product = await _productService.GetItemAsync(id);
            await _productService.DeleteAsync(id);
            return RedirectToAction("EditableList", new { categoryId = product.CategoryID });
        }

        public async Task<ActionResult> Edit(WebUser user, int id)
        {
            var product = await _productService.GetItemAsync(id);
            ProductToEdit ptEdit = new ProductToEdit()
            {
                Product = product
            };
            return PartialView(ptEdit);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(ProductToEdit ptEdit)
        {
            if (ModelState.IsValid)
            {
                await _productService.UpdateAsync(ptEdit.Product);
                return RedirectToAction("EditableList", new { categoryId = ptEdit.Product.CategoryID });
            }
            return PartialView(ptEdit);
        }
    }
}