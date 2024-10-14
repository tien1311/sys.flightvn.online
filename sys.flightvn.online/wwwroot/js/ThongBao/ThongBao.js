function DanhDauDaXem(ID) {
    var id = ID.substring(2, 4);
    $.ajax({
        type: "POST",
        url: "../ThongBao/DanhDauDaXem",
        data: { khoachinh: id },
        success: function (response) {
            if (response == true) {
                document.getElementById(ID).disabled = true;
                alert("Click đã xem thành công");
            }
            else {
                alert("Click đã xem không thành công");
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
function ChiTietThongBaoDL(id, i) {
    /*var index = $('#gridThongBaoDL tr').index($(this).closest('tr'));*/
    //var id = String($(this).closest('tr').attr('id'));
    /*var maNVLap = String($(this).closest('tr').attr('id'));*/
    //var closestTR = $(this).closest("tr");
    //var hiddenInputId = closestTR.find("td > input[type='hidden']").attr("id");

    var idMaNVLap = "NVL" + i;
    var idMaNVKD = "NVKD" + i;
    var maNVKD = document.getElementById(idMaNVKD).value;
    var maNVLap = document.getElementById(idMaNVLap).value;
    var maNV = document.getElementById("MaNV").value;
    if (maNVLap == maNV || maNVKD == maNV) {
        $.ajax({
            type: "POST",
            url: "../ThongBao/ChiTietThongBao",
            data: { khoachinh: id },
            success: function (response) {
                $('#openPopup1').html(response);

                $('#openPopup1').modal('show');
                /*document.getElementById("gridThongBaoDL").rows[index].cells[4].getElementsByTagName("input")[0].checked = true;*/
            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            }
        });
    }
    else {
        alert("Bạn không phải là người tạo thông báo này");
    }

};

function ActiveKHV(id) {
    $.ajax({
        type: "POST",
        url: "../ThongBao/ChangeActiveKHV",
        data: {
            ID: id
        },
        success: function (response) {
            if (response == true) {
                alert("Bạn đã thay đổi thành công");


                $('#checkbox-' + id).prop('disabled', true);
            }
            else {
                alert("Bạn đã thay đổi không thành công");
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
