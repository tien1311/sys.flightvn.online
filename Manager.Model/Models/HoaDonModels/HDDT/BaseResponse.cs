using EasyInvoice.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Manager.Model.Services.Model.Response;
using Manager.Model.Services.Model.Request;

namespace Manager.Model.Models.HoaDonModels.HDDT
{

    public class BaseResponse
    {
        public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;
        public string Message { get; set; } = "Success";
        public Response Result { get; set; } = null;

    }


    [Serializable]
    public class DanhSachHDDTResponse
    {
        public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;
        public string Message { get; set; } = "Success";
        public List<EInvoiceRequest> Result { get; set; } = null;

    }


    [Serializable]
    public class SaveYCHDResponse
    {
        public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;
        public string Message { get; set; } = "Success";
        public YCHDResponse Result { get; set; } = null;

    }


    [Serializable]
    public class DanhSachYCHDDTResponse
    {
        public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;
        public string Message { get; set; } = "Success";
        public List<YeuCauXuat_EInvoiceRequest> Result { get; set; } = null;


        public IEnumerable<StatusDSYC> ListStatus { get; set; }
    }


    [Serializable]
    public class StatusListResponse
    {
        public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;
        public string Message { get; set; } = "Success";
        public IEnumerable<StatusDSYC> Result { get; set; }



    }

}
