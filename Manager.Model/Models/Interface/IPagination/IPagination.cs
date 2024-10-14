using System.Collections.Generic;

namespace Manager.Model.Models.Interface.IPagination
{
    public interface IPagination<T>
    {
        IEnumerable<T> ListProduct { get; set; }
        int TotalQuantityOfProduct { get; } // Tổng số sản phẩm có trong list
        int CurrentPage { get; set; } // Trang hiện tại
        int PageSize { get; set; } // Số lượng dữ liệu hiển thị trong trang
        int TotalPage { get; set; } // Tổng số trang hiện có
        int NumberPageToShow { get; set; } // Số trang sẽ show ra
        int StartPage { get; } // trang bắt đầu dựa trên số trang sẽ show ra
        int EndPage { get; } // trang kết thúc dựa trên số trang sẽ show ra
    }
}
