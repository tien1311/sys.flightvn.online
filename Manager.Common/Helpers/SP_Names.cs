using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Common.Helpers
{

    /// <summary>
    /// Dùng để lấy tên StoreProcidure
    /// Cách gọi SP_Names.LoadVe_VAT_VNA giống như dùng Enum
    /// </summary>
    public static class SP_Names
    {
        public static string LoadVe_VAT_VNA { get { return "SP_LOADVE_DAILY_HOADONVAT_VNA"; } }
        public static string LoadVe_VAT_BSP { get { return "SP_LOADVE_DAILY_HOADONVAT_BSP"; } }
        public static string LoadVe_YC_VNA { get { return "SP_LOADVE_DAILY_YEUCAUVAT_VNA"; } }
        public static string LoadVe_YC_BAMBOO { get { return "SP_LOADVE_DAILY_YEUCAUVAT_BAMBOO"; } }
        public static string LoadVe_YC_BSP { get { return "SP_LOADVE_DAILY_YEUCAUVAT_BSP"; } }
        public static string LoadVe_YC_JS { get { return "SP_LOADVE_DAILY_YEUCAUVAT_Jetstar"; } }
        public static string LoadVe_YC_VJ { get { return "SP_LOADVE_DAILY_YEUCAUVAT_Vietjet"; } }
        public static string LoadVe_HOAN_VNA { get { return "SP_LOADVE_DAILY_YEUCAUVAT_HOANVNA"; } }
        public static string LoadVe_HOAN_BSP { get { return "SP_LOADVE_DAILY_YEUCAUVAT_HOANBSP"; } }

        public static string LoadVe_HOAN_BAMBOO { get { return "SP_LOADVE_DAILY_YEUCAUVAT_HOANBAMBOO"; } }

        public static string LoadVe_VAT_BAMBOO { get { return "SP_LOADVE_DAILY_HOADONVAT_BAMBOO"; } }
        public static string LoadVe_VAT_VJ_ND { get { return "SP_LOADVE_DAILY_HOADONVAT_VJ_NoiDia"; } }

        public static string LoadVe_VAT_VJ_QT { get { return "SP_LOADVE_DAILY_HOADONVAT_Vietjet_QuocTe"; } }

        public static string LoadVe_VAT_VIETRAVEL { get { return "SP_LOADVE_DAILY_HOADONVAT_VU"; } }
        public static string LoadVe_HDDT_VNA { get { return "SP_LOADVE_DAILY_HOADONVAT_VNA"; } }
    }
}
