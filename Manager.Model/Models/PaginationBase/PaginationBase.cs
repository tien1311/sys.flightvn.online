using Manager.Model.Models.Interface.IPagination;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Manager.Model.Models.PaginationBase
{
    public class PaginationBase<T> : IPagination<T> where T : class
    {
        public IEnumerable<T> ListProduct { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPage { get; set; }
        public int NumberPageToShow { get; set; } = 3; // Số page start và end sẽ hiển thị
        public int TotalQuantityOfProduct { get; set; }
        // Các thuộc tính chỉ có getter
        public int StartPage { get => CalculateStartPage(); }
        public int EndPage { get => CalculateEndPage(); }


        private int CalculateStartPage()
        {
            int startPage = CurrentPage - NumberPageToShow / 2;

            // startPage không nhỏ hơn 1
            if (startPage < 1)
            {
                startPage = 1;
            }

            // điều chỉnh để không vượt quá tổng số trang
            if (startPage + NumberPageToShow - 1 > TotalPage)
            {
                startPage = TotalPage - NumberPageToShow + 1;
                if (startPage < 1)
                {
                    startPage = 1;
                }
            }

            return startPage;
        }

        private int CalculateEndPage()
        {
            // Tính số trang kết thúc
            int endPage = CurrentPage + NumberPageToShow / 2;

            // Đảm bảo endPage không lớn hơn tổng số trang
            if (endPage > TotalPage)
            {
                endPage = TotalPage;
            }

            // Nếu số trang kết thúc quá gần đầu, điều chỉnh để không nhỏ hơn số trang bắt đầu
            if (endPage - NumberPageToShow + 1 < 1)
            {
                endPage = NumberPageToShow;
                if (endPage > TotalPage)
                {
                    endPage = TotalPage;
                }
            }

            return endPage;
        }
    }
}
