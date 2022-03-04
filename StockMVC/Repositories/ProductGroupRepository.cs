using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StockMVC.Data;
using StockMVC.Interfaces;
using StockMVC.Models;
using Bar.Tools;

namespace StockMVC.Repositories
{
    public class ProductGroupRepository : IProductGroup
    {
        private readonly StockContext _context; // for connecting to efcore.
        public ProductGroupRepository(StockContext context) // will be passed by dependency injection.
        {
            _context = context;
        }
        public ProductGroup Create(ProductGroup ProductGroup)
        {
            _context.ProductGroups.Add(ProductGroup);
            _context.SaveChanges();
            return ProductGroup;
        }
        public ProductGroup Delete(ProductGroup ProductGroup)
        {
            _context.ProductGroups.Attach(ProductGroup);
            _context.Entry(ProductGroup).State = EntityState.Deleted;
            _context.SaveChanges();
            return ProductGroup;
        }
        public ProductGroup Edit(ProductGroup ProductGroup)
        {
            _context.ProductGroups.Attach(ProductGroup);
            _context.Entry(ProductGroup).State = EntityState.Modified;
            _context.SaveChanges();
            return ProductGroup;
        }
        private List<ProductGroup> DoSort(List<ProductGroup> items, string SortProperty, SortOrder sortOrder)
        {

            if (SortProperty.ToLower() == "name")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.Name).ToList();
                else
                    items = items.OrderByDescending(n => n.Name).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(d => d.Description).ToList();
                else
                    items = items.OrderByDescending(d => d.Description).ToList();
            }
            return items;
        }
        public PaginatedList<ProductGroup> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<ProductGroup> items;

            if (SearchText != "" && SearchText != null)
            {
                items = _context.ProductGroups.Where(n => n.Name.Contains(SearchText) || n.Description.Contains(SearchText))
                    .ToList();
            }
            else
                items = _context.ProductGroups.ToList();
            items = DoSort(items, SortProperty, sortOrder);
            PaginatedList<ProductGroup> retItems = new PaginatedList<ProductGroup>(items, pageIndex, pageSize);
            return retItems;
        }
        public ProductGroup GetItem(int id)
        {
            ProductGroup item = _context.ProductGroups.Where(u => u.Id == id).FirstOrDefault();
            return item;
        }
        public bool IsItemExists(string name)
        {
            int ct = _context.ProductGroups.Where(n => n.Name.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
        public bool IsItemExists(string name, int Id)
        {
            int ct = _context.ProductGroups.Where(n => n.Name.ToLower() == name.ToLower() && n.Id != Id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
    }
}
