function GiaoCho() {
    var OrderID = document.getElementById("OrderID").value;
    var per = document.getElementById("NguoiThayDoi").value;
    var perOld = document.getElementById("NguoiThayDoiOld").value;

    var LgsProtiValues = "Giao cho";
    var LgsOldValues = perOld;
    var LgsNewValues = per;
    $.ajax({
        type: "POST",
        url: "../PhongVe/UpdateGiaoCho",
        data: {
            ID: OrderID,
            per: per,
            LgsProtiValues: LgsProtiValues,
            LgsOldValues: LgsOldValues,
            LgsNewValues: LgsNewValues
        },
        success: function (response) {
            if (response == true) {
                alert("Cập nhật booking thành công");
            }
            else {
                alert("Cập nhật booking không thành công");
            }
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}
function DoiTrangThai() {
    var OrderID = document.getElementById("OrderID").value;
    var TinhTrang = document.getElementById("Tinhtrang").value;
    var selTinhTrang = document.getElementById("Tinhtrang");
    var lblTinhTrang = selTinhTrang.options[selTinhTrang.selectedIndex].text;

    var lblTinhTrangOld = document.getElementById('lblTinhTrang').innerHTML;

    var LgsProtiValues = "Tình trạng";
    var LgsOldValues = lblTinhTrangOld;
    var LgsNewValues = lblTinhTrang;
    $.ajax({
        type: "POST",
        url: "../PhongVe/UpdateTinhTrangBooking",
        data: {
            ID: OrderID,
            TinhTrang: TinhTrang,
            LgsProtiValues: LgsProtiValues,
            LgsOldValues: LgsOldValues,
            LgsNewValues: LgsNewValues
        },
        success: function (response) {
            if (response == true) {
                alert("Cập nhật tình trạng booking thành công");
                document.getElementById('lblTinhTrang').innerHTML = lblTinhTrang;
            }
            else {
                alert("Cập nhật tình trạng booking không thành công");
            }
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}
function EditTinhTrang() {
    document.getElementById("btnEditTinhTrang").style.display = "none";
    document.getElementById("btnUpdateTinhTrang").style.display = "inline";
    document.getElementById("btnCancelTinhTrang").style.display = "inline";

    const targetEle = document.querySelectorAll('.TinhTrang');
    var elemCount = targetEle.length;
    for (var i = 0; i < elemCount; i++) {
        if (targetEle[i].classList.contains('show')) {
            targetEle[i].classList.remove('show');
            targetEle[i].classList.add('hide');
        }
        else {
            targetEle[i].classList.remove('hide');
            targetEle[i].classList.add('show');
        }
    }
}
function CancelTinhTrang() {
    document.getElementById("btnEditTinhTrang").style.display = "inline";
    document.getElementById("btnUpdateTinhTrang").style.display = "none";
    document.getElementById("btnCancelTinhTrang").style.display = "none";

    const targetEle = document.querySelectorAll('.TinhTrang');
    var elemCount = targetEle.length;
    for (var i = 0; i < elemCount; i++) {
        if (targetEle[i].classList.contains('show')) {
            targetEle[i].classList.remove('show');
            targetEle[i].classList.add('hide');
        }
        else {
            targetEle[i].classList.remove('hide');
            targetEle[i].classList.add('show');
        }
    }
}
function UpdateTinhTrang() {
    var OrderID = document.getElementById("OrderID").value;

    var HTThanhToan = document.getElementById("txtHTThanhToan").value;
    var selThanhToan = document.getElementById("txtHTThanhToan");
    var lblHTThanhToan = selThanhToan.options[selThanhToan.selectedIndex].text;

    var NganHangCK = document.getElementById("txtNganHangCK").value;
    var LuotDi = document.getElementById("txtLuotDi").value;
    var TimeLimit = document.getElementById("txtTimeLimit").value;
    var DateLimit = document.getElementById("txtDateLimit").value;
    var MaGiaoDich = document.getElementById("txtMaGiaoDich").value;
    var NganHangTTTT = document.getElementById("txtNganHangTTTT").value;

    var TinhTrang = document.getElementById("txtTinhTrang").value;
    var selTinhTrang = document.getElementById("txtTinhTrang");
    var lblTinhTrang = selTinhTrang.options[selTinhTrang.selectedIndex].text;

    var TinhTrangTT = document.getElementById("txtTinhTrangTT").checked;
    if (TinhTrangTT == true) {
        var lblTinhTrangTT = "Đã Thanh Toán";
    }
    else {
        var lblTinhTrangTT = "Chưa Thanh Toán";
    }
    var SoTienCK = document.getElementById("txtSoTienCK").value;
    var MaDatChoLD = document.getElementById("txtMaDatChoLD").value;
    var MaDatChoLV = document.getElementById("txtMaDatChoLV").value;
    var LuotVe = document.getElementById("txtLuotVe").value;
    var Remark = document.getElementById("txtRemark").value;
    var MaThamChieu = document.getElementById("txtMaThamChieu").value;
    var TienTrucTuyen = document.getElementById("txtTienTrucTuyen").value;

    var lblHTThanhToanOld = document.getElementById('lblHTThanhToan').innerHTML;
    var lblNganHangCKOld =  document.getElementById('lblNganHangCK').innerHTML;
    var lblLuotDiOld = document.getElementById('lblLuotDi').innerHTML;
    var lblTimeLimitOld = document.getElementById('lblTimeLimit').innerHTML;
    var lblDateLimitOld = document.getElementById('lblDateLimit').innerHTML;
    var lblMaGiaoDichOld = document.getElementById('lblMaGiaoDich').innerHTML;
    var lblNganHangTTTTOld = document.getElementById('lblNganHangTTTT').innerHTML;
    var lblTinhTrangOld = document.getElementById('lblTinhTrang').innerHTML;
    var lblTinhTrangTTOld = document.getElementById('lblTinhTrangTT').innerHTML;
    var lblSoTienCKOld = document.getElementById('lblSoTienCK').innerHTML;
    var lblMaDatChoLDOld = document.getElementById('lblMaDatChoLD').innerHTML;
    var lblMaDatChoLVOld = document.getElementById('lblMaDatChoLV').innerHTML;
    var lblLuotVeOld = document.getElementById('lblLuotVe').innerHTML;
    var lblRemarkOld = document.getElementById('lblRemark').innerHTML;
    var lblMaThamChieuOld = document.getElementById('lblMaThamChieu').innerHTML;
    var lblTienTrucTuyenOld = document.getElementById('lblTienTrucTuyen').innerHTML;

    var LgsProtiValues = "Hình thức thanh toán|Ngân hàng chuyển khoản|Lượt đi|Time Limit|Date Limit|Mã giao dịch|Ngân hàng trực tuyến|Tình trạng|Tình trạng thanh toán|Số tiền CK|Mã đặt chổ LD|Mã đặt chổ LV|Lượt về|Remark|Mã tham chiếu|Số tiền GD trực tuyến";
    var LgsOldValues = lblHTThanhToanOld + "|" + lblNganHangCKOld + "|" + lblLuotDiOld + "|" + lblTimeLimitOld + "|" + lblDateLimitOld + "|" + lblMaGiaoDichOld + "|" + lblNganHangTTTTOld + "|" + lblTinhTrangOld + "|" + lblTinhTrangTTOld + "|" + lblSoTienCKOld + "|" + lblMaDatChoLDOld + "|" + lblMaDatChoLVOld + "|" + lblLuotVeOld + "|" + lblRemarkOld + "|" + lblMaThamChieuOld + "|" + lblTienTrucTuyenOld;
    var LgsNewValues = lblHTThanhToan + "|" + NganHangCK + "|" + LuotDi + "|" + TimeLimit + "|" + DateLimit + "|" + MaGiaoDich + "|" + NganHangTTTT + "|" + lblTinhTrang + "|" + lblTinhTrangTT + "|" + SoTienCK + "|" + MaDatChoLD + "|" + MaDatChoLV + "|" + LuotVe + "|" + Remark + "|" + MaThamChieu + "|" + TienTrucTuyen;
    $.ajax({
        type: "POST",
        url: "../PhongVe/UpdateTinhTrang",
        data: {
            ID: OrderID,

            HTThanhToan: HTThanhToan,
            NganHangCK: NganHangCK,
            LuotDi: LuotDi,
            TimeLimit: TimeLimit,
            DateLimit: DateLimit,
            MaGiaoDich: MaGiaoDich,
            NganHangTTTT: NganHangTTTT,
            TinhTrang: TinhTrang,
            TinhTrangTT: TinhTrangTT,
            SoTienCK: SoTienCK,
            MaDatChoLD: MaDatChoLD,
            MaDatChoLV: MaDatChoLV,
            LuotVe: LuotVe,
            Remark: Remark,
            MaThamChieu: MaThamChieu,
            TienTrucTuyen: TienTrucTuyen,

            LgsProtiValues: LgsProtiValues,
            LgsOldValues: LgsOldValues,
            LgsNewValues: LgsNewValues
        },
        success: function (response) {
            if (response == true) {
                alert("Cập nhật tình trạng booking thành công");
                CancelTinhTrang();
                document.getElementById('lblHTThanhToan').innerHTML = lblHTThanhToan;
                document.getElementById('lblNganHangCK').innerHTML = NganHangCK;
                document.getElementById('lblLuotDi').innerHTML = LuotDi;
                document.getElementById('lblTimeLimit').innerHTML = TimeLimit;
                document.getElementById('lblDateLimit').innerHTML = DateLimit;
                document.getElementById('lblMaGiaoDich').innerHTML = MaGiaoDich;
                document.getElementById('lblNganHangTTTT').innerHTML = NganHangTTTT;
                document.getElementById('lblTinhTrang').innerHTML = lblTinhTrang;
                if (TinhTrangTT == true) {
                    document.getElementById('lblTinhTrangTT').innerHTML = "Đã Thanh Toán";
                }
                else {
                    document.getElementById('lblTinhTrangTT').innerHTML = "Chưa Thanh Toán";
                }
                document.getElementById('lblSoTienCK').innerHTML = SoTienCK;
                document.getElementById('lblMaDatChoLD').innerHTML = MaDatChoLD;
                document.getElementById('lblMaDatChoLV').innerHTML = MaDatChoLV;
                document.getElementById('lblLuotVe').innerHTML = LuotVe;
                document.getElementById('lblRemark').innerHTML = Remark;
                document.getElementById('lblMaThamChieu').innerHTML = MaThamChieu;
                document.getElementById('lblTienTrucTuyen').innerHTML = TienTrucTuyen;
            }
            else {
                alert("Cập nhật tình trạng booking không thành công");
            }
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}

function EditLienHe() {
    document.getElementById("btnEditLienHe").style.display = "none";
    document.getElementById("btnUpdateLienHe").style.display = "inline";
    document.getElementById("btnCancelLienHe").style.display = "inline";

    const targetEle = document.querySelectorAll('.LienHe');
    var elemCount = targetEle.length;
    for (var i = 0; i < elemCount; i++) {
        if (targetEle[i].classList.contains('show')) {
            targetEle[i].classList.remove('show');
            targetEle[i].classList.add('hide');
        }
        else {
            targetEle[i].classList.remove('hide');
            targetEle[i].classList.add('show');
        }
    }
}
function CancelLienHe() {
    document.getElementById("btnEditLienHe").style.display = "inline";
    document.getElementById("btnUpdateLienHe").style.display = "none";
    document.getElementById("btnCancelLienHe").style.display = "none";

    const targetEle = document.querySelectorAll('.LienHe');
    var elemCount = targetEle.length;
    for (var i = 0; i < elemCount; i++) {
        if (targetEle[i].classList.contains('show')) {
            targetEle[i].classList.remove('show');
            targetEle[i].classList.add('hide');
        }
        else {
            targetEle[i].classList.remove('hide');
            targetEle[i].classList.add('show');
        }
    }
}
function UpdateLienHe() {
    var OrderID = document.getElementById("OrderID").value;

    var NguoiLienHe = document.getElementById("txtNguoiLienHe").value;
    var Email = document.getElementById("txtEmail").value;
    var ThanhPho = document.getElementById("txtThanhPho").value;
    var DienThoai = document.getElementById("txtDienThoai").value;
    var DiaChi = document.getElementById("txtDiaChi").value;
    var QuocGia = document.getElementById("txtQuocGia").value;

    var lblNguoiLienHeOld = document.getElementById('lblNguoiLienHe').innerHTML;
    var lblEmailOld = document.getElementById('lblEmail').innerHTML ;
    var lblThanhPhoOld = document.getElementById('lblThanhPho').innerHTML;
    var lblDienThoaiOld = document.getElementById('lblDienThoai').innerHTML;
    var lblDiaChiOld = document.getElementById('lblDiaChi').innerHTML;
    var lblQuocGiaOld = document.getElementById('lblQuocGia').innerHTML;

    var LgsProtiValues = "Người liên hệ|Email|Thành phố|Điện thoại|Địa chỉ|Quốc gia";
    var LgsOldValues = lblNguoiLienHeOld + "|" + lblEmailOld + "|" + lblThanhPhoOld + "|" + lblDienThoaiOld + "|" + lblDiaChiOld + "|" + lblQuocGiaOld;
    var LgsNewValues = NguoiLienHe + "|" + Email + "|" + ThanhPho + "|" + DienThoai + "|" + DiaChi + "|" + QuocGia;
    $.ajax({
        type: "POST",
        url: "../PhongVe/UpdateLienHe",
        data: {
            ID: OrderID,
            NguoiLienHe: NguoiLienHe,
            Email: Email,
            ThanhPho: ThanhPho,
            DienThoai: DienThoai,
            DiaChi: DiaChi,
            QuocGia: QuocGia,
            LgsProtiValues: LgsProtiValues,
            LgsOldValues: LgsOldValues,
            LgsNewValues: LgsNewValues
        },
        success: function (response) {
            if (response == true) {
                alert("Cập nhật thông tin liên hệ thành công");
                CancelLienHe();
                document.getElementById('lblNguoiLienHe').innerHTML = NguoiLienHe;
                document.getElementById('lblEmail').innerHTML = Email;
                document.getElementById('lblThanhPho').innerHTML = ThanhPho;
                document.getElementById('lblDienThoai').innerHTML = DienThoai;
                document.getElementById('lblDiaChi').innerHTML = DiaChi;
                document.getElementById('lblQuocGia').innerHTML = QuocGia;
            }
            else {
                alert("Cập nhật thông tin liên hệ không thành công");
            }
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}

function EditHoaDon() {
    document.getElementById("btnEditHoaDon").style.display = "none";
    document.getElementById("btnUpdateHoaDon").style.display = "inline";
    document.getElementById("btnCancelHoaDon").style.display = "inline";

    const targetEle = document.querySelectorAll('.HoaDon');
    var elemCount = targetEle.length;
    for (var i = 0; i < elemCount; i++) {
        if (targetEle[i].classList.contains('show')) {
            targetEle[i].classList.remove('show');
            targetEle[i].classList.add('hide');
        }
        else {
            targetEle[i].classList.remove('hide');
            targetEle[i].classList.add('show');
        }
    }
}
function CancelHoaDon() {
    document.getElementById("btnEditHoaDon").style.display = "inline";
    document.getElementById("btnUpdateHoaDon").style.display = "none";
    document.getElementById("btnCancelHoaDon").style.display = "none";

    const targetEle = document.querySelectorAll('.HoaDon');
    var elemCount = targetEle.length;
    for (var i = 0; i < elemCount; i++) {
        if (targetEle[i].classList.contains('show')) {
            targetEle[i].classList.remove('show');
            targetEle[i].classList.add('hide');
        }
        else {
            targetEle[i].classList.remove('hide');
            targetEle[i].classList.add('show');
        }
    }
}
function UpdateHoaDon() {
    var OrderID = document.getElementById("OrderID").value;
    var CongTy = document.getElementById("txtCongTy").value;
    var DiaChiCTY = document.getElementById("txtDiaChiCTY").value;
    var MST = document.getElementById("txtMST").value;
    var DiaChiHD = document.getElementById("txtDiaChiHD").value;

    var lblCongTyOld = document.getElementById('lblCongTy').innerHTML;
    var lblDiaChiCTYOld = document.getElementById('lblDiaChiCTY').innerHTML;
    var lblMSTOld = document.getElementById('lblMST').innerHTML;
    var lblDiaChiHDOld = document.getElementById('lblDiaChiHD').innerHTML;

    var LgsProtiValues = "Công ty|Địa Chỉ CTY|MST|Địa chỉ HD";
    var LgsOldValues = lblCongTyOld + "|" + lblDiaChiCTYOld + "|" + lblMSTOld + "|" + lblDiaChiHDOld;
    var LgsNewValues = CongTy + "|" + DiaChiCTY + "|" + MST + "|" + DiaChiHD;
    $.ajax({
        type: "POST",
        url: "../PhongVe/UpdateHoaDon",
        data: {
            ID: OrderID,
            CongTy: CongTy,
            DiaChiCTY: DiaChiCTY,
            MST: MST,
            DiaChiHD: DiaChiHD,
            LgsProtiValues: LgsProtiValues,
            LgsOldValues: LgsOldValues,
            LgsNewValues: LgsNewValues
        },
        success: function (response) {
            if (response == true) {
                alert("Cập nhật thông tin hóa đơn thành công");
                CancelHoaDon();
                document.getElementById('lblCongTy').innerHTML = CongTy;
                document.getElementById('lblDiaChiCTY').innerHTML = DiaChiCTY;
                document.getElementById('lblMST').innerHTML = MST;
                document.getElementById('lblDiaChiHD').innerHTML = DiaChiHD;
            }
            else {
                alert("Cập nhật thông tin hóa đơn không thành công");
            }
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}

function btnEditThongTinKhach(RowID) {
    var btnEditThongTinKhach = "btnEditThongTinKhach" + RowID;
    var btnUpdateThongTinKhach = "btnUpdateThongTinKhach" + RowID;
    var btnCancelThongTinKhach = "btnCancelThongTinKhach" + RowID;
    var ThongTinKhach = ".ThongTinKhach" + RowID;
    document.getElementById(btnEditThongTinKhach).style.display = "none";
    document.getElementById(btnUpdateThongTinKhach).style.display = "inline";
    document.getElementById(btnCancelThongTinKhach).style.display = "inline";

    const targetEle = document.querySelectorAll(ThongTinKhach);
    var elemCount = targetEle.length;
    for (var i = 0; i < elemCount; i++) {
        if (targetEle[i].classList.contains('show')) {
            targetEle[i].classList.remove('show');
            targetEle[i].classList.add('hide');
        }
        else {
            targetEle[i].classList.remove('hide');
            targetEle[i].classList.add('show');
        }
    }
}
function btnCancelThongTinKhach(RowID) {
    var btnEditThongTinKhach = "btnEditThongTinKhach" + RowID;
    var btnUpdateThongTinKhach = "btnUpdateThongTinKhach" + RowID;
    var btnCancelThongTinKhach = "btnCancelThongTinKhach" + RowID;
    var ThongTinKhach = ".ThongTinKhach" + RowID;
    document.getElementById(btnEditThongTinKhach).style.display = "inline";
    document.getElementById(btnUpdateThongTinKhach).style.display = "none";
    document.getElementById(btnCancelThongTinKhach).style.display = "none";

    const targetEle = document.querySelectorAll(ThongTinKhach);
    var elemCount = targetEle.length;
    for (var i = 0; i < elemCount; i++) {
        if (targetEle[i].classList.contains('show')) {
            targetEle[i].classList.remove('show');
            targetEle[i].classList.add('hide');
        }
        else {
            targetEle[i].classList.remove('hide');
            targetEle[i].classList.add('show');
        }
    }
}
function btnUpdateThongTinKhach(RowID) {
    var OrderID = document.getElementById("OrderID").value;
    var lblCodeDi = "lblCodeDi" + RowID;
    var lblCodeVe = "lblCodeVe" + RowID;
    var Code_OneWay = "txtCodeDi" + RowID;
    var Code_RoundTrip = "txtCodeVe" + RowID;
    var CodeDi = document.getElementById(Code_OneWay).value;
    var CodeVe = document.getElementById(Code_RoundTrip).value;
    var lblCodeDiOld = document.getElementById(lblCodeDi).innerHTML;
    var lblCodeVeOld = document.getElementById(lblCodeVe).innerHTML;
    var LgsProtiValues = "Code đi|Code về";
    var LgsOldValues = lblCodeDiOld + "|" + lblCodeVeOld;
    var LgsNewValues = CodeDi + "|" + CodeVe;
    $.ajax({
        type: "POST",
        url: "../PhongVe/UpdateThongTinKhach",
        data: {
            ID: RowID,
            OrderID: OrderID,
            CodeDi: CodeDi,
            CodeVe: CodeVe,
            LgsProtiValues: LgsProtiValues,
            LgsOldValues: LgsOldValues,
            LgsNewValues: LgsNewValues
        },
        success: function (response) {
            if (response == true) {
                alert("Cập nhật thông tin khách thành công");
                btnCancelThongTinKhach(RowID);
                document.getElementById(lblCodeDi).innerHTML = CodeDi;
                document.getElementById(lblCodeVe).innerHTML = CodeVe;
            }
            else {
                alert("Cập nhật thông tin khách không thành công");
            }
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}

function EditThongTinCBLD() {
    document.getElementById("btnEditThongTinCBLD").style.display = "none";
    document.getElementById("btnUpdateThongTinCBLD").style.display = "inline";
    document.getElementById("btnCancelThongTinCBLD").style.display = "inline";

    const targetEle = document.querySelectorAll('.ThongTinCBLD');
    var elemCount = targetEle.length;
    for (var i = 0; i < elemCount; i++) {
        if (targetEle[i].classList.contains('show')) {
            targetEle[i].classList.remove('show');
            targetEle[i].classList.add('hide');
        }
        else {
            targetEle[i].classList.remove('hide');
            targetEle[i].classList.add('show');
        }
    }
}
function CancelThongTinCBLD() {
    document.getElementById("btnEditThongTinCBLD").style.display = "inline";
    document.getElementById("btnUpdateThongTinCBLD").style.display = "none";
    document.getElementById("btnCancelThongTinCBLD").style.display = "none";

    const targetEle = document.querySelectorAll('.ThongTinCBLD');
    var elemCount = targetEle.length;
    for (var i = 0; i < elemCount; i++) {
        if (targetEle[i].classList.contains('show')) {
            targetEle[i].classList.remove('show');
            targetEle[i].classList.add('hide');
        }
        else {
            targetEle[i].classList.remove('hide');
            targetEle[i].classList.add('show');
        }
    }
}
function UpdateThongTinCBLD() {
    var OrderID = document.getElementById("OrderID").value;

    var SoHieu_LD = document.getElementById("txtSoHieu_LD").value;
    var Hang_LD = document.getElementById("txtHang_LD").value;
    var Code_LD = document.getElementById("txtCode_LD").value;
    var DiemDi_LD = document.getElementById("txtDiemDi_LD").value;
    var DiemDen_LD = document.getElementById("txtDiemDen_LD").value;
    var NgayDi_LD = document.getElementById("txtNgayDi_LD").value;
    var GioDi_LD = document.getElementById("txtGioDi_LD").value;
    var GioDen_LD = document.getElementById("txtGioDen_LD").value;
    var GiaNet_LD = document.getElementById("txtGiaNet_LD").value;
    var ThuePhi_LD = document.getElementById("txtThuePhi_LD").value;
    var PhiDV_LD = document.getElementById("txtPhiDV_LD").value;
    var Giam_LD = document.getElementById("txtGiam_LD").value;
    var TongTien_LD = document.getElementById("txtTongTien_LD").value;

    var lblSoHieu_LDOld = document.getElementById('lblSoHieu_LD').innerHTML;
    var lblHang_LDOld = document.getElementById('lblHang_LD').innerHTML;
    var lblCode_LDOld = document.getElementById('lblCode_LD').innerHTML;
    var lblDiemDi_LDOld = document.getElementById('lblDiemDi_LD').innerHTML;
    var lblDiemDen_LDOld = document.getElementById('lblDiemDen_LD').innerHTML;
    var lblNgayDi_LDOld = document.getElementById('lblNgayDi_LD').innerHTML;
    var lblGioDi_LDOld = document.getElementById('lblGioDi_LD').innerHTML;
    var lblGioDen_LDOld = document.getElementById('lblGioDen_LD').innerHTML;
    var lblGiaNet_LDOld = document.getElementById('lblGiaNet_LD').innerHTML.replace(",", "");
    var lblThuePhi_LDOld = document.getElementById('lblThuePhi_LD').innerHTML.replace(",", "");
    var lblPhiDV_LDOld = document.getElementById('lblPhiDV_LD').innerHTML.replace(",", "");
    var lblGiam_LDOld = document.getElementById('lblGiam_LD').innerHTML.replace(",", "");
    var lblTongTien_LDOld = document.getElementById('lblTongTien_LD').innerHTML.replace(",", "");

    var LgsProtiValues = "Số hiệu LD|Hạng LD|Code LD|Điểm đi LD|Điểm đến LD|Ngày đi LD|Giờ đi LD|Giờ đến LD|Giá net LD|Thuế phí LD|Phí DV LD|Giảm LD|Tổng tiền LD";
    var LgsOldValues = lblSoHieu_LDOld + "|" + lblHang_LDOld + "|" + lblCode_LDOld + "|" + lblDiemDi_LDOld + "|" + lblDiemDen_LDOld + "|" + lblNgayDi_LDOld + "|" + lblGioDi_LDOld + "|" + lblGioDen_LDOld + "|" + lblGiaNet_LDOld + "|" + lblThuePhi_LDOld + "|" + lblPhiDV_LDOld + "|" + lblGiam_LDOld + "|" + lblTongTien_LDOld;
    var LgsNewValues = SoHieu_LD + "|" + Hang_LD + "|" + Code_LD + "|" + DiemDi_LD + "|" + DiemDen_LD + "|" + NgayDi_LD + "|" + GioDi_LD + "|" + GioDen_LD + "|" + GiaNet_LD + "|" + ThuePhi_LD + "|" + PhiDV_LD + "|" + Giam_LD + "|" + TongTien_LD;
    $.ajax({
        type: "POST",
        url: "../PhongVe/UpdateThongTinCBLD",
        data: {
            ID: OrderID,
            SoHieu_LD: SoHieu_LD,
            Hang_LD: Hang_LD,
            Code_LD: Code_LD,
            DiemDi_LD: DiemDi_LD,
            DiemDen_LD: DiemDen_LD,
            NgayDi_LD: NgayDi_LD,
            GioDi_LD: GioDi_LD,
            GioDen_LD: GioDen_LD,
            GiaNet_LD: GiaNet_LD,
            ThuePhi_LD: ThuePhi_LD,
            PhiDV_LD: PhiDV_LD,
            Giam_LD: Giam_LD,
            TongTien_LD: TongTien_LD,
            LgsProtiValues: LgsProtiValues,
            LgsOldValues: LgsOldValues,
            LgsNewValues: LgsNewValues
        },
        success: function (response) {
            if (response == true) {
                alert("Cập nhật thông tin chuyến bay lượt đi thành công");
                CancelThongTinCBLD();
                document.getElementById('lblSoHieu_LD').innerHTML = SoHieu_LD;
                document.getElementById('lblHang_LD').innerHTML = Hang_LD;
                document.getElementById('lblCode_LD').innerHTML = Code_LD;
                document.getElementById('lblDiemDi_LD').innerHTML = DiemDi_LD;
                document.getElementById('lblDiemDen_LD').innerHTML = DiemDen_LD;
                document.getElementById('lblNgayDi_LD').innerHTML = NgayDi_LD;
                document.getElementById('lblGioDi_LD').innerHTML = GioDi_LD;
                document.getElementById('lblGioDen_LD').innerHTML = GioDen_LD;
                document.getElementById('lblGiaNet_LD').innerHTML = GiaNet_LD;
                document.getElementById('lblThuePhi_LD').innerHTML = ThuePhi_LD;
                document.getElementById('lblPhiDV_LD').innerHTML = PhiDV_LD;
                document.getElementById('lblGiam_LD').innerHTML = Giam_LD;
                document.getElementById('lblTongTien_LD').innerHTML = TongTien_LD;
                
            }
            else {
                alert("Cập nhật thông tin chuyến bay lượt đi không thành công");
            }
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}
function EditThongTinCBLV() {
    document.getElementById("btnEditThongTinCBLV").style.display = "none";
    document.getElementById("btnUpdateThongTinCBLV").style.display = "inline";
    document.getElementById("btnCancelThongTinCBLV").style.display = "inline";

    const targetEle = document.querySelectorAll('.ThongTinCBLV');
    var elemCount = targetEle.length;
    for (var i = 0; i < elemCount; i++) {
        if (targetEle[i].classList.contains('show')) {
            targetEle[i].classList.remove('show');
            targetEle[i].classList.add('hide');
        }
        else {
            targetEle[i].classList.remove('hide');
            targetEle[i].classList.add('show');
        }
    }
}
function CancelThongTinCBLV() {
    document.getElementById("btnEditThongTinCBLV").style.display = "inline";
    document.getElementById("btnUpdateThongTinCBLV").style.display = "none";
    document.getElementById("btnCancelThongTinCBLV").style.display = "none";

    const targetEle = document.querySelectorAll('.ThongTinCBLV');
    var elemCount = targetEle.length;
    for (var i = 0; i < elemCount; i++) {
        if (targetEle[i].classList.contains('show')) {
            targetEle[i].classList.remove('show');
            targetEle[i].classList.add('hide');
        }
        else {
            targetEle[i].classList.remove('hide');
            targetEle[i].classList.add('show');
        }
    }
}
function UpdateThongTinCBLV() {
    var OrderID = document.getElementById("OrderID").value;

    var SoHieu_LV = document.getElementById("txtSoHieu_LV").value;
    var Hang_LV = document.getElementById("txtHang_LV").value;
    var Code_LV = document.getElementById("txtCode_LV").value;
    var DiemDi_LV = document.getElementById("txtDiemDi_LV").value;
    var DiemDen_LV = document.getElementById("txtDiemDen_LV").value;
    var NgayDi_LV = document.getElementById("txtNgayDi_LV").value;
    var GioDi_LV = document.getElementById("txtGioDi_LV").value;
    var GioDen_LV = document.getElementById("txtGioDen_LV").value;
    var GiaNet_LV = document.getElementById("txtGiaNet_LV").value;
    var ThuePhi_LV = document.getElementById("txtThuePhi_LV").value;
    var PhiDV_LV = document.getElementById("txtPhiDV_LV").value;
    var Giam_LV = document.getElementById("txtGiam_LV").value;
    var TongTien_LV = document.getElementById("txtTongTien_LV").value;

    var lblSoHieu_LVOld = document.getElementById('lblSoHieu_LV').innerHTML;
    var lblHang_LVOld = document.getElementById('lblHang_LV').innerHTML;
    var lblCode_LVOld = document.getElementById('lblCode_LV').innerHTML;
    var lblDiemDi_LVOld = document.getElementById('lblDiemDi_LV').innerHTML;
    var lblDiemDen_LVOld = document.getElementById('lblDiemDen_LV').innerHTML;
    var lblNgayDi_LVOld = document.getElementById('lblNgayDi_LV').innerHTML;
    var lblGioDi_LVOld = document.getElementById('lblGioDi_LV').innerHTML;
    var lblGioDen_LVOld = document.getElementById('lblGioDen_LV').innerHTML;
    var lblGiaNet_LVOld = document.getElementById('lblGiaNet_LV').innerHTML.replace(",","");
    var lblThuePhi_LVOld = document.getElementById('lblThuePhi_LV').innerHTML.replace(",", "");
    var lblPhiDV_LVOld = document.getElementById('lblPhiDV_LV').innerHTML.replace(",", "");
    var lblGiam_LVOld = document.getElementById('lblGiam_LV').innerHTML.replace(",", "");
    var lblTongTien_LVOld = document.getElementById('lblTongTien_LV').innerHTML.replace(",", "");

    var LgsProtiValues = "Số hiệu LV|Hạng LV|Code LV|Điểm đi LV|Điểm đến LV|Ngày đi LV|Giờ đi LV|Giờ đến LV|Giá net LV|Thuế phí LV|Phí DV LV|Giảm LV|Tổng tiền LV";
    var LgsOldValues = lblSoHieu_LVOld + "|" + lblHang_LVOld + "|" + lblCode_LVOld + "|" + lblDiemDi_LVOld + "|" + lblDiemDen_LVOld + "|" + lblNgayDi_LVOld + "|" + lblGioDi_LVOld + "|" + lblGioDen_LVOld + "|" + lblGiaNet_LVOld + "|" + lblThuePhi_LVOld + "|" + lblPhiDV_LVOld + "|" + lblGiam_LVOld + "|" + lblTongTien_LVOld;
    var LgsNewValues = SoHieu_LV + "|" + Hang_LV + "|" + Code_LV + "|" + DiemDi_LV + "|" + DiemDen_LV + "|" + NgayDi_LV + "|" + GioDi_LV + "|" + GioDen_LV + "|" + GiaNet_LV + "|" + ThuePhi_LV + "|" + PhiDV_LV + "|" + Giam_LV + "|" + TongTien_LV;
    $.ajax({
        type: "POST",
        url: "../PhongVe/UpdateThongTinCBLV",
        data: {
            ID: OrderID,
            SoHieu_LV: SoHieu_LV,
            Hang_LV: Hang_LV,
            Code_LV: Code_LV,
            DiemDi_LV: DiemDi_LV,
            DiemDen_LV: DiemDen_LV,
            NgayDi_LV: NgayDi_LV,
            GioDi_LV: GioDi_LV,
            GioDen_LV: GioDen_LV,
            GiaNet_LV: GiaNet_LV,
            ThuePhi_LV: ThuePhi_LV,
            PhiDV_LV: PhiDV_LV,
            Giam_LV: Giam_LV,
            TongTien_LV: TongTien_LV,
            LgsProtiValues: LgsProtiValues,
            LgsOldValues: LgsOldValues,
            LgsNewValues: LgsNewValues
        },
        success: function (response) {
            if (response == true) {
                alert("Cập nhật thông tin chuyến bay lượt về thành công");
                CancelThongTinCBLV();
                document.getElementById('lblSoHieu_LV').innerHTML = SoHieu_LV;
                document.getElementById('lblHang_LV').innerHTML = Hang_LV;
                document.getElementById('lblCode_LV').innerHTML = Code_LV;
                document.getElementById('lblDiemDi_LV').innerHTML = DiemDi_LV;
                document.getElementById('lblDiemDen_LV').innerHTML = DiemDen_LV;
                document.getElementById('lblNgayDi_LV').innerHTML = NgayDi_LV;
                document.getElementById('lblGioDi_LV').innerHTML = GioDi_LV;
                document.getElementById('lblGioDen_LV').innerHTML = GioDen_LV;
                document.getElementById('lblGiaNet_LV').innerHTML = GiaNet_LV;
                document.getElementById('lblThuePhi_LV').innerHTML = ThuePhi_LV;
                document.getElementById('lblPhiDV_LV').innerHTML = PhiDV_LV;
                document.getElementById('lblGiam_LV').innerHTML = Giam_LV;
                document.getElementById('lblTongTien_LV').innerHTML = TongTien_LV;

            }
            else {
                alert("Cập nhật thông tin chuyến bay lượt về không thành công");
            }
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}

function NhatKy() {
    const targetEle = document.querySelectorAll('.ThongTinChiTietBK');
    var elemCount = targetEle.length;
    for (var i = 0; i < elemCount; i++) {
        if (targetEle[i].classList.contains('show')) {
            targetEle[i].classList.remove('show');
            targetEle[i].classList.add('hide');
        }
        else {
            targetEle[i].classList.remove('hide');
            targetEle[i].classList.add('show');
        }
    }
}
function SendSMSBooking() {
    var OrderID = document.getElementById("OrderID").value;
    var GiaVeMoi_SMS = document.getElementById("GiaVeMoi_SMS").value;
    var Gio_SMS = document.getElementById("Gio_SMS").value;
    var NgayThanhToan_SMS = document.getElementById("NgayThanhToan_SMS").value;
    if (GiaVeMoi_SMS == "") {
        alert("Bạn phải nhập giá vé mới gửi được sms");
        return;
    }
    if (Gio_SMS == "") {
        alert("Bạn phải nhập giờ thanh toán mới gửi được sms");
        return;
    }
    if (NgayThanhToan_SMS == "") {
        alert("Bạn phải nhập ngày thanh toán mới gửi được sms");
        return;
    }
    let text = "Bạn có muốn gửi SMS cho booking " + OrderID + " này không";
    if (confirm(text) == true) {
        var HanhTrinh_SMS = document.getElementById("HanhTrinh_SMS").value;
        var NguoiLienHe_SMS = document.getElementById("NguoiLienHe_SMS").value;
        var DienThoai_SMS = document.getElementById("DienThoai_SMS").value;
        
        var TinhTrang_SMS = document.getElementById("Tinhtrang").value;
        /*var  = document.getElementById("").value;*/
        $.ajax({
            type: "POST",
            url: "../PhongVe/SendSMSBookingDetail",
            data: {
                ID: OrderID,
                hanhTrinh_SMS: HanhTrinh_SMS,
                nguoiLienHe_SMS: NguoiLienHe_SMS,
                dienThoai_SMS: DienThoai_SMS,
                gio_SMS: Gio_SMS,
                ngayThanhToan_SMS: NgayThanhToan_SMS,
                giaVeMoi_SMS: GiaVeMoi_SMS,
                tinhTrang_SMS: TinhTrang_SMS
                /*HTThanhToan: HTThanhToan,*/
            },
            success: function (response) {
                if (response == true) {
                    alert("Gửi SMS thành công");
                }
                else {
                    alert("Gửi SMS thất bại");
                }
            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            }
        });
    }
}
function SendSMSBookingXuatVe() {
    var TinhTrang_SMS = document.getElementById("Tinhtrang").value;
   
    if (TinhTrang_SMS != "6") {
        alert("Tình trạng xuất vé mới được gửi sms");
        return;
    }
    var OrderID = document.getElementById("OrderID").value;
    let text = "Bạn có muốn gửi SMS cho booking " + OrderID + " này không";
    if (confirm(text) == true) {
        
        $.ajax({
            type: "POST",
            url: "../PhongVe/SendSMSBookingDetailXuatVe",
            data: {
                ID: OrderID
            },
            success: function (response) {
                if (response == true) {
                    alert("Gửi SMS thành công");
                }
                else {
                    alert("Gửi SMS thất bại");
                }
            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            }
        });
    }
}
function SendMail() {
    var TongTien_LD = document.getElementById("txtTongTien_LD").value;
    var TongTien_LV = document.getElementById("txtTongTien_LV").value;
    var ThongTinKhachs = new Array();
    var table2 = document.getElementById("gridThongTinKhach");
    var length_new = table2.rows.length;
    for (var i = 1; i < length_new; i++) {
        var ThongTinKhach = {};
        var DanhXung = table2.rows[i].cells.item(0).innerHTML;
        var Ho = table2.rows[i].cells.item(1).innerHTML;
        var Ten = table2.rows[i].cells.item(2).innerHTML;
        var fullname = Ho + " " + Ten;
        ThongTinKhach.DanhXung = DanhXung;
        ThongTinKhach.FullName = fullname;
        ThongTinKhachs.push(ThongTinKhach);
    }

    var lblHTThanhToan = document.getElementById('lblHTThanhToan').innerHTML;
    var lblTinhTrangTT = document.getElementById('lblTinhTrangTT').innerHTML;
    var TongTien = Number(TongTien_LD) + Number(TongTien_LV);
    var OrderID = document.getElementById("OrderID").value;
    var lblTinhTrang = document.getElementById('lblTinhTrang').innerHTML;
    var TinhTrangStatus = document.getElementById('txtTinhTrang').value;
    var DiemDi = document.getElementById("txtDiemDi_LD").value;
    var DiemDen = document.getElementById("txtDiemDen_LD").value;
    var NgayDi_LD = document.getElementById("txtNgayDi_LD").value;
    var NgayDi_LV = document.getElementById("txtNgayDi_LV").value;
    var Code_LD = document.getElementById("txtCode_LD").value;
    var Code_LV = document.getElementById("txtCode_LV").value;
    var lblAirline_LD = document.getElementById('lblAirline_LD').innerHTML;
    var lblAirline_LV = document.getElementById('lblAirline_LV').innerHTML;
    var SoHieu_LD = document.getElementById("txtSoHieu_LD").value;
    var SoHieu_LV = document.getElementById("txtSoHieu_LV").value;
    var Hang_LD = document.getElementById("txtHang_LD").value;
    var Hang_LV = document.getElementById("txtHang_LV").value;
    var GioDi_LD = document.getElementById("txtGioDi_LD").value;
    var GioDen_LD = document.getElementById("txtGioDen_LD").value;
    var GioDi_LV = document.getElementById("txtGioDi_LV").value;
    var GioDen_LV = document.getElementById("txtGioDen_LV").value;
    var Email = document.getElementById("txtEmail").value;
    var HanhTrinh = document.getElementById("HanhTrinh").value;
    var TimeLimit = document.getElementById("txtTimeLimit").value;
    var DateLimit = document.getElementById("txtDateLimit").value;
  
    var DetailBooking = {};
    DetailBooking.ListThongTinKhach = ThongTinKhachs;
    DetailBooking.HinhThucThanhToan = lblHTThanhToan;
    DetailBooking.TinhTrangThanhToan = lblTinhTrangTT;
    DetailBooking.TongTienMoi = TongTien;
    DetailBooking.OrderID_SMS = OrderID;
    DetailBooking.TinhTrang = lblTinhTrang;
    DetailBooking.TinhTrangStatus = TinhTrangStatus;
    DetailBooking.DiemDi = DiemDi;
    DetailBooking.DiemDen = DiemDen;
    DetailBooking.NgayDi_LD = NgayDi_LD;
    DetailBooking.NgayDi_LV = NgayDi_LV;
    DetailBooking.Code_LD = Code_LD;
    DetailBooking.Code_LV = Code_LV;
    DetailBooking.Airline_LD = lblAirline_LD;
    DetailBooking.Airline_LV = lblAirline_LV;
    DetailBooking.SoHieu_LD = SoHieu_LD;
    DetailBooking.SoHieu_LV = SoHieu_LV;
    DetailBooking.Hang_LD = Hang_LD;
    DetailBooking.Hang_LV = Hang_LV;
    DetailBooking.GioDi_LD = GioDi_LD;
    DetailBooking.GioDen_LD = GioDen_LD
    DetailBooking.GioDi_LV = GioDi_LV;
    DetailBooking.GioDen_LV = GioDen_LV;
    DetailBooking.Email = Email;
    DetailBooking.HanhTrinh = HanhTrinh;
    DetailBooking.TimeLimit = TimeLimit;
    DetailBooking.DateLimit = DateLimit;

    $.ajax({
        type: "POST",
        url: "../PhongVe/SendMail",
        data: {
            DetailBooking: DetailBooking
        },
        success: function (response) {
            if (response == true) {
                alert("Gửi mail thành công");
            }
            else {
                alert("Gửi mail không thành công");
            }
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}