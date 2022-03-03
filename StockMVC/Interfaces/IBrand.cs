﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StockMVC.Models;
using Bar.Tools;

namespace StockMVC.Interfaces
{
    public interface IBrand
    {
        PaginatedList<Brand> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5);
        Brand GetItem(int id);
        Brand Create(Brand brand);
        Brand Edit(Brand brand);
        Brand Delete(Brand brand);
        public bool IsItemExists(string name);
        public bool IsItemExists(string name, int Id);

    }
}