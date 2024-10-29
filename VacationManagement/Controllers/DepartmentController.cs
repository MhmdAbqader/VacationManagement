using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VacationManagement.Data;
using VacationManagement.Models;

namespace VacationManagement.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepartmentController(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }
        // GET: DepartmentController
        public ActionResult Departments()
        {
            return View(_context.Departments.ToList());
        }

 

        // GET: DepartmentController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DepartmentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Department department)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Departments.Add(department);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Departments));
                }
                else
                {
                    return View(department);
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: DepartmentController/Edit/5
        public ActionResult Edit(int id)
        {
            return View(_context.Departments.Find(id));
        }

        // POST: DepartmentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Departments.Update(department);
                _context.SaveChanges();
                return RedirectToAction(nameof(Departments));
            }
            else
                return View(department);
        }

        // GET: DepartmentController/Delete/5
        public ActionResult Delete(int id)
        {
            return View(_context.Departments.Find(id));
        }

        // POST: DepartmentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Department department)
        {
            if (department != null) 
            {
                _context.Departments.Remove(department); 
                _context.SaveChanges();
                return RedirectToAction(nameof(Departments));
            }   
            return Content("Department Is Not Found!");
        }

        // GET: DepartmentController/Details/5
        public ActionResult Details(int id)
        {

            return View(_context.Departments.Find(id));
        }

    }
}
