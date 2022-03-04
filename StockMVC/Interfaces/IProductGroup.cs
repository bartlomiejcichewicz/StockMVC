using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StockMVC.Models;
using Bar.Tools;

namespace StockMVC.Interfaces
{
    public interface IProductGroup
    {
        PaginatedList<ProductGroup> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5);
        ProductGroup GetItem(int id);
        ProductGroup Create(ProductGroup unit);
        ProductGroup Edit(ProductGroup unit);
        ProductGroup Delete(ProductGroup unit);
        public bool IsItemExists(string name);
        public bool IsItemExists(string name, int Id);
    }
}
