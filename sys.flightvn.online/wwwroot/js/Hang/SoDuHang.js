$(document).ready(function () {
    $('a[data-toggle="tab"]').on('show.bs.tab', function (e) {
        localStorage.setItem('activeTab', $(e.target).attr('href'));
    });
    var activeTab = localStorage.getItem('activeTab');
    if (activeTab) {
        $('#myTab a[href="' + activeTab + '"]').tab('show');
    }
});
function clickABC() {
    for (var i = 1; i <= 4; i++) {
        var Mien = "";
        if (i == 1) {
            Mien = "UJU";
        }
        else {
            if (i == 2) {
                Mien = "JPQ";
            }
            else {
                if (i == 3) {
                    Mien = "FHQ";
                }
                else {
                    Mien = "LXQ";
                }
            }
        }
        $.ajax({
            type: "POST",
            url: "../Hang/GetSoDuHang",
            data: { mien: Mien },
            success: function (response) {

            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            }
        });
    }
}