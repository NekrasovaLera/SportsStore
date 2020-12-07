﻿using SportsStore.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.KendoUI.Controllers
{
    public class NavController : Controller
    {
        private IProductRepository repository;

        public NavController(IProductRepository repo)
        {
            repository = repo;
        }

        public PartialViewResult Menu(int? category = null)
        {
            ViewBag.SelectedCategory = category;
            IEnumerable<string> categories = repository.Categories
                .Select(x => x.CatName)
                .Distinct()
                .OrderBy(x => x);
            return PartialView(categories);
        }
    }
}