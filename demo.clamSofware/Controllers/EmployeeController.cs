using demo.clamSofware.Interface;
using demo.clamSofware.Models;
using demo.clamSofware.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace demo.clamSofware.Controllers
{

    [AllowAnonymous]
    public class EmployeeController : Controller
    {
        private readonly IEmployeesRepository _employeesRepository;

        private readonly ILogger logger;

        private readonly IDataProtector dataProtector;

        private readonly IWebHostEnvironment hostingEnvironment;
        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public EmployeeController(IEmployeesRepository employeesRepository, IWebHostEnvironment hostingEnvironment,
            ILogger<EmployeeController> logger, DataProtectionPurposeStrings dataProtectionPurposeStrings, IDataProtectionProvider dataProtectionProvider)
        {
            _employeesRepository = employeesRepository;
            this.hostingEnvironment = hostingEnvironment;
            this.logger = logger;
            dataProtector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.StudentIdRouteValue);

        }





        public IActionResult Index()
        {
            IEnumerable<Employees> students = _employeesRepository.GetAllEmployees().
                Select(s =>
                {

                    s.EncryptedId = dataProtector.Protect(s.EmployeeId.ToString());
                    return s;
                });

            return View(students);

        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpGet]
        public ViewResult CreateRoles()
        {
            
            var fakeRole = new List<FakeData>();
            var data = new FakeData();
            for(var i=0; i<10; i++)
            {
                data.Id = Guid.NewGuid();
                data.Name = RandomString(10);
                data.rule = "ADMIN";
                fakeRole.Add(data);
            }
           
   
            return View(fakeRole);
        }

        [HttpPost]
        public IActionResult CreateRolse(List<Rules> model)
        {
            var filterChecked = model.Where(item=> item.isChecked.Equals(true) && !!item.isChecked).ToList();
            var debugger = filterChecked;
            Console.WriteLine(model);
            //someAnyTableModal  = new someAnyTableModal()
            //someAnyTableModal.property = get propety in modal...
            //_someAnyTableModal.add(dataa);
            //if success then redict to action  have tabe. or work something

            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeesCreateViewModel model)
        {
            if (ModelState.IsValid)
            {

                string uniqueFileName = null;

                if (model.Photos != null && model.Photos.Count > 0)
                {
                    uniqueFileName = ProcessUploadedFile(model);

                }
                Employees newEmployees = new Employees
                {
                    Name = model.Name,
                    Address = model.Address,
                    Designation = model.Designation,
                    PhotoPath = uniqueFileName
                };

                _employeesRepository.Add(newEmployees);
                return RedirectToAction("Index");
                // nếu bạn muốn chuyển đến mot trang(action) nao do kem theo du lieu can truyen den action
                // return RedirectToAction("Details", new { id = newEmployees.EmployeeId });
            }
            return View();
        }



        [HttpGet]
        public ViewResult Edit(int id)
        {
            Employees employees = _employeesRepository.GetEmployees(id);


            EmployeesEditVidewModel studentEditVidew = new EmployeesEditVidewModel
            {
                Id = employees.EmployeeId,
                Name = employees.Name,
                Salary = employees.Salary,
                Address = employees.Address,
                ExistingPhotoPath = employees.PhotoPath
            };

            return View(studentEditVidew);

            //   throw new Exception("some exception if u using try catch");       


        }

        [HttpPost]
        public IActionResult Edit(EmployeesEditVidewModel model)
        {
            //kiểm tra validator dữ liệu hợp lệ hoặc không. Nếu không trả ra lỗi.
            if (ModelState.IsValid)
            {
                Employees employees = _employeesRepository.GetEmployees(model.Id);
                employees.Address = model.Address;
                employees.Name = model.Name;
                employees.Salary = model.Salary;

                if (model.Photos.Count > 0)
                {

                    if (model.ExistingPhotoPath != null)
                    {
                        string filePahth = Path.Combine(hostingEnvironment.WebRootPath, "images", model.ExistingPhotoPath);

                        System.IO.File.Delete(filePahth);

                    }

                    employees.PhotoPath = ProcessUploadedFile(model);

                }

                Employees updateEmployees = _employeesRepository.Update(employees);

                return RedirectToAction("Index");

            }

            return View(model);
        }

        private string ProcessUploadedFile(EmployeesCreateViewModel model)
        {


            string uniqueFileName = null;

            if (model.Photos.Count > 0)
            {
                foreach (var photo in model.Photos)
                {

                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;

                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {

                        photo.CopyTo(fileStream);
                    }
                }
            }

            return uniqueFileName;

        }




        public IActionResult Details(string id)
        {


            var decryptedId = dataProtector.Unprotect(id);

            var decryptedStudentId = Convert.ToInt32(decryptedId);


            Employees student = _employeesRepository.GetEmployees(decryptedStudentId);

            if (student == null)
            {
                ViewBag.ErrorMessage = $"Information for id={id} not exist";
                return View("NotFound");
            }


            EmployeesDetailsViewModel homeDetailsViewModel = new EmployeesDetailsViewModel()
            {
                Employees = student,
                PageTitle = "Employees Details"
            };

            return View(homeDetailsViewModel);
        }
    }
}
