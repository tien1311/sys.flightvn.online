function Save() {
    var ID_DEBT = document.getElementById("ID").value;
    var MaNV = document.getElementById("MaNV").value;
    var TenNV = document.getElementById("TenNV").value;
    var SoTienNo = document.getElementById("SoTienNo").value.replace(/\$|\,/g, '');
    var ThoiGianXuatVe = document.getElementById("ThoiGianXuatVe").value;
    var DuNo = document.getElementById("DuNo").value.replace(/\$|\,/g, '');
    var GhiChu = document.getElementById("GhiChu").value;
    var TinhTrang = document.getElementById("TinhTrang").value;

    if (SoTienNo == null || SoTienNo == "") {
        SoTienNo = 0;
    }
    if (DuNo == null || DuNo == "") {
        DuNo = 0;
    }

    var CongNoQuaHan = {};
    CongNoQuaHan.ID_DEBT = ID_DEBT;
    CongNoQuaHan.MaNV = MaNV;
    CongNoQuaHan.TenNV = TenNV;
    CongNoQuaHan.SoTienNo = SoTienNo;
    CongNoQuaHan.ThoiGianXuatVe = ThoiGianXuatVe;
    CongNoQuaHan.DuNo = DuNo;
    CongNoQuaHan.GhiChu = GhiChu;
    CongNoQuaHan.TinhTrang = TinhTrang;
    let DataJson = JSON.stringify(CongNoQuaHan);
    $.ajax({
        type: "POST",
        url: "../KeToan/SaveCongNoNVQuaHan",
        data: {
            data: DataJson
        },
        dataType: "json",
        success: function (resultData) {
            alert(resultData);
            window.location.href = "/KeToan/CongNoQuaHan?i=5";
        },
        error: function () {
        }
    })
}
function Del(ID) {
    $.ajax({
        type: "POST",
        url: "../KeToan/DelCongNoNVQuaHan",
        data: {
            ID: ID
        },
        dataType: "json",
        success: function (resultData) {
            alert(resultData);
            window.location.href = "/KeToan/CongNoQuaHan?i=5";
        },
        error: function () {
        }
    })
}