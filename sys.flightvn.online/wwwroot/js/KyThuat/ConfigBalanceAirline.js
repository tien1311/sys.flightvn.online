$(document).ready(function () {
    $('#gridConfigBalance').DataTable();
});

$("#gridConfigBalance .Edit").click(function () {
    var id = String($(this).closest('tr').attr('id'));
    var cells = $(this).closest('tr').find('td');

    var AirlineBalance = {};
    AirlineBalance.ID = id;
    AirlineBalance.Airline = cells[2].textContent;
    AirlineBalance.Amount = cells[3].textContent;

    $.ajax({
        type: "POST",
        url: `/${AreaNameScript.AREA_KyThuat}/KyThuat/EditConfigAirlineBalance`,
        data: {
            model: AirlineBalance
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

function formatNumber(number, ID) {
    var tongCong = Number(number.replace(/\$|\,/g, ''));
    number = tongCong.toFixed(0) + '';
    x = number.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    document.getElementById(ID).value = x1 + x2;
    return x1 + x2;
}

var ButtonUpdate = document.getElementById("Update");
ButtonUpdate.addEventListener("click", function (event) {
    var ID = document.getElementById("ID").value;
    var Airline = document.getElementById("Airline").value;
    var Amount = document.getElementById("Amount").value.replace(/\$|\,/g, '');

    var request = {};
    request.ID = ID;
    request.Airline = Airline;
    request.Amount = Amount;
   
    Swal.fire({
        html: '<img src="/images/loading.gif" >',
        showCancelButton: false,
        showConfirmButton: false,
        allowOutsideClick: false,
        background: 'transparent',
        onBeforeOpen: () => {
            Swal.showLoading();
        }
    });

    $.ajax({
        type: "POST",
        url: `/${AreaNameScript.AREA_KyThuat}/KyThuat/UpdateAirlineBalance`,
        data: { model: request },
        dataType: "json",
        beforeSend: function () {
            //show loading
            //$("#loader").show();
        },
        complete: function () {
            //hide loading
            //$("#loader").hide();
        },
        success: function (resultData) {
            Swal.close();
            if (resultData == true) {
                Swal.fire({
                    icon: 'success',
                    title: 'Sửa thông tin thành công',
                    showConfirmButton: false,
                    timer: 1500
                }).then(() => {
                    location.reload(true);
                });
            }
            else {
                Swal.fire({
                    icon: 'error',
                    title: 'Sửa thông tin thất bại',
                    showConfirmButton: false,
                    timer: 1500
                })
            }
            //$("#loader").hide();
        },
        error: function (xhr, status, p3, p4) {
            //$("#loader").hide();
            var err = "Error " + " " + status + " " + p3 + " " + p4;
            if (xhr.responseText)
                err = xhr.responseText;
            alert(err);
        }
    })
})


