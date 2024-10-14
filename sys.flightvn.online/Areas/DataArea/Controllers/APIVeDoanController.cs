using Manager.Common.Helpers.AreaHelpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager_EV.Areas.DataArea.Controllers
{
    [Area(AreaNameConst.AREA_Data)]
    public class APIVeDoanController : Controller
    {
        public IActionResult APIVeDoan()
        {
            return View();
        }
    }
}
