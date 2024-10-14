using System.Collections.Generic;
using System.Threading.Tasks;
using Manager.Model.Services.Model.Request;
using Manager.Model.Models.HoaDonModels.HDDT;
using Manager.Model.Models.ViewModel;

namespace Manager.Model.Services.Abstraction
{
    public interface IEInvoiceService
    {
        Task<DanhSachHDDTResponse> DanhSachHDDT(DanhSachHDDTRequest request);
        Task<List<EInvoice_CT>> DanhSachVeHDDT(DanhSachHDDTResponse response, string ikey, string pattern, string serial);
        Task<string> GetMaYeuCau(string ikey);
        Task<ReturnObject> HDDTIn(InHDDTRequest request);
        Task<BaseResponse> HDDTCancel(DeleteCancelHDDTRequest request);
        Task<bool> UpdateNgayChungTu(string Ngay);
    }
}
