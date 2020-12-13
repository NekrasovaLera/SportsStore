using SportsStore.Domain.Abstract;
using SportsStore.KendoUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;

namespace SportsStore.KendoUI.Controllers
{
    public class NavController : Controller
    {
        private IProductRepository repository;

        public NavController(IProductRepository repo)
        {
            repository = repo;
        }

        public PartialViewResult ShowCategories()
        {
            ViewBag.inline = GetData();
            return PartialView();
        }

        private IEnumerable<CategoryItem> GetData()
        {
            List<string> items = repository.Categories.Select(e => e.CatName).ToList();
            List <CategoryItem> inline = new List<CategoryItem>
            {
                new CategoryItem
                {
                    Category = "All categories",
                    SubCategories = items
                }
            };

            return inline;
        }
    }
}