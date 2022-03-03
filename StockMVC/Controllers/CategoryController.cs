using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StockMVC.Data;
using StockMVC.Interfaces;
using StockMVC.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Bar.Tools;

namespace StockMVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategory _Repo;
        public CategoryController(ICategory repo) // repozytorium przekazane w dependency injection
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
            PaginatedList<Category> items = _Repo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, SearchText, pg, pageSize);
            var pager = new PagerModel(items.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;
            TempData["CurrentPage"] = pg;
            return View(items);
        }
        public IActionResult Create()
        {
            Category item = new Category();
            return View(item);
        }
        [HttpPost]
        public IActionResult Create(Category item)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
                if (item.Description.Length < 5 || item.Description == null)
                {
                    errMessage = "Product description must be at least 5 characters!";
                }
                if (_Repo.IsItemExists(item.Name) == true)
                {
                    errMessage = errMessage + " " + " Product name " + item.Name + "already exists!";
                }
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
                TempData["SuccessMessage"] = "Product " + item.Name + " has been added to the database!";
                return RedirectToAction(nameof(Index));
            }
        }
        public IActionResult Details(int id)
        {
            Category item = _Repo.GetItem(id);
            return View(item);
        }
        public IActionResult Edit(int id)
        {
            Category item = _Repo.GetItem(id);
            TempData.Keep();
            return View(item);
        }
        [HttpPost]
        public IActionResult Edit(Category item)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
                if (item.Description.Length < 5 || item.Description == null)
                {
                    errMessage = "Product description must be at least 5 characters!";
                }
                if (_Repo.IsItemExists(item.Name, item.Id) == true)
                {
                    errMessage = errMessage + " " + " Product name " + item.Name + "already exists!";
                }
                if (errMessage == "")
                {
                    item = _Repo.Edit(item);
                    TempData["SuccesMessage"] = "Product " + item.Name + " has been edited!";
                    bolret = true;
                }
            }
            catch (Exception ex)
            {
                errMessage = errMessage + " " + ex.Message;
            }
            int currentPage = 1;
            if (TempData["CurrentPage"] != null)
            {
                currentPage = (int)TempData["CurrentPage"];
            }
            if (bolret == false)
            {
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(item);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        public IActionResult Delete(int id)
        {
            Category item = _Repo.GetItem(id);
            TempData.Keep();
            return View(item);
        }
        [HttpPost]
        public IActionResult Delete(Category item)
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
            TempData["SuccessMessage"] = "Product " + item.Name + " has been deleted!";
            return RedirectToAction(nameof(Index), new { pg = currentPage });
        }
    }
}