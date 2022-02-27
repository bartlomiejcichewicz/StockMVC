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

namespace StockMVC.Controllers
{
    public class UnitController : Controller
    {

        public IActionResult Index()
        {
            List<Unit> units = _unitRepo.GetItems();
            return View(units);
        }
        private readonly IUnit _unitRepo;
        public UnitController(IUnit unitrepo)
        {
            _unitRepo = unitrepo;
        }
        public IActionResult Create()
        {
            Unit unit = new Unit();
            return View(unit);
        }
        [HttpPost]
        public IActionResult Create(Unit unit)
        {
            try
            {
                unit = _unitRepo.Create(unit);
            }
            catch
            {

            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Details(int id)
        {
            Unit unit = _unitRepo.GetUnit(id);
            return View(unit);
        }
        public IActionResult Edit(int id)
        {
            Unit unit = _unitRepo.GetUnit(id);
            return View(unit);
        }
        [HttpPost]
        public IActionResult Edit(Unit unit)
        {
            try
            {
                unit = _unitRepo.Edit(unit);
            }
            catch
            {

            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete (int id)
        {
            Unit unit = _unitRepo.GetUnit(id);
            return View(unit);
        }
        [HttpPost]
        public IActionResult Delete(Unit unit)
        {
            try
            {
                unit = _unitRepo.Delete(unit);
            }
            catch
            {

            }
            return RedirectToAction(nameof(Index));
        }
    }
}
