function EditDaiLy(obj, x) {
    var id = '.DaiLy' + x;
    const targetEle = document.querySelectorAll(id);
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
function SaveDaiLy(obj, x, ID) {
    var idMaKH = 'txtMaKH' + x;
    var idSoLuong = 'txtSoLuong' + x;
    var maKH = document.getElementById(idMaKH).value;
    var soLuong = document.getElementById(idSoLuong).value;
    $.ajax({
        type: "POST",
        url: "../Data/SaveDaiLy",
        data: {
            ID: ID,
            MaKH: maKH,
            SoLuong: soLuong
        },
        success: function (response) {
            location.reload();
            if (response == true) {
                alert("Bạn đã lưu danh mục thành công");
            }
            else {
                alert("Bạn đã lưu danh mục không thành công");
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