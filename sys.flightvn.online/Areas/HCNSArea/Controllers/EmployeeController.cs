
using EasyInvoice.Json.Linq;
using Manager.Common.Helpers.AreaHelpers;
using Manager.DataAccess.IRepository.IUnitOfWork_Repository;
using Manager.DataAccess.Repository;
using Manager.DataAccess.Services.CarBooking;
using Manager.Model.Models;
using Manager.Model.Models.CarBooking;
using Manager.Model.Models.CarBooking.Result;
using Manager.Model.Models.HCNS;
using Manager.Model.Models.Other;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;


namespace Manager_EV.Areas.DuLichArea.Controllers
{
    [Area(AreaNameConst.AREA_HCNS)]
    public class EmployeeController : Controller
    {
        private IUnitOfWork_Repository _unitOfWork_Rep;
        public EmployeeController(IUnitOfWork_Repository unitOfWork_Rep)
        {
            _unitOfWork_Rep = unitOfWork_Rep;
        }

        // Settings và lưu bản ghi (records) 
        public Dictionary<string, int> Record
        {
            get
            {
                if (HttpContext.Session.IsAvailable)
                {
                    // Chứa active record [0] và list records [1]
                    Dictionary<string, int> record = HttpContext.Session.GetObject<Dictionary<string, int>>("record");
                    if (record != null)
                    {
                        return record;
                    }
                }
                return new Dictionary<string, int> { { "record", 10 } };
            }
        }
        public async Task<IActionResult> Employee()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateEmployee()
        {
            ViewBag.EmployeesCode = await _unitOfWork_Rep.Employee_Rep.GetEmployeeCode();
            ViewBag.Positions = await _unitOfWork_Rep.Employee_Rep.GetPosition();
            ViewBag.Departments = await _unitOfWork_Rep.Employee_Rep.GetDepartment();
            ViewBag.Divisions = await _unitOfWork_Rep.Employee_Rep.GetDivision();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(EmployeeModel employee)
        {
            ViewBag.EmployeesCode = await _unitOfWork_Rep.Employee_Rep.GetEmployeeCode();
            ViewBag.Positions = await _unitOfWork_Rep.Employee_Rep.GetPosition();
            ViewBag.Departments = await _unitOfWork_Rep.Employee_Rep.GetDepartment();
            ViewBag.Divisions = await _unitOfWork_Rep.Employee_Rep.GetDivision();

            return View();
        }



    }
}
