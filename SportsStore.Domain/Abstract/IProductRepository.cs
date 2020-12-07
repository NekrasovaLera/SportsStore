using System;
using System.Web;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsStore.Domain.Entities;

namespace SportsStore.Domain.Abstract
{
    public interface IProductRepository
    {
        IQueryable<Product> Products { get; }
        IQueryable<Category> Categories { get; }
        void SaveProduct(Product product);
        void SaveCategory(Category category);
        Product DeleteProduct(int productID);
        Category DeleteCategory(int CatID);
    }
}
