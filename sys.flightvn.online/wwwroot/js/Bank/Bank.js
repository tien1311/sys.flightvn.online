$("#BtnCreate").click(function () {

    $.ajax({
        type: "POST",
        url: "../BankInfo/CreateBank",
        success: function (response) {
            $('#openPopup').html(response);
            $('#openPopup').modal({
                backdrop: 'static',
                keyboard: false,
                show: true
            });
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
});

$("#gridBank .Edit").click(function () {
    var id = String($(this).closest('tr').attr('id'));

    $.ajax({
        type: "POST",
        url: "../BankInfo/EditBank",
        data: {
            ID: id
        },
        success: function (response) {
            $('#openPopup').html(response);
            $('#openPopup').modal({
                backdrop: 'static',
                keyboard: false,
                show: true
            });
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
});
$("#gridBank .Delete").click(function () {
    var id = String($(this).closest('tr').attr('id'));
    $.ajax({
        type: "POST",
        url: "../BankInfo/DeleteBank",
        data: {
            ID: id
        },
        success: function (response) {
            if (response == true) {
                alert("Bạn đã xóa thành công");
            }
            else {
                alert("Bạn đã xóa không thành công");
            }
            window.location.href = "../BankInfo/Bank?&i=9";
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
});
$("#gridBank .Active").click(function () {
    var id = String($(this).closest('tr').attr('id'));
    $.ajax({
        type: "POST",
        url: "../BankInfo/ActiveBank",
        data: {
            ID: id
        },
        success: function (response) {
            if (response == true) {
                alert("Bạn đã kích hoạt thành công");
            }
            else {
                alert("Bạn đã kích hoạt không thành công");
            }
            window.location.href = "../BankInfo/Bank?&i=9";
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
});