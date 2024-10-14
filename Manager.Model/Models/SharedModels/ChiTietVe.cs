using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Models.SharedModels
{
    public class ChiTietVeHoaDon
    {
        public int STT { get; set; } = 1;
        public string SoVe { get; set; }
        public string HanhTrinh { get; set; }
        public int SoLuong { get; set; } = 1;
        [DisplayFormat(DataFormatString = "{0:#,###.##}", ApplyFormatInEditMode = true)]
        public decimal DonGia { get; set; } = 0;
        [DisplayFormat(DataFormatString = "{0:#,###.##}", ApplyFormatInEditMode = true)]
        public decimal Thue { get; set; } = 0;
        [DisplayFormat(DataFormatString = "{0:#,###.##}", ApplyFormatInEditMode = true)]
        public decimal ThueKhac { get; set; } = 0;
        [DisplayFormat(DataFormatString = "{0:#,###.##}", ApplyFormatInEditMode = true)]
        public decimal PhiDoi { get; set; } = 0;
        [DisplayFormat(DataFormatString = "{0:#,###.##}", ApplyFormatInEditMode = true)]

        public decimal PhiHoan { get; set; } = 0;
        [DisplayFormat(DataFormatString = "{0:#,###.##}", ApplyFormatInEditMode = true)]
        public decimal PhiEV { get; set; } = 0;
        [DisplayFormat(DataFormatString = "{0:#,###.##}", ApplyFormatInEditMode = true)]
        public decimal TongCong { get; set; } = 0;
        [DisplayFormat(DataFormatString = "{0:#,###.##}", ApplyFormatInEditMode = true)]

        public string GhiChu { get; set; }
        public string HangHK { get; set; }
        public string NgayXuat { get; set; }
        public string PNR { get; set; }

    }
}
