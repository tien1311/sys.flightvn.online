using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Common.Helpers
{
    public class ExcelHelper
    {
        public static int FindColumnIndex(ExcelWorksheet worksheet, string columnName)
        {
            // Tìm tất cả các dòng có thể chứa tiêu đề cột
            for (int row = 1; row <= worksheet.Dimension.End.Row; row++)
            {
                for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                {
                    if (worksheet.Cells[row, col].Text.Equals(columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        return col;
                    }
                }
            }
            return -1; // Trả về -1 nếu không tìm thấy cột
        }

        public static int FindRowIndex(ExcelWorksheet worksheet, string columnName)
        {
            // Tìm tất cả các cột có thể chứa tiêu đề cột
            for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
            {
                for (int row = 1; row <= worksheet.Dimension.End.Row; row++)
                {
                    if (worksheet.Cells[row, col].Text.Equals(columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        return row;
                    }
                }
            }
            return -1; // Trả về -1 nếu không tìm thấy dòng
        }
    }
}
