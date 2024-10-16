$("#check").click(function () {
    var makh = document.getElementById("MaKH").value;
    if (makh == '') {
        alert("Mã KH không dc để trống");
        return;
    }
    $.ajax({
        type: "POST",
        url: "../PhongVe/SearchMaKHBaoCaoVeSot",
        data: {
            MaKH: makh,
        },
        success: function (response) {

            document.getElementById("tenCTY").innerHTML = makh + " : " + response
            return;
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
});

$("#save").click(function () {
    var pnr = document.getElementById("PNR").value;
    var sove = document.getElementById("SoVe").value;
    var hang = document.getElementById("Hang").value;
    var makh = document.getElementById("MaKH").value;
    var giamua = document.getElementById("GiaMua").value;
    var phimua = document.getElementById("PhiMua").value;
    var loaiphi = document.getElementById("KindName").selectedOptions[0].textContent;
    var phixuatve = document.getElementById("PhiXuatVe").value;
    var phiban = document.getElementById("PhiBan").value;
    var phihoan = document.getElementById("PhiHoan").value;
    var chietkhau = document.getElementById("ChietKhau").value;
    var Index = document.getElementById("CurrentIndex").value;
    var magioithieu = document.getElementById("MaGioiThieu").value;
    var nguoigioithieu = document.getElementById("NguoiGioiThieu").value;
    var soluong = document.getElementById("SoLuong").value;

    if (makh == '') {
        alert("Mã KH không dc để trống");
        return;
    }
    $.ajax({
        type: "POST",
        url: "../PhongVe/SaveBaoCaoVeSot",
        data: {
            PNR: pnr,
            SoVe: sove,
            Hang: hang,
            MaKH: makh,
            GiaMua: giamua,
            PhiMua: phimua,
            LoaiPhi: loaiphi,
            PhiXuatVe: phixuatve,
            PhiBan: phiban,
            PhiHoan: phihoan,
            ChietKhau: chietkhau,
            MaGioiThieu: magioithieu,
            NguoiGioiThieu: nguoigioithieu,
            SoLuong: soluong
        },
        success: function (response) {
            if (response == 1) {
                document.getElementById("gridVeSot").rows[Index].cells[10].innerHTML = "Đã báo cáo, chờ KT cập nhật";
                alert("Bạn đã báo cáo vé thành công");
                $('#openPopup').modal('hide');
                return;
            }
            if (response == -1) {
                alert("Số vé này bạn đã báo cáo rồi");
                $('#openPopup').modal('hide');
                return;
            }
            if (response == -2) {
                alert("Mã KH này không tồn tại vui lòng kiểm tra lại mã KH hoặc liên hệ kế toán để kiểm tra mã KH");
                $('#openPopup').modal('hide');
                return;
            }
            if (response == 0 || response == 1) {
                alert("Báo cáo vé không thành công");
                $('#openPopup').modal('hide');
                return;
            }

        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
});

$("#update").click(function () {
    var id = document.getElementById("ID").value;
    var pnr = document.getElementById("PNR").value;
    var sove = document.getElementById("SoVe").value;
    var hang = document.getElementById("Hang").value;
    var makh = document.getElementById("MaKH").value;
    var giamua = document.getElementById("GiaMua").value;
    var phimua = document.getElementById("PhiMua").value;
    var loaiphi = document.getElementById("KindName").selectedOptions[0].textContent;
    var phixuatve = document.getElementById("PhiXuatVe").value;
    var phiban = document.getElementById("PhiBan").value;
    var phihoan = document.getElementById("PhiHoan").value;
    var chietkhau = document.getElementById("ChietKhau").value;
    var Index = document.getElementById("CurrentIndex").value;
    var magioithieu = document.getElementById("MaGioiThieu").value;
    var nguoigioithieu = document.getElementById("NguoiGioiThieu").value;
    var soluong = document.getElementById("SoLuong").value;

    if (makh == '') {
        alert("Mã KH không dc để trống");
        return;
    }
    $.ajax({
        type: "POST",
        url: "../PhongVe/UpdateBaoCaoVeSot",
        data: {
            PNR: pnr,
            SoVe: sove,
            Hang: hang,
            MaKH: makh,
            GiaMua: giamua,
            PhiMua: phimua,
            LoaiPhi: loaiphi,
            PhiXuatVe: phixuatve,
            PhiBan: phiban,
            PhiHoan: phihoan,
            ChietKhau: chietkhau,
            MaGioiThieu: magioithieu,
            NguoiGioiThieu: nguoigioithieu,
            ID: id,
            SoLuong: soluong
        },
        success: function (response) {
            if (response == 1) {
                document.getElementById("gridVeSot").rows[Index].cells[10].innerHTML = "Đã báo cáo, chờ KT cập nhật";
                alert("Bạn đã update báo cáo vé thành công");
                $('#openPopup').modal('hide');
                return;
            }
            if (response == -2) {
                alert("Mã KH này không tồn tại vui lòng kiểm tra lại mã KH hoặc liên hệ kế toán để kiểm tra mã KH");
                $('#openPopup').modal('hide');
                return;
            }
            if (response == -3) {
                alert("PNR này đã được đưa vào dữ liệu, nên không thể cập nhật");
                $('#openPopup').modal('hide');
                $("#getBCVS").click();
                return;
            }
            if (response == 0 || response == 1) {
                alert("Báo cáo vé không thành công");
                $('#openPopup').modal('hide');
                return;
            }

        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
});

function ShowPhiXuatVe(PhiXuatVeId, Id, SoLuong) {
    var SL = document.getElementById(SoLuong).value;
    var Name = document.getElementById(Id).selectedOptions[0].textContent;
    if (Name == "WebBSP" || Name == "HoaHong") {
        document.getElementById(PhiXuatVeId).value = 0;
        document.getElementById(PhiXuatVeId).disabled = false;
    }
    else {
        var phiXuatVe = SL * document.getElementById(Id).value;
        if (phiXuatVe == -1) {
            phiXuatVe = 0;
        }
        document.getElementById(PhiXuatVeId).value = phiXuatVe;
        document.getElementById(PhiXuatVeId).disabled = true;
    }
}