using EmpMgmt.Data;
using EmpMgmt.Models;
using EmpMgmt.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmpMgmt.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly EmDbContext emDbContext;
        Random rnd = new Random();
        public EmployeesController(EmDbContext emDbContext)
        {
            this.emDbContext = emDbContext;
        }

        [HttpGet]

        public async Task<IActionResult> Index()
        {
            var employees = await emDbContext.Employees.ToListAsync();
            return View(employees);
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel req)
        {
            var employee = new Employee()
            {
                Name = req.Name,
                Email = req.Email,
                Salary = req.Salary,
                Department = req.Department,
                DateOfBirth = req.DateOfBirth
            }; 
            //adding employee to the Db context
            await emDbContext.Employees.AddAsync(employee);

            //save changes to database
            await emDbContext.SaveChangesAsync();

            //redirect to the view
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> View(int id)
        {
            var employee = await emDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);

            if(employee != null)
            {
                var viewModel = new UpdateEmployeeViewModel()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    Department = employee.Department,
                    DateOfBirth = employee.DateOfBirth
                };

                return await Task.Run(()=> View("View",viewModel));
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        
        public async Task<IActionResult> View(UpdateEmployeeViewModel model)
        {
            var employee = await emDbContext.Employees.FindAsync(model.Id);

            if(employee != null)
            {
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Salary = model.Salary;
                employee.Department = model.Department;
                employee.DateOfBirth = model.DateOfBirth;

                await emDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]

        public async Task<IActionResult> Delete(UpdateEmployeeViewModel model)
        {
            var employee = await emDbContext.Employees.FindAsync(model.Id);

            if(employee != null)
            {
                emDbContext.Employees.Remove(employee);

                await emDbContext.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            return RedirectToAction("Index");

        }
    }
}
