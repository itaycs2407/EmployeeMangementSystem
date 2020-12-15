using EmployeeMangement.Models;
using EmployeeMangement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeMangement.controllers
{
  //  [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly Models.IEmployeeRepository m_EmployeeRepository;
        public HomeController(Models.IEmployeeRepository i_EmployeeRepository)
        {
            this.m_EmployeeRepository = i_EmployeeRepository;
        }

        // attribute routing
       // [Route("")]// default route
      //  [Route("~/")] 
        public ViewResult Index()
        {
            //return m_EmployeeRepository.GetEmployee(2).Name;

            var model = this.m_EmployeeRepository.GetAllEmployee();
            return View(model);
        }
       // [Route("{id?}")]
        public ViewResult Details(int? Id)
        {
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                //.GetEmployee(Id??1) if id has value - use the value, if not place 1 as value
                Employee = this.m_EmployeeRepository.GetEmployee(Id??1),
                PageTitle = "employee details"
            };
            /*
            //ViewData example:
            ViewData["Employee"] = model;
            ViewData["pageTitle"] = "employee details";
            */

            /*
            //ViewBag example:
            ViewBag.Employee = model;
            ViewBag.pageTitle = "employee details";
            */
            return View(homeDetailsViewModel);
        }
        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                this.m_EmployeeRepository.addEmployee(employee);
                return RedirectToAction("details", new { id = employee.Id });
            }
            return View();
        }
    }
}
