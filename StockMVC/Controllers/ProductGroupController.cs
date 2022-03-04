﻿using Bar.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockMVC.Interfaces;
using StockMVC.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace StockMVC.Controllers
{
    public class ProductGroupController : Controller
    {

        private readonly IProductGroup _Repo;
        public ProductGroupController(IProductGroup repo)
        {
            _Repo = repo;
        }
        public IActionResult Index(string sortExpression = "", string SearchText = "", int pg = 1, int pageSize = 5)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("name");
            sortModel.AddColumn("description");
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;
            ViewBag.SearchText = SearchText;
            PaginatedList<ProductGroup> items = _Repo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, SearchText, pg, pageSize);
            var pager = new PagerModel(items.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;
            TempData["CurrentPage"] = pg;
            return View(items);
        }
        public IActionResult Create()
        {
            ProductGroup item = new ProductGroup();
            return View(item);
        }
        [HttpPost]
        public IActionResult Create(ProductGroup item)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
                if (item.Description.Length < 5 || item.Description == null)
                    errMessage = "Description must be at least 5 characters!";
                if (_Repo.IsItemExists(item.Name) == true)
                    errMessage = errMessage + " " + " Name " + item.Name + " already exists!";
                if (errMessage == "")
                {
                    item = _Repo.Create(item);
                    bolret = true;
                }
            }
            catch (Exception ex)
            {
                errMessage = errMessage + " " + ex.Message;
            }
            if (bolret == false)
            {
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(item);
            }
            else
            {
                TempData["SuccessMessage"] = "" + item.Name + " has been added to the database!";
                return RedirectToAction(nameof(Index));
            }
        }
        public IActionResult Details(int id)
        {
            ProductGroup item = _Repo.GetItem(id);
            return View(item);
        }
        public IActionResult Edit(int id)
        {
            ProductGroup item = _Repo.GetItem(id);
            TempData.Keep();
            return View(item);
        }
        [HttpPost]
        public IActionResult Edit(ProductGroup item)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
                if (item.Description.Length < 5 || item.Description == null)
                    errMessage = "Description must be at least 5 characters!";
                if (_Repo.IsItemExists(item.Name, item.Id) == true)
                    errMessage = errMessage + item.Name + " qlready exists!";
                if (errMessage == "")
                {
                    item = _Repo.Edit(item);
                    TempData["SuccessMessage"] = "Changes in " + item.Name + " has been saved!";
                    bolret = true;
                }
            }
            catch (Exception ex)
            {
                errMessage = errMessage + " " + ex.Message;
            }
            int currentPage = 1;
            if (TempData["CurrentPage"] != null)
                currentPage = (int)TempData["CurrentPage"];
            if (bolret == false)
            {
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(item);
            }
            else
                return RedirectToAction(nameof(Index), new { pg = currentPage });
        }
        public IActionResult Delete(int id)
        {
            ProductGroup item = _Repo.GetItem(id);
            TempData.Keep();
            return View(item);
        }
        [HttpPost]
        public IActionResult Delete(ProductGroup item)
        {
            try
            {
                item = _Repo.Delete(item);
            }
            catch (Exception ex)
            {
                string errMessage = ex.Message;
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(item);
            }
            int currentPage = 1;
            if (TempData["CurrentPage"] != null)
                currentPage = (int)TempData["CurrentPage"];

            TempData["SuccessMessage"] = "Deleted succesfully!";
            return RedirectToAction(nameof(Index), new { pg = currentPage });
        }
    }
}
