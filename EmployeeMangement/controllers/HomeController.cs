using EmployeeMangement.Models;
using EmployeeMangement.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeMangement.controllers
{
  //  [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly Models.IEmployeeRepository m_EmployeeRepository;
        private readonly IHostingEnvironment m_HostingEnvironment;

        public HomeController(Models.IEmployeeRepository i_EmployeeRepository, IHostingEnvironment i_HostingEnvironment
                )
        {
            this.m_EmployeeRepository = i_EmployeeRepository;
            this.m_HostingEnvironment = i_HostingEnvironment;
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
            Employee employee = this.m_EmployeeRepository.GetEmployee(Id.Value);
            if (employee == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", Id);
            }
            
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                //.GetEmployee(Id??1) if id has value - use the value, if not place 1 as value
                Employee = employee,
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
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = processUploadedFile(model);

                Employee newEmployee = new Employee
                {
                    Name = model.Name,
                    Department = model.Department,
                    Email = model.Email,
                    PhotoPath = uniqueFileName
                }; 
                this.m_EmployeeRepository.addEmployee(newEmployee);
                return RedirectToAction("details", new { id = newEmployee.Id });
            }
            return View();
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            Employee employee = m_EmployeeRepository.GetEmployee(id);
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Department = employee.Department,
                Email = employee.Email,
                ExitsingPhotoPath = employee.PhotoPath
            };
            return View(employeeEditViewModel);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Employee employee = m_EmployeeRepository.GetEmployee(model.Id);
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Department = model.Department;
                if (model.Photo !=null)
                {
                    if (model.ExitsingPhotoPath != null)
                    {
                        string filePath = Path.Combine(this.m_HostingEnvironment.WebRootPath,"images", model.ExitsingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    employee.PhotoPath = processUploadedFile(model);
                }
                this.m_EmployeeRepository.Update(employee);
                return RedirectToAction("index");
            }
            return View();
        }

        private string processUploadedFile(EmployeeCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photo != null)
            {
                string uploadFolder = Path.Combine(m_HostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);

                // handeled the sipose process after finishing with the stream
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }
    }
}
