$(document).ready(function () {
    $('body').on('change', '.btnActive', function () {
        var checkbox = $(this);
        var id = checkbox.data('id');

        $.ajax({
            url: '../PaymentGateway/IsPaymentMethodActive',
            type: 'POST',
            data: { id: id },
            success: function (result) {
                if (result.success) {
                    checkbox.attr("checked", "checked");
                }
                else {
                    checkbox.removeAttr('checked');
                }
            }
        })
    });

    $('body').on('change', '.paymentMethodImage', function () {
        var id = $(this).data('id');
        var formData = new FormData();
        var fileInput = $(this);
        formData.append('PaymentId', id);
        formData.append('paymentMethodImage', fileInput.prop('files')[0]);

        $.ajax({
            url: '../PaymentGateway/ChangePaymentMethodImage',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (result) {
                if (result.success) {
                    location.reload();
                    alert(result.message);
                }
                else {
                    alert(result.message);
                }
            },
        });
    });

    $('body').on('click', '#btnSavePrice', function () {
        var id = $(this).data('id');
        var row = $(this).closest('tr');

        var FixedCost = row.find("#fixedcost").val();
        var Percent = row.find("#percent").val();
        var name = row.find("#name").val();

        var formData = new FormData();
        formData.append('id', id);
        formData.append('fixedCost', FixedCost);
        formData.append('percent', Percent);
        formData.append('name', name);

        $.ajax({
            url: '../PaymentGateway/EditPrice',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (result) {
                if (result.success) {
                    location.reload();
                    alert(result.message);
                }
                else {
                    alert(result.message);
                }
            },
        });
    });
});
