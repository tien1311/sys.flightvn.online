$("#Doanhso .btnDoanhSo").click(function () {
    var makh = document.getElementById("MaKH").value;
    var nam = document.getElementById("Nam").value;

    $.ajax({
        type: "POST",
        url: "../KinhDoanh/SearchDoanhSo",
        data: { MaKH: makh, Nam: nam },
        success: function (response) {
            $("#tbody").empty();
            var len = response.length;

            for (var i = 0; i < len; i++) {
                var tr_str = "<tr>" +
                    "<td align='left'>" + response[i].stt + "</td>" +
                    "<td align='left'>" + response[i].thang + "/" + response[i].nam + "</td>" +
                    "<td align='left'>" + response[i].maKH + "</td>" +
                    "<td align='left' style='font-weight:bold; color: red;'>" + formatNumber(response[i].tong) + "</td>" +
                    "<td align='left'>" + formatNumber(response[i].vn) + "</td>" +
                    "<td align='left'>" + formatNumber(response[i].vj) + "</td>" +
                    "<td align='left'>" + formatNumber(response[i].qh) + "</td>" +
                    "<td align='left'>" + formatNumber(response[i].iata) + "</td>" +
                    "<td align='left'>" + formatNumber(response[i].khac) + "</td>"
                "</tr>";

                $("#gridDoanhSo tbody").append(tr_str);
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


function formatNumber(number) {
    number = number.toFixed(0) + '';
    x = number.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
}