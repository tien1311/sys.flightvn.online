(function ($) {
    var origin = window.location.origin;

    function validation_radio(type, input_selector) {
        var input_element = $(input_selector);
        if (type == "%") {
            input_element.val('');
            input_element.attr('type', 'number');
            input_element.attr('placeholder', 'Ví dụ: 10');
            input_element.attr('min', 1);
            input_element.attr('max', 100);
        } else {
            input_element.val('');
            input_element.attr('type', 'text');
            input_element.attr('placeholder', 'Ví dụ: 10,000');
            input_element.removeAttr('min');
            input_element.removeAttr('max');
        }
        //validation
        input_element.on('input', function (e) {
            if (type != "VND") {
                // Khi loại không phải là VND, xóa validation
                e.target.setCustomValidity('');
                return;
            }
            let value = e.target.value;
            let input_element = e.target;
            // Xóa hết dấu phẩy hiện tại trong chuỗi
            value = value.replace(/,/g, '');

            // Kiểm tra xem giá trị có phải là số hay không
            if (isNaN(value) || value === '') {
                input_element.setCustomValidity('Vui lòng nhập một số hợp lệ.');
                input_element.reportValidity();
                return;
            } else {
                input_element.setCustomValidity('');
            }
            // Chuyển đổi chuỗi thành số
            let number_value = Number(value);
            // Kiểm tra giá trị nhỏ nhất là 1,000
            if (number_value < 1000) {
                input_element.setCustomValidity('Giá trị nhập vào tối thiểu 1,000.');
                input_element.reportValidity();
                return;
            } else {
                input_element.setCustomValidity('');
            }
            // Định dạng lại thành chuỗi với dấu phẩy
            input_element.value = number_value.toLocaleString('en');
        });
    }

    var type = $('input[name="type"]:checked').val();
    $('input[name="type"]').on('change', function () {
        type = $(this).val();
        $('input[name="type"]').prop('checked', false);
        $(this).prop('checked', true);
        validation_radio(type, '#price_coupon');
    });


    var type_value = $('input[name="type_value"]:checked').val();
    $('input[name="type_value"]').on('change', function () {
        type_value = $(this).val();
        $('input[name="type_value"]').prop('checked', false);
        $(this).prop('checked', true);
        validation_radio(type_value, '#value_price_coupon');
    });


    $('#form_add_counpon').on('submit', function (event) {
        event.preventDefault(); 
        var url = $(this).attr("action");
        var code = $('#code_coupon').val();
        var price = $('#price_coupon').val();
        price = price.replace(/,/g, '');
        price = Number(price);
        var type = $('input[name="type"]:checked').val();
        fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ code: code, price: price, type: type })
        })
        .then(response => response.text())
        .then(data => {
            var imagesUrl = data == "Thêm mã giảm giá thành công!" ? "/images/success.png" : "/images/fail.png";
            const swalWithBootstrapButtons = Swal.mixin({
                customClass: {
                    confirmButton: "btn btn-outline-primary btn-block",
                    cancelButton: "btn btn-danger"
                },
                buttonsStyling: false
            });
            swalWithBootstrapButtons.fire({
                imageUrl: imagesUrl,
                imageWidth: 100,
                imageHeight: 100,
                text: data,
                confirmButtonText: 'Đóng',
            }).then((result) => {
                if (result.isConfirmed) {
                    location.reload();
                }
            });
        })
        .catch(error => {
            console.error('error:', error);
        });
    });

    $('.btn_delete_coupon').on('click', function (event) {
        var id = $(this).data('id');
        var url = origin + `/CarBooking/DeleteCoupon`;
        fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ id: id })
        })
        .then(response => response.text())
        .then(data => {
            var imagesUrl = data == "Xóa mã giảm giá thành công!" ? "/images/success.png" : "/images/fail.png";
            const swalWithBootstrapButtons = Swal.mixin({
                customClass: {
                    confirmButton: "btn btn-outline-primary btn-block",
                    cancelButton: "btn btn-danger"
                },
                buttonsStyling: false
            });
            swalWithBootstrapButtons.fire({
                imageUrl: imagesUrl,
                imageWidth: 100,
                imageHeight: 100,
                text: data,
                confirmButtonText: 'Đóng',
            }).then((result) => {
                if (result.isConfirmed) {
                    location.reload();
                }
            });
        })
        .catch(error => {
            console.error('error:', error);
        });
    });


    $('.btn_edit_coupon').on('click', function (event) {
        var id = $(this).data("id");
        var code = $(this).data("code");
        var price = $(this).data("price");
        var type = $(this).data("type");
        validation_radio(type, '#value_price_coupon');
        if (type == "%") {
            $('input[name="type_value"][value="%"]').prop('checked', true);
        }
        else
        {
            $('input[name="type_value"][value="VND"]').prop('checked', true);
        } 
        $('#value_id_coupon').val(id);
        $('#value_code_coupon').val(code);
        $('#value_price_coupon').val(price);
    });


    $('#form_edit_counpon').on('submit', function (event) {
        event.preventDefault();
        var url = $(this).attr("action");
        var id = $('#value_id_coupon').val();
        var code = $('#value_code_coupon').val();
        var type = $('input[name="type_value"]:checked').val();
        var price = $('#value_price_coupon').val();
        price = price.replace(/,/g, '');
        price = Number(price);
        fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ id: id, code: code, type: type, price: price })
        })
        .then(response => response.text())
        .then(data => {
            var imagesUrl = data == "Cập nhật mã giảm giá thành công!" ? "/images/success.png" : "/images/fail.png";
            const swalWithBootstrapButtons = Swal.mixin({
                customClass: {
                    confirmButton: "btn btn-outline-primary btn-block",
                    cancelButton: "btn btn-danger"
                },
                buttonsStyling: false
            });
            swalWithBootstrapButtons.fire({
                imageUrl: imagesUrl,
                imageWidth: 100,
                imageHeight: 100,
                text: data,
                confirmButtonText: 'Đóng',
            }).then((result) => {
                if (result.isConfirmed) {
                    location.reload();
                }
            });
        })
        .catch(error => {
            console.error('error:', error);
        });
    });

    $('.check_coupon_apply').on('click', function () {
        var isChecked = $(this).prop('checked');
        var id = $(this).data('id');
        var url = origin + `/CarBooking/ApplyCoupon`;
        if (isChecked) {
            fetch(url, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ id: id })
            })
            .then(response => response.text())
            .then(data => {
                var imagesUrl = data == "Áp dụng mã giảm giá thành công!" ? "/images/success.png" : "/images/fail.png";
                const swalWithBootstrapButtons = Swal.mixin({
                    customClass: {
                        confirmButton: "btn btn-outline-primary btn-block",
                        cancelButton: "btn btn-danger"
                    },
                    buttonsStyling: false
                });
                swalWithBootstrapButtons.fire({
                    imageUrl: imagesUrl,
                    imageWidth: 100,
                    imageHeight: 100,
                    text: data,
                    confirmButtonText: 'Đóng',
                }).then((result) => {
                    if (result.isConfirmed) {
                        location.reload();
                    }
                });
            })
            .catch(error => {
                console.error('error:', error);
            });
        } 
    });


})(jQuery);
