namespace Manager.Model.Models.ThongKe
{
    public class ThongKeData
    {
        // Dữ liệu TourBooking
        public int SLDaTiepNhan;
        public int SLDaGiuCho;
        public int SLDaDatCoc;
        public int SLHoanTatThanhToan;

        // Dữ liệu VISA booking
        public int SLDaNhanBooking;
        public int SLDaTiepNhanHoSo;
        public int SLDangXuLyHoSo;
        public int SLYeuCauBoSungHoSo;
        public int SLNopHoSoThanhCong;
        public int SLDangXetDuyetHoSo;

        // Dữ liệu Car Booking
        public int SLChoGui;
        public int SLChoXuLy;
        public int SLChoXacNhan;
        public int SLXacNhanChuyen;
        public int SLTuChoi;

        // Dữ liệu chung của các trạng thái booking
        public int SLMoi;
        public int SLHoanThanh;
        public int SLHuy;

        // Dữ liệu của doanh thu các kênh bán
        public string CreatedDate;
        public string Name;
        public decimal TotalPrice;
    }
}
