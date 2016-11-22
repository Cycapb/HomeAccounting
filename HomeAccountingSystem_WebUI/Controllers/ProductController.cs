using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.SessionState;
using HomeAccountingSystem_DAL.Model;
using HomeAccountingSystem_DAL.Repositories;
using HomeAccountingSystem_WebUI.Models;

namespace HomeAccountingSystem_WebUI.Controllers
{
    [Authorize]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class ProductController : Controller
    {

        private readonly IRepository<Product> _productRepository;

        public ProductController(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public ActionResult Add(WebUser user, int categoryId)
        {
            ViewBag.CategoryId = categoryId;
            return PartialView(new Product());
        }

        [HttpPost]
        public async Task<ActionResult> Add(WebUser user,Product product)
        {
            product.UserID = user.Id;
            if (ModelState.IsValid)
            {
                await _productRepository.CreateAsync(product);
                await _productRepository.SaveAsync();
                return RedirectToAction("EditableList", new { categoryId = product.CategoryID });
            }
            ViewBag.CategoryId = product.CategoryID;
            return PartialView(product);
        }

        public PartialViewResult List(int categoryId)
        {
            var products = _productRepository.GetList()
                .Where(p => p.CategoryID == categoryId)
                .ToList();
            return PartialView(products);
        }

        public ActionResult EditableList(int categoryId)
        {
            var products = _productRepository.GetList()
                .Where(p => p.CategoryID == categoryId)
                .ToList();
            return PartialView(products);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            var product = await _productRepository.GetItemAsync(id);
            await _productRepository.DeleteAsync(id);
            await _productRepository.SaveAsync();
            return RedirectToAction("EditableList", new { categoryId = product.CategoryID });
        }

        public async Task<ActionResult> Edit(WebUser user, int id)
        {
            var product = await _productRepository.GetItemAsync(id);
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
                await _productRepository.UpdateAsync(ptEdit.Product);
                await _productRepository.SaveAsync();
                return RedirectToAction("EditableList", new { categoryId = ptEdit.Product.CategoryID });
            }
            else
            {
                return PartialView(ptEdit);
            }
        }
    }
}