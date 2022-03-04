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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace StockMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnit _unitRepo;
        private readonly IProduct _productRepo;
        public ProductController(IProduct productrepo, IUnit unitrepo)
        {
            _productRepo = productrepo;
            _unitRepo = unitrepo;
        }
        public IActionResult Index(string sortExpression = "", string SearchText = "", int pg = 1, int pageSize = 5)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("Code");
            sortModel.AddColumn("name");
            sortModel.AddColumn("description");
            sortModel.AddColumn("Cost");
            sortModel.AddColumn("Price");
            sortModel.AddColumn("Unit");
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;
            ViewBag.SearchText = SearchText;
            PaginatedList<Product> products = _productRepo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, SearchText, pg, pageSize);
            var pager = new PagerModel(products.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;
            TempData["CurrentPage"] = pg;
            return View(products);
        }
        public IActionResult Create()
        {
            Product product = new Product();
            ViewBag.Units = GetUnits();
            return View(product);
        }
        [HttpPost]
        public IActionResult Create(Product product)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
                if (product.Description.Length < 5 || product.Description == null)
                    errMessage = "Product description must be at least 5 characters";
                if (_productRepo.IsItemExists(product.Name) == true)
                    errMessage = errMessage + " " + " Product name " + product.Name + " already exists!";
                if (errMessage == "")
                {
                    product = _productRepo.Create(product);
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
                return View(product);
            }
            else
            {
                TempData["SuccessMessage"] = "" + product.Name + " has been added to the database!";
                return RedirectToAction(nameof(Index));
            }
        }
        public IActionResult Details(string id)
        {
            Product product = _productRepo.GetItem(id);
            return View(product);
        }
        public IActionResult Edit(string id)
        {
            Product product = _productRepo.GetItem(id);
            ViewBag.Units = GetUnits();
            TempData.Keep();
            return View(product);
        }
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
                if (product.Description.Length < 5 || product.Description == null)
                    errMessage = "Product description must be at least 5 characters!";
                if (_productRepo.IsItemExists(product.Name, product.Code) == true)
                    errMessage = errMessage + "Product Name " + product.Name + " already exists!";
                if (errMessage == "")
                {
                    product = _productRepo.Edit(product);
                    TempData["SuccessMessage"] = "Changes in " + product.Name + " has been saved!";
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
                return View(product);
            }
            else
                return RedirectToAction(nameof(Index), new { pg = currentPage });
        }
        public IActionResult Delete(string id)
        {
            Product product = _productRepo.GetItem(id);
            TempData.Keep();
            return View(product);
        }
        [HttpPost]
        public IActionResult Delete(Product product)
        {
            try
            {
                product = _productRepo.Delete(product);
            }
            catch (Exception ex)
            {
                string errMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errMessage = ex.InnerException.Message;
                }
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(product);
            }
            int currentPage = 1;
            if (TempData["CurrentPage"] != null)
                currentPage = (int)TempData["CurrentPage"];
            TempData["SuccessMessage"] = "Deleted succesfully!";
            return RedirectToAction(nameof(Index), new { pg = currentPage });
        }
        private List<SelectListItem> GetUnits()
        {
            var lstUnits = new List<SelectListItem>();
            PaginatedList<Unit> units = _unitRepo.GetItems("Name", SortOrder.Ascending,"",1,1000);
            lstUnits = units.Select(ut => new SelectListItem()
            {
                Value = ut.Id.ToString(),
                Text = ut.Name
            }).ToList();
            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "----Select Unit----"
            };
            lstUnits.Insert(0, defItem);
            return lstUnits;
        }
    }
}