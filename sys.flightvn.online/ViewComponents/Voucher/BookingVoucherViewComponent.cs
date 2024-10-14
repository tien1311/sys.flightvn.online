using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Manager.Model.Models;
using Manager.DataAccess.IRepository.IUnitOfWork_Repository;

namespace Manager_EV.ViewComponents.Voucher
{
    public class BookingVoucherViewComponent : ViewComponent
    {

        //private VoucherRepository Voucher_Rep = new VoucherRepository();
        private readonly IUnitOfWork_Repository _repositoryManager;
        private readonly IConfiguration _configuration;
        public BookingVoucherViewComponent(IConfiguration configuration, IUnitOfWork_Repository ManagerAllRepository)
        {
            _configuration = configuration;
            _repositoryManager = ManagerAllRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync(int orderHeaderId)
        {
            OrderHeaderVoucher updateStatusModel = new OrderHeaderVoucher();
            updateStatusModel = _repositoryManager.Voucher_Rep.GetStatusNoteAfterUpdate(orderHeaderId);

            return View("~/Views/Shared/Components/Voucher/BookingVoucher/Default.cshtml", updateStatusModel);
        }
    }
}
