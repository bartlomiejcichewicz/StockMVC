using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StockMVC.Data;
using StockMVC.Interfaces;
using StockMVC.Models;

namespace StockMVC.Repositories
{
    public class UnitRepository : IUnit
    {
        private readonly StockContext _context;
        public UnitRepository(StockContext context)
        {
            _context = context;
        }

        public Unit Create(Unit unit)
        {
            _context.Units.Add(unit);
            _context.SaveChanges();
            return unit;
        }

        public Unit Delete(Unit unit)
        {
            _context.Units.Attach(unit);
            _context.Entry(unit).State = EntityState.Deleted;
            _context.SaveChanges();
            return unit;
        }

        public Unit Edit(Unit unit)
        {
            _context.Units.Attach(unit);
            _context.Entry(unit).State = EntityState.Modified;
            _context.SaveChanges();
            return unit;
        }

        public List<Unit> GetItems()
        {
            List<Unit> units = _context.Units.ToList();
            return units;
        }

        public Unit GetUnit(int id)
        {
            Unit unit = _context.Units.Where(u => u.Id == id).FirstOrDefault();
            return unit;
        }
    }
}
