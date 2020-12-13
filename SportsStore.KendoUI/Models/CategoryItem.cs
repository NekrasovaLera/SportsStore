using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SportsStore.Domain.Entities;

namespace SportsStore.KendoUI.Models
{
    public class CategoryItem
    {
        public string Category { get; set; }

        public List<string> SubCategories { get; set; }
    }
}