$('input[name="FlightDate1"]').daterangepicker({
    singleDatePicker: true,
    showDropdowns: true,
    locale: {
        format: 'DD/MM/YYYY'
    }
});
$('input[name="FlightDate2"]').daterangepicker({
    singleDatePicker: true,
    showDropdowns: true,
    locale: {
        format: 'DD/MM/YYYY'
    }
});
function KindTripChange() {
    var Kind = document.getElementById("KindTrip").value;
    var x = document.getElementById("chieuve");
    if (Kind == "MC") {
        x.style.display = "none";

    }
    else {
        x.style.display = "block";
    }
}
function formatNumber(id) {
    var number = document.getElementById(id).value
    number = number.replaceAll(",", "");
    x = number.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    var tong = x1 + x2;
    document.getElementById(id).value = x1 + x2;

    return x1 + x2;
}


$("#SaveFlight").click(function () {

    var Hang = document.getElementById("Airline").value;
    var HanhTrinh = document.getElementById("Itinerary").value;
    var SoLuong = document.getElementById("NumberOfGuests").value;
    var DonGia = document.getElementById("Price").value;
    var DonGiaGiam = document.getElementById("PriceAgent").value;
    var Loai = document.getElementById("KindTrip").value;
    var Specification = document.getElementById("Specification").value;

    var active = document.getElementById("Active").checked;
    var MaChuyenBayDi = document.getElementById("FlightNumber1").value;
    var GioBayDi = document.getElementById("Hour1").value;
    var NgayBayDi = document.getElementById("single_cal1").value;
    var MaChuyenBayVe = document.getElementById("FlightNumber2").value;
    var GioBayVe = document.getElementById("Hour2").value;
    var NgayBayVe = document.getElementById("single_cal2").value;
    var Donvi = document.getElementById("Donvi").value;
    //var DieuKien = document.getElementById("Condition").value;
    var DieuKien = CKEDITOR.instances.Condition.getData();

    $.ajax({
        type: "GET",
        url: "../KyThuat/SaveFlightData",
        data: { hang: Hang, hanhTrinh: HanhTrinh, soLuong: SoLuong, donGia: DonGia, donGiaGiam: DonGiaGiam, loai: Loai, maChuyenBayDi: MaChuyenBayDi, gioBayDi: GioBayDi, ngayBayDi: NgayBayDi, maChuyenBayVe: MaChuyenBayVe, gioBayVe: GioBayVe, ngayBayVe: NgayBayVe, hoatDong: active, dieuKien: DieuKien, Specification: Specification, Donvi: Donvi },
        success: function (response) {

            alert("Lưu thành công");
            location.reload();
        },
        failure: function (response) {

            alert(response.responseText);
        },
        error: function (response) {

            alert(response.responseText);
        }
    });
});