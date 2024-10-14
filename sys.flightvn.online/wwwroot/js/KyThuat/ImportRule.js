var expanded = false;

function showCheckboxes() {
    var checkboxes = document.getElementById("checkboxes");
    if (!expanded) {
        checkboxes.style.display = "block";
        expanded = true;
    } else {
        checkboxes.style.display = "none";
        expanded = false;
    }
}
$('#selectall').click(function () {
    if ($(this).is(':checked')) {
        $('#checkboxes input').attr('checked', true);
    } else {
        $('#checkboxes input').attr('checked', false);
    }
});
function ImportRule() {
    var selectedOptions = document.querySelectorAll('#checkboxes input[type=checkbox]:checked');
    var table = document.getElementById("gridRules_RuleDetails");
    var tbody = document.querySelector("#gridRules_RuleDetails tbody");
    for (var i = 0; i < selectedOptions.length; i++) {
        if (selectedOptions[i].id != "selectall") {
            var length = table.rows.length;
            // Insert a new row at the end of the table
            var newRow = tbody.insertRow();

            // Add cells to the row
            var cell1 = newRow.insertCell();
            var cell2 = newRow.insertCell();
            var cell3 = newRow.insertCell();

            // Set the content of each cell
            cell1.textContent = Number(length);
            cell2.textContent = selectedOptions[i].value;
            cell3.textContent = selectedOptions[i].id;
            cell3.style.display = 'none';
        }
    }
}
function SaveImportRule() {
    var table = document.getElementById("gridRules_RuleDetails");
    var PartnerID = document.getElementById("PartnerID").value;
    var data = [];
    for (var i = 1; i < table.rows.length; i++) {
        var row = table.rows[i];
        var rowData = {
            RuleDetailID: row.cells[2].textContent.trim()
        };
        data.push(rowData);
    }

    var model = {
        PartnerID: PartnerID,
        ListRuleDetails: data,
    }
    var jsonData = JSON.stringify(model);
    $.ajax({
        type: "POST",
        processData: true,
        url: "../FareRules/SaveImportRule",
        data: {
            data: jsonData
        },
        success: function (response) {
            if (response == true) {
                alert("Import dữ liệu thành công");
            }
            else {
                alert("Import dữ liệu thất bại");
            }
            window.location.href = "../FareRules/Rules_Partners?&i=12";
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}