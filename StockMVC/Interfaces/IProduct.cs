using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StockMVC.Models;
using Bar.Tools;

namespace StockMVC.Interfaces
{
    public interface IProduct
    {
        PaginatedList<Product> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5);
        Product GetItem(string Code);
        Product Create(Product product);
        Product Edit(Product product);
        Product Delete(Product product);
        public bool IsItemExists(string name);
        public bool IsItemExists(string name, string Code);
    }
}
