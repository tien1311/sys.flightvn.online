function Save() {
    var ID = document.getElementById("ID").value;
    var IDBank = document.getElementById("Bank").value;
    var AccountName = document.getElementById("AccountName").value;
    var AccountNumber = document.getElementById("AccountNumber").value;
    var Position = document.getElementById("Position").value;
    var model = {
        ID: ID,
        IDBank: IDBank,
        AccountName: AccountName,
        AccountNumber: AccountNumber,
        Position: Position
    }
    var jsonData = JSON.stringify(model);
    $.ajax({
        type: "POST",
        processData: true,
        url: "../BankInfo/SaveEditBank",
        data: {
            data: jsonData
        },
        success: function (response) {
            if (response == true) {
                alert("Bạn đã chỉnh sửa thành công");
            }
            else {
                alert("Bạn đã chỉnh sửa không thành công");
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
}