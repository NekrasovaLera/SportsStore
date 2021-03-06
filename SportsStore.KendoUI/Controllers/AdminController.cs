﻿using SportsStore.Domain.Abstract;
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
            PopulateCategories();
            PopulateImages();
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
                CatID = product.CatID,
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
        public ActionResult EditProduct([DataSourceRequest] DataSourceRequest request, ProductViewModel product)
        {
            if (product != null && ModelState.IsValid)
            {
                var target = repository.Products.FirstOrDefault(e => e.ProductID == product.ProductID);
                target.Name = product.Name;
                target.Description = product.Description;
                target.Price = product.Price;
                target.CatID = product.CatID;
                repository.SaveProduct(target);
            }

            return Json(new[] { product }.ToDataSourceResult(request, ModelState));
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
                var entity = new Product();
                entity.ProductID = product.ProductID;
                entity.Name = product.Name;
                entity.Description = product.Description;
                entity.Price = product.Price;
                entity.CatID = product.CatID;
                if (entity.CatID == 0)
                {
                    entity.CatID = 1;
                }

                if (product.Category != null)
                {
                    entity.CatID = product.Category.CatID;
                }
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

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteProduct([DataSourceRequest] DataSourceRequest request, Product product)
        {
            if (product != null)
            {
                repository.DeleteProduct(product.ProductID);
            }

            return Json(new[] { product }.ToDataSourceResult(request, ModelState));
        }

        public void PopulateCategories()
        {
            var categories = repository.Categories.ToList().OrderBy(e => e.CatName);
            ViewData["categories"] = categories;
        }

        public void PopulateImages()
        {
            var imgs = repository.Products.Select(i => new
            {
                ProductID = i.ProductID,
                ImageData = i.ImageData,
                ImageMimeType = i.ImageMimeType
            }).ToList();
            ViewData["imgs"] = imgs;
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