using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace SportsStore.KendoUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private IProductRepository repository;

        public AdminController(IProductRepository repo)
        {
            repository = repo;
        }

        [Authorize]
        public ViewResult Index()
        {
            return View(repository.Categories);
        }

        public ActionResult ReadCategories([DataSourceRequest] DataSourceRequest request)
        {
            var result = repository.Categories.Select(x => x.CatName);
            return Json(result.ToList().ToDataSourceResult(request));
        }

        public ViewResult Edit(int productId)
        {
            Product product = repository.Products
                .FirstOrDefault(p => p.ProductID == productId);
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    product.ImageMimeType = image.ContentType;
                    product.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(product.ImageData, 0, image.ContentLength);
                }
                repository.SaveProduct(product);
                TempData["message"] = string.Format("{0} has been saved", product.Name);
                return RedirectToAction("Index");
            }
            else
            {
                return View(product);
            }
        }

        public ViewResult Create()
        {
            return View("Edit", new Product());
        }

        [HttpPost]
        public ActionResult Delete(int productId)
        {
            Product deletedProduct = repository.DeleteProduct(productId);
            if (deletedProduct != null)
            {
                TempData["message"] = string.Format("{0} was deleted", deletedProduct.Name);
            }

            return RedirectToAction("Index");
        }

        public ViewResult EditCategory(int CatId)
        {
            Category category = repository.Categories
                .FirstOrDefault(p => p.CatID == CatId);
            return View(category);
        }

        [HttpPost]
        public ActionResult EditCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                repository.SaveCategory(category);
                TempData["message"] = string.Format("{0} has been saved", category.CatName);
                return RedirectToAction("Index");
            }
            else
            {
                return View(category);
            }
        }

        public ViewResult CreateCategory()
        {
            return View("EditCategory", new Category());
        }

        [HttpPost]
        public ActionResult DeleteCategory(int CatId)
        {
            Category deletedCategory = repository.DeleteCategory(CatId);
            if (deletedCategory != null)
            {
                TempData["message"] = string.Format("{0} was deleted", deletedCategory.CatName);
            }

            return RedirectToAction("Index");
        }
    }
}