
$("#gridHoanVe .ThongTinVeHoan").click(function () {



    var subject_id = String($(this).closest('tr').attr('id'));

    $.ajax({
        type: "POST",
        url: "../HoanVe/ThongTinVe",
        data: { khoachinh: subject_id },


        success: function (response) {


            $('#openPopup').html(response);

            $('#openPopup').modal('show');

        },
        failure: function (response) {

            alert(response.responseText);
        },
        error: function (response) {

            alert(response.responseText);
        }


    });
});



function DanhDauTinhTrangVe(obj) {
    var table = document.getElementById("gridHoanVe");
    var index = obj.parentNode.parentNode.rowIndex;
    var subject_id = document.getElementById("gridHoanVe").getElementsByTagName('tr')[index].id;



    var danhdau = document.getElementById("gridHoanVe").rows[index].cells[7].getElementsByTagName("input")[0].checked;

    if (confirm("Bạn có chắc muốn đánh dấu")) {
        $.ajax({
            url: '../HoanVe/DanhDauTinhTrangVe',
            data: { id: subject_id, tinhtrang: danhdau },
            method: 'POST'
        })
            .done(function () {


            })
    }
}




function HuyHoanVe(obj) {
    var table = document.getElementById("gridHoanVe");
    var index = obj.parentNode.parentNode.rowIndex;
    var subject_id = document.getElementById("gridHoanVe").getElementsByTagName('tr')[index].id;

    if (confirm("Bạn có chắc muốn hủy")) {
        $.ajax({ url: '../HoanVe/HuyHoanVe', data: { id: subject_id }, method: 'POST' })
            .done(function () {
                location.reload(true);

            })


    }



}



