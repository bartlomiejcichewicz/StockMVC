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
    public class UnitController : Controller
    {

        private readonly IUnit _unitRepo;
        public UnitController(IUnit unitrepo)
        {
            _unitRepo = unitrepo;
        }
        public IActionResult Index(string sortExpression = "", string SearchText = "", int pg = 1, int pageSize = 5)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("name");
            sortModel.AddColumn("description");
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;
            ViewBag.SearchText = SearchText;
            PaginatedList<Unit> units = _unitRepo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, SearchText, pg, pageSize);
            var pager = new PagerModel(units.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;
            TempData["CurrentPage"] = pg;
            return View(units);
        }
        public IActionResult Create()
        {
            Unit unit = new Unit();
            return View(unit);
        }
        [HttpPost]
        public IActionResult Create(Unit unit)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
                if (unit.Description.Length < 4 || unit.Description == null)
                    errMessage = "Unit Description Must be atleast 4 Characters";
                if (_unitRepo.IsUnitNameExists(unit.Name) == true)
                    errMessage = errMessage + " " + " Unit Name " + unit.Name + " Exists Already";
                if (errMessage == "")
                {
                    unit = _unitRepo.Create(unit);
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
                return View(unit);
            }
            else
            {
                TempData["SuccessMessage"] = "" + unit.Name + " has been added to the database!";
                return RedirectToAction(nameof(Index));
            }
        }
        public IActionResult Details(int id)
        {
            Unit unit = _unitRepo.GetUnit(id);
            return View(unit);
        }
        public IActionResult Edit(int id)
        {
            Unit unit = _unitRepo.GetUnit(id);
            TempData.Keep();
            return View(unit);
        }
        [HttpPost]
        public IActionResult Edit(Unit unit)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
                if (unit.Description.Length < 5 || unit.Description == null)
                    errMessage = "Unit description must be at least 5 characters";
                if (_unitRepo.IsUnitNameExists(unit.Name, unit.Id) == true)
                    errMessage = errMessage + "Unit Name " + unit.Name + " already exists!";
                if (errMessage == "")
                {
                    unit = _unitRepo.Edit(unit);
                    TempData["SuccessMessage"] = "Changes in " + unit.Name + " has been saved!";
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
                return View(unit);
            }
            else
                return RedirectToAction(nameof(Index), new { pg = currentPage });
        }
        public IActionResult Delete(int id)
        {
            Unit unit = _unitRepo.GetUnit(id);
            TempData.Keep();
            return View(unit);
        }
        [HttpPost]
        public IActionResult Delete(Unit unit)
        {
            try
            {
                unit = _unitRepo.Delete(unit);
            }
            catch (Exception ex)
            {
                string errMessage = ex.Message;
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(unit);
            }
            int currentPage = 1;
            if (TempData["CurrentPage"] != null)
                currentPage = (int)TempData["CurrentPage"];
            TempData["SuccessMessage"] = "Deleted succesfully!";
            return RedirectToAction(nameof(Index), new { pg = currentPage });
        }
    }
}