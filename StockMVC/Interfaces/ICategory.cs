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
        Category Create(Category category);
        Category Edit(Category category);
        Category Delete(Category category);
        public bool IsItemExists(string name);
        public bool IsItemExists(string name, int Id);

    }
}
