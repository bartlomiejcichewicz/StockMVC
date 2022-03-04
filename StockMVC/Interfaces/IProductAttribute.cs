using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StockMVC.Models;
using Bar.Tools;

namespace StockMVC.Interfaces
{
    public interface IProductAttribute
    {
        PaginatedList<ProductAttribute> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5);
        ProductAttribute GetItem(int id);
        ProductAttribute Create(ProductAttribute unit);
        ProductAttribute Edit(ProductAttribute unit);
        ProductAttribute Delete(ProductAttribute unit);
        public bool IsItemExists(string name);
        public bool IsItemExists(string name, int Id);
    }
}
