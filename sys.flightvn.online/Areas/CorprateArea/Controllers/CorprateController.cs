﻿using Manager.Common.Helpers.AreaHelpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager_EV.Areas.CorprateArea.Controllers
{
    [Area(AreaNameConst.AREA_Corprate)]
    public class CorprateController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}