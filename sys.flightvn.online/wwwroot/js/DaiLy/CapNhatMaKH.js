function CapNhat() {
    var maCK = document.getElementById("MaChuyenKhoan").value;
    var maKH = document.getElementById("MaKH").value;
    var length = Number(maKH.length);
    if (length != 8 && length != 7) {
        alert("Phải 7 ký tự hoặc 8 ký tự");
        return;
    }
    $.ajax({
        type: "POST",
        url: "../DaiLy/UpdateMaKH",
        data: {
            MaCK: maCK,
            MaKH: maKH

        },
        success: function (response) {
            if (response == "TrungKH") {
                alert("Mã chuyển khoản này đã có người cập nhật");
            }
            else {
                if (response == "KhongTonTai") {
                    alert("Mã KH không tồn tại");
                }
                else {
                    alert("Cập nhật thành công");
                }
            }

            location.reload(true);
        },
        failure: function (response) {

            alert(response.responseText);
        },
        error: function (response) {

            alert(response.responseText);
        }
    });


};
function CheckMaKH() {
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
};
