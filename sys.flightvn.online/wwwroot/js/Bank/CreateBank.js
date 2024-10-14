function Save() {
    var IDBank = document.getElementById("Bank").value;
    var AccountName = document.getElementById("AccountName").value;
    var AccountNumber = document.getElementById("AccountNumber").value;
    var Position = document.getElementById("Position").value;
    var model = {
        IDBank: IDBank,
        AccountName: AccountName,
        AccountNumber: AccountNumber,
        Position: Position
    }
    var jsonData = JSON.stringify(model);
    $.ajax({
        type: "POST",
        processData: true,
        url: "../BankInfo/SaveCreateBank",
        data: {
            data: jsonData
        },
        success: function (response) {
            if (response == true) {
                alert("Bạn đã tạo mới thành công");
            }
            else {
                alert("Bạn đã tạo mới không thành công");
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