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
    public class ProductRepository : IProduct
    {
        private readonly StockContext _context;
        public ProductRepository(StockContext context)
        {
            _context = context;
        }
        public Product Create(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return product;
        }
        public Product Delete(Product Product)
        {
            Product = pGetItem(Product.Code);
            _context.Products.Attach(Product);
            _context.Entry(Product).State = EntityState.Deleted;
            _context.SaveChanges();
            return Product;
        }
        public Product Edit(Product product)
        {
            _context.Products.Attach(product);
            _context.Entry(product).State = EntityState.Modified;
            _context.SaveChanges();
            return product;
        }
        private List<Product> DoSort(List<Product> items, string SortProperty, SortOrder sortOrder)
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
        public PaginatedList<Product> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<Product> items;
            if (SearchText != "" && SearchText != null)
            {
                items = _context.Products.Where(n => n.Name.Contains(SearchText) || n.Description.Contains(SearchText))
                    .Include(u => u.Units)
                    .ToList();
            }
            else
                items = _context.Products.Include(u => u.Units).ToList();
            items = DoSort(items, SortProperty, sortOrder);
            PaginatedList<Product> retItems = new PaginatedList<Product>(items, pageIndex, pageSize);
            return retItems;
        }
        public Product GetItem(string Code)
        {
            Product item = _context.Products.Where(u => u.Code == Code)
                .Include(u => u.Units)
                .FirstOrDefault();
            return item;
        }
        public Product pGetItem(string Code)
        {
            Product item = _context.Products.Where(u => u.Code == Code)
                .FirstOrDefault();
            return item;
        }
        public bool IsItemExists(string name)
        {
            int ct = _context.Products.Where(n => n.Name.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
        public bool IsItemExists(string name, string Code)
        {
            int ct = _context.Products.Where(n => n.Name.ToLower() == name.ToLower() && n.Code != Code).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
    }
}