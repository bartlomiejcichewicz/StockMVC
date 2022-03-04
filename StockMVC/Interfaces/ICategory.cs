using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StockMVC.Models;
using Bar.Tools;

namespace StockMVC.Interfaces
{
    public interface ICategory
    {
        PaginatedList<Category> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5);
        Category GetItem(int id);
        Category Create(Category unit);
        Category Edit(Category unit);
        Category Delete(Category unit);
        public bool IsItemExists(string name);
        public bool IsItemExists(string name, int Id);
    }
}
