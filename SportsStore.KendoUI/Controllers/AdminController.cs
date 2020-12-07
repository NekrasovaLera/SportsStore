using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.KendoUI.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace SportsStore.KendoUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private IProductRepository repository;

        public AdminController(IProductRepository repository)
        {
            this.repository = repository;
        }

        [Authorize]
        public ViewResult Index()
        {
            var categories = repository.Categories.Select(e => new
            {
                CatID = e.CatID,
                CatName = e.CatName
            })
                .OrderBy(e => e.CatName);
            ViewData["categories"] = categories;
            ViewData["defaultCategory"] = categories.First();
            return View();
        }

        public ActionResult ReadCategories([DataSourceRequest] DataSourceRequest request)
        {
            var result = repository.Categories.ToList();
            return Json(result.ToDataSourceResult(request));
        }

        public IList<ProductViewModel> GetProducts()
        {
            var result = repository.Products.Select(product => new ProductViewModel
            {
                ProductID = product.ProductID,
                Name = product.Name,
                Description = product.Description,
                Category = product.Category,
                Price = product.Price,
                ImageData = product.ImageData,
                ImageMimeType = product.ImageMimeType
            }).ToList();
            return result;
        }

        public FileContentResult GetImage(int productId)
        {
            Product prod = repository.Products.FirstOrDefault(p => p.ProductID == productId);
            if (prod != null)
            {
                if (prod.ImageData != null)
                {
                    return File(prod.ImageData, prod.ImageMimeType);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public ActionResult ReadProducts([DataSourceRequest] DataSourceRequest request)
        {
            var result = GetProducts();
            return Json(result.ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditCategory([DataSourceRequest] DataSourceRequest request, Category category)
        {
            if (category != null && ModelState.IsValid)
            {
                var target = repository.Categories.FirstOrDefault(e => e.CatID == category.CatID);
                target.CatID = category.CatID;
                target.CatName = category.CatName;
                repository.SaveCategory(target);
            }

            return Json(new[] { category }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateCategory([DataSourceRequest] DataSourceRequest request, Category category)
        {
            if (category != null && ModelState.IsValid)
            {
                var entity = new Category {
                    CatID = category.CatID,
                    CatName = category.CatName
                };
                repository.SaveCategory(entity);
                category.CatID = entity.CatID;
            }

            return Json(new[] { category }.ToDataSourceResult(request, ModelState));
        }

        public Category GetCategory(string catName)
        {
            var result = repository.Categories.FirstOrDefault(e => e.CatName == catName);
            if (result != null)
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateProduct([DataSourceRequest] DataSourceRequest request, ProductViewModel product)
        {
            if (product != null && ModelState.IsValid)
            {
                var entity = new Product
                {
                    ProductID = product.ProductID,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Category = GetCategory(product.Category.CatName),
                    CatID = GetCategory(product.Category.CatName).CatID
                };
                repository.SaveProduct(entity);
                product.ProductID = entity.ProductID;
            }

            return Json(new[] { product }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteCategory([DataSourceRequest] DataSourceRequest request, Category category)
        {
            if (category != null)
            {
                repository.DeleteCategory(category.CatID);
            }

            return Json(new[] { category }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult SaveImage(HttpPostedFileBase image)
        {
            return View();
        }

        public ActionResult RemoveImage(HttpPostedFileBase image)
        {
            return View();
        }
    }
}