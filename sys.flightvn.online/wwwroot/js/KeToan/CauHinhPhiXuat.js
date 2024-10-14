$("#datatable1 .UpdatePhiXuat").click(function () {
    var rowjQuery = $(this).closest("tr");
    var currentIndex = rowjQuery[0].rowIndex;
    var ID = document.getElementById("datatable1").rows[currentIndex].cells[0].innerHTML;
    var Price = document.getElementById("datatable1").rows[currentIndex].cells[2].getElementsByTagName("input")[0].value;
    var ExchangeRate = document.getElementById("datatable1").rows[currentIndex].cells[3].getElementsByTagName("input")[0].value;
    var Amount = document.getElementById("datatable1").rows[currentIndex].cells[4].getElementsByTagName("input")[0].value;


    $.ajax({
        type: "POST",
        url: "../KeToan/UpdatePhiXuat",
        data: {
            ID: ID,
            Price: Price,
            ExchangeRate: ExchangeRate,
            Amount: Amount
        },
        success: function (response) {
            if (response == true) {
              
                Swal.fire({
                    icon: 'success',
                    title: 'Sửa thông tin thành công',
                    showConfirmButton: false,
                    timer: 2000
                })
                setTimeout(function () {
                    location.reload(true);
                }, 1000);
            }
            else {
                Swal.fire({
                    icon: 'error',
                    title: 'Sửa thông tin thất bại',
                    showConfirmButton: true,
                    timer: 2000
                })
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

function ThanhTien(obj) {
    
    const currentIndex = obj.parentNode.parentNode.rowIndex;
    var Price = document.getElementById("datatable1").rows[currentIndex].cells[2].getElementsByTagName("input")[0].value.replace(/\$|\,/g, '');
    var ExchangeRate = document.getElementById("datatable1").rows[currentIndex].cells[3].getElementsByTagName("input")[0].value.replace(/\$|\,/g, '');
    var Amount = Price * ExchangeRate;
    document.getElementById("datatable1").rows[currentIndex].cells[4].getElementsByTagName("input")[0].value = Amount.toLocaleString('en-US', { style: 'decimal', minimumFractionDigits: 2, maximumFractionDigits: 2 });
}
