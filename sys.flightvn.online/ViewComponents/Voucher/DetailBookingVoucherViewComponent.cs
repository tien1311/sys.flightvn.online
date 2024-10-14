using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Configuration;
using Manager.Model.Models;
using Manager.DataAccess.IRepository.IUnitOfWork_Repository;

namespace Manager_EV.ViewComponents.Voucher
{
    public class DetailBookingVoucherViewComponent : ViewComponent
    {
        //private VoucherRepository Voucher_Rep = new VoucherRepository();
        private readonly IUnitOfWork_Repository _repositoryManager;
        private readonly IConfiguration _configuration;
        public DetailBookingVoucherViewComponent(IConfiguration configuration, IUnitOfWork_Repository ManagerAllRepository)
        {
            _configuration = configuration;
            _repositoryManager = ManagerAllRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync(int orderHeaderId)
        {
            OrderHeaderVoucher updateStatusModel = new OrderHeaderVoucher();
            updateStatusModel = _repositoryManager.Voucher_Rep.GetStatusNoteAfterUpdate(orderHeaderId);

            return View("~/Views/Shared/Components/Voucher/DetailBookingVoucher/Default.cshtml", updateStatusModel);
        }
    }
}
