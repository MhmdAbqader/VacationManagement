using Microsoft.AspNetCore.Mvc;
using VacationManagement.Data;
using VacationManagement.Models;

namespace VacationManagement.Controllers
{
    public class VacationTypeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VacationTypeController(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }
       
        public ActionResult VacationTypes()
        {
            return View(_context.VacationTypes.ToList());
        }
              
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VacationType vacationType)
        {
         
                if (ModelState.IsValid)
                {
                    var existVacation = _context.VacationTypes
                        .FirstOrDefault(a => a.VacationName.Contains(vacationType.VacationName.Trim()) || a.VacationName.Equals(vacationType.VacationName.Trim()));
                    if (existVacation == null) 
                    {
                        _context.VacationTypes.Add(vacationType);
                        _context.SaveChanges();
                        return RedirectToAction(nameof(VacationTypes));
                    }   
                   
                }
            ViewBag.vacationTypeExist = true;
            return View(vacationType);
              
            
       
        }
       
        public ActionResult Edit(int id)
        {
            return View(_context.VacationTypes.Find(id));
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VacationType vacationType)
        {
            if (ModelState.IsValid)
            {
                _context.VacationTypes.Update(vacationType);
                _context.SaveChanges();
                return RedirectToAction(nameof(VacationTypes));
            }
            else
                return View(vacationType);
        }
        
        public ActionResult Delete(int id)
        {
            return View(_context.VacationTypes.Find(id));
        }

        // POST: DepartmentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(VacationType vacationType)
        {
            if (vacationType != null)
            {
                _context.VacationTypes.Remove(vacationType);
                _context.SaveChanges();
                return RedirectToAction(nameof(VacationTypes));
            }
            return Content("VacationType Is Not Found!");
        }
    }
}
