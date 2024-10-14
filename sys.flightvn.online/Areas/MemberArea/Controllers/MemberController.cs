using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manager.Common.Helpers.AreaHelpers;
using Manager.Model.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Manager_EV.Areas.MemberArea.Controllers
{
    [Area(AreaNameConst.AREA_Member)]
    public class MemberController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        MemberListViewModel memberlist_vm = new MemberListViewModel();


        public IActionResult MemberList(MemberListViewModel memberlist)
        {
            try
            {

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return View();
        }
    }
}