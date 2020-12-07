using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SportsStore.Domain.Entities;

namespace SportsStore.Domain.Concrete
{
    public class ListViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
    }
}