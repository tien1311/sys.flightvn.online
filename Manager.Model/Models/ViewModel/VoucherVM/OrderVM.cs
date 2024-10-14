using System.Collections.Generic;

namespace Manager.Model.Models.ViewModel.VoucherVM
{
    public class OrderVM
    {
        public OrderHeaderVoucher OrderHeaderVoucher { get; set; }
        public IEnumerable<VoucherModel> OrderVouchers { get; set; }
        public IEnumerable<Status> ListStatus { get; set; }
    }
}
