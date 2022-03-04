using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StockMVC.Models;
using Bar.Tools;

namespace StockMVC.Interfaces
{
    public interface IProductProfile
    {
        PaginatedList<ProductProfile> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5);
        ProductProfile GetItem(int id);
        ProductProfile Create(ProductProfile unit);
        ProductProfile Edit(ProductProfile unit);
        ProductProfile Delete(ProductProfile unit);
        public bool IsItemExists(string name);
        public bool IsItemExists(string name, int Id);
    }
}
