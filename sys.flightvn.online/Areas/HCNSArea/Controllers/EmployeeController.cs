
using EasyInvoice.Json.Linq;
using Manager.Common.Helpers.AreaHelpers;
using Manager.DataAccess.Repository;
using Manager.DataAccess.Services.CarBooking;
using Manager.Model.Models;
using Manager.Model.Models.CarBooking;
using Manager.Model.Models.CarBooking.Result;
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
        private readonly TaxiServices taxiServices;
        public EmployeeController(TaxiServices _taxiServices)
        {
            taxiServices = _taxiServices;
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
        [HttpGet]
        public IActionResult CreateEmployee()
        {
             
            return View();
        }

 


    }
}
