using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VacationManagement.Data;
using VacationManagement.Models;

namespace VacationManagement.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }
        public IActionResult Employees()
        {
            return View(_context.Employees.Include(a => a.Department).ToList());
        }
        public IActionResult Create() 
        {
            ViewBag.departments = _context.Departments.OrderBy(a=>a.Id).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Employees.Add(employee);
                _context.SaveChanges();
                return RedirectToAction(nameof(Employees));
            }
            ViewBag.departments = _context.Departments.OrderBy(a => a.Id).ToList();
            return View();
        }

        public IActionResult Edit(int? id)
        {
            ViewBag.departments = _context.Departments.OrderBy(a => a.Id).ToList();
            return View(_context.Employees.Find(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Employees.Update(employee);
                _context.SaveChanges();
                return RedirectToAction(nameof(Employees));
            }
            ViewBag.departments = _context.Departments.OrderBy(a => a.Id).ToList();
            return View();
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Delete(int? id) 
        {
            if (id == null || id == 0)
                return NotFound($"NO Employess with id = {id}");
            var emp = _context.Employees.Find(id);
            if (emp != null) {
                _context.Employees.Remove(emp);
                _context.SaveChanges();
                return RedirectToAction(nameof(Employees));
            }
            return NotFound($"NO Employess with id = {id}");
        }


        public IActionResult Details(int? id)
        {
            
         //   return View(_context.Employees.Include(a=>a.Department).Find(id)); // find doesn't work with INclude
            return View(_context.Employees.Include(a=>a.Department).FirstOrDefault(a=>a.Id == id)); // find doesn't work with INclude
        }

 
    }

}
