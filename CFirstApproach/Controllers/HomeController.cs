using System.Diagnostics;
using CFirstApproach.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CFirstApproach.Controllers
{
    public class HomeController : Controller
    {
        private readonly EmployeeDBContext employeeDB;

        public HomeController(EmployeeDBContext employeeDB)
        {
            this.employeeDB = employeeDB;
        }

        public async Task<IActionResult> Index()
        {
            var empData = await employeeDB.Employees.ToListAsync();
            return View(empData);
        }

        //create method for Creating data
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Employee emp)
        {
            if (ModelState.IsValid)
            {
                await employeeDB.Employees.AddAsync(emp);
                await employeeDB.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View(emp);
        }


        //create method for Editing data
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var empData = await employeeDB.Employees.FindAsync(id);
            if(empData == null)
            {
                return NotFound();
            }
            return View(empData);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int? id, Employee editEmp)
        {
            if(id != editEmp.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                //It update the employee data in the database
                employeeDB.Entry(editEmp).State = EntityState.Modified;
                await employeeDB.SaveChangesAsync();

                //redirect to the details action 
                return RedirectToAction("Index", "Home");

            }
            return View(editEmp);
        }

        //create method for Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || employeeDB.Employees == null)
            {
                return NotFound();
            }
            var empData = await employeeDB.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if(empData == null)
            {
                return NotFound();
            }
            return View(empData);
        }

        //create method for Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || employeeDB.Employees == null)
            {
                return NotFound();
            }
            var empData = await employeeDB.Employees.FindAsync(id);
            if (empData == null)
            {
                return NotFound();
            }
            return View(empData);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var empData = await employeeDB.Employees.FindAsync(id);
            if (empData == null)
            {
                return NotFound();
            }

            employeeDB.Employees.Remove(empData);
            await employeeDB.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
