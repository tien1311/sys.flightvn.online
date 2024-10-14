$("#check").click(function () {
    var makh = document.getElementById("MaKH").value;
    if (makh == '') {
        alert("Mã KH không dc để trống");
        return;
    }
    $.ajax({
        type: "POST",
        url: `/${AREA_PhongVe}/PhongVe/SearchMaKHBaoCaoVeSot`,
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
    var id = document.getElementById("RowID").value;
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

    if (makh == '') {
        alert("Mã KH không dc để trống");
        return;
    }
    $.ajax({
        type: "POST",
        url: "../KeToan/SaveBaoCaoVeSotWebVoQuy",
        data: {
            ID: id,
            PNR: pnr,
            SoVe: sove,
            Hang: hang,
            MaKH: makh,
            GiaMua: giamua,
            PhiMua: phimua,
            PhiBan: phiban,
            LoaiPhi: loaiphi,
            PhiXuatVe: phixuatve,
            PhiHoan: phihoan,
            ChietKhau: chietkhau,
            MaGioiThieu: magioithieu,
            NguoiGioiThieu: nguoigioithieu
        },
        success: function (response) {
            if (response != "") {
                alert(response);
                return;
            }
            else {
                alert("Báo cáo thành công");
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

function ShowPhiXuatVe(PhiXuatVeId, Id) {
    var Name = document.getElementById(Id).selectedOptions[0].textContent;
    if (Name == "WebBSP" || Name == "HoaHong") {
        document.getElementById(PhiXuatVeId).value = 0;
        document.getElementById(PhiXuatVeId).disabled = false;
    }
    else {
        var phiXuatVe = document.getElementById(Id).value;
        if (phiXuatVe == -1) {
            phiXuatVe = 0;
        }
        document.getElementById(PhiXuatVeId).value = phiXuatVe;
        document.getElementById(PhiXuatVeId).disabled = true;
    }
}