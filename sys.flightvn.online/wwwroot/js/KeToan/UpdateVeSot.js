
    $("#check").click(function () {
        var makh = document.getElementById("MaKH").value;
        if (makh == '') {
            alert("Mã KH không dc để trống");
            return;
        }
        $.ajax({
            type: "POST",
            url: `/${AreaNameScript.AREA_PhongVe}/PhongVe/SearchMaKHBaoCaoVeSot`,
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
            url: `/${AreaNameScript.AREA_KeToan}/KeToan/SaveBaoCaoVeSot`,
            data: {
                ID: id,
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
                NguoiGioiThieu: nguoigioithieu
            },
            success: function (response) {
              
                if (response == 1) {
                    document.getElementById("gridVeSot").rows[Index].cells[2].innerHTML = hang;
                

                    document.getElementById("gridVeSot").rows[Index].cells[3].innerHTML = makh;
                    document.getElementById("gridVeSot").rows[Index].cells[4].innerHTML = makh;
                   
                    document.getElementById("gridVeSot").rows[Index].cells[8].innerHTML = pnr;
                    document.getElementById("gridVeSot").rows[Index].cells[9].innerHTML = sove;
                    document.getElementById("gridVeSot").rows[Index].cells[10].innerHTML = giamua;
                    document.getElementById("gridVeSot").rows[Index].cells[11].innerHTML = phimua;
                    document.getElementById("gridVeSot").rows[Index].cells[12].innerHTML = loaiphi;
                    document.getElementById("gridVeSot").rows[Index].cells[13].innerHTML = phixuatve;
                    document.getElementById("gridVeSot").rows[Index].cells[14].innerHTML = phiban;
                    document.getElementById("gridVeSot").rows[Index].cells[15].innerHTML = phihoan;
                    document.getElementById("gridVeSot").rows[Index].cells[16].innerHTML = chietkhau;
                    document.getElementById("gridVeSot").rows[Index].cells[18].innerHTML = magioithieu;
                    document.getElementById("gridVeSot").rows[Index].cells[19].innerHTML = nguoigioithieu;
                    alert("Sửa báo cáo thành công");
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