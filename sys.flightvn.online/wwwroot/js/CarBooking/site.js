
(function ($) {
    var origin = window.location.origin;

    var loading = $('#loading-overlay');
    // Sự kiện khi yêu cầu Ajax bắt đầu
    $(document).ajaxStart(function () {
        loading.show();
    });
    // Dùng để kích hoạt lại các element khi load patialview
    $(document).ajaxComplete(function () {
        loading.hide();
    });


    // Hàm set value cho sessionStorage
    function setSession(jsonString, tab, page) {
        // Kiểm tra nếu jsonString là một object rỗng, thì gán null cho nó
        if ($.isEmptyObject(jsonString)) {
            jsonString = null;
        }
        var settings = {
            jsonString: jsonString,
            tab: tab,
            page: page
        };
        var data = JSON.stringify(settings);
        sessionStorage.setItem('settings', data);
    }
    setSession(null, "pending", 1)

    //Lưu data address vào trình duyệt
    //function list_pickup_address() {
    //    $.ajax({
    //        url: `/CarBooking/GetLocationData`,
    //        type: 'GET',
    //        dataType: 'json',
    //        success: function (data) {
    //            // Lưu dữ liệu vào localStorage
    //            // localStorage.setItem('list_airport', JSON.stringify(data));
    //            // Lưu dữ liệu vào localStorage session storage
    //            sessionStorage.setItem("list_pickup_address", JSON.stringify(data));
    //        },
    //        complete: function () {
    //            // Hàm này sẽ được gọi sau khi request hoàn tất, bất kể thành công hay thất bại
    //            load_data_dropdown_address_start();
    //        }
    //    });
    //};
    //list_pickup_address();

    function load_data_dropdown_location() {
        url = `../CarBooking/GetLocationData`;
        fetch(url)
            .then(response => response.text())
            .then(data => {
                //sessionStorage.setItem("list_pickup_address", JSON.stringify(data));
                load_data_dropdown_address_start(data);
            })
            .catch(error => {
                console.error('error:', error);
            });
    }
    load_data_dropdown_location();


    $("#dropdown_address_search").dropdown({
        onChange: function (value, text, $selectedItem) {
            //console.log('Selected Value:', value);
            //console.log('Selected Text:', text);

            // Get settings từ session và mặc định nếu không tồn tại
            var settings = JSON.parse(sessionStorage.getItem('settings'));

            // Get giá trị của page và size
            //var page = parseInt($(document).find("#partialContainer .pagination .item.active").data("value"));
            //var size = parseInt($(document).find('#dropdown_record_selected .text').text());
            //var tab = $('.ui.tab.segment.active').data("tab");

            // Cập nhật giá trị của keyword nếu chúng không tồn tại trong settings
            if (value !== "") {
                if (settings.jsonString == null) {
                    settings.jsonString = {};
                }
                settings.jsonString.keyword = text;
            }
            else {
                delete settings.jsonString['keyword'];
            }
            // Lưu settings vào session
            sessionStorage.setItem('settings', JSON.stringify(settings));
            load_partial_list_booking(settings.jsonString, settings.tab, 1);
        }
    });


    function load_data_dropdown_address_start(obj) {
        // Xóa toàn bộ dropdown_location_from
        $('#dropdown_address_search .menu').empty();
        // Đọc dữ liệu từ session storage
        //var list_pickup_address = sessionStorage.getItem("list_pickup_address");

        list_pickup_address = JSON.parse(obj);
        //Get dữ liệu trong session storage và gán vào dropdown
        if (list_pickup_address) {
            var item = $('<div class="item">')
                .attr('data-value', "")
                .text("Tất cả");
            // Append the new item to the dropdown menu
            $('#dropdown_address_search .menu').append(item);
            $.each(list_pickup_address, function (index, value) {
                // Create a new item element
                var element = $('<div class="item">')
                    .attr('data-value', value)
                    .text(value);
                // Append the new item to the dropdown menu
                $('#dropdown_address_search .menu').append(element);
            });
            //$('#dropdown_address_search').dropdown('refresh');
        }
    }


    //function activate_dropdown_record_selected() {
    //    var dropdown_record_selected = $(document).find('#dropdown_record_selected');
    //    var settings = sessionStorage.getItem('settings');
    //    settings = JSON.parse(settings);
    //    dropdown_record_selected.dropdown({
    //        onChange: function (value, text, $selectedItem) {
    //            //console.log('Selected Value:', value);
    //            //console.log('Selected Text:', text);
    //            // Get settings từ session và mặc định nếu không tồn tại
    //            var settings = JSON.parse(sessionStorage.getItem('settings')) || { jsonString: null };
    //            //var page = parseInt($(document).find("#partialContainer .pagination .item.active").data("value"));
    //            //var size = parseInt(text);
    //            var tab = $('.ui.tab.segment.active').data("tab");
    //            // Cập nhật giá trị của record nếu chúng không tồn tại trong settings
    //            //settings.record = size;

    //            // Lưu settings vào session
    //            sessionStorage.setItem('settings', JSON.stringify(settings));
    //            load_partial_list_booking(settings.jsonString, tab, 1);
    //        }
    //    });
    //    if (settings) {
    //        $(document).find('#dropdown_record_selected .text').text(settings.record);
    //        var selected_records = $(document).find('#dropdown_record_selected .menu').children();
    //        selected_records.removeClass("active selected");
    //        // Tìm phần tử con có data-value bằng settings.record và thêm lớp active selected
    //        var active_record = selected_records.filter(`[data-value="${settings.record}"]`);
    //        active_record.addClass("active selected");
    //    }
    //}
    /*activate_dropdown_record_selected();*/


    function load_partial_list_booking(jsonString, tab, page) {
        $.ajax({
            type: "POST",
            url: `../CarBooking/LoadPartialDataListBooking`,
            data: { jsonString: JSON.stringify(jsonString), tab: tab, page: page },
            success: function (response) {
                //console.log(response);
                //$('#partialContainer').html(response);
                var table = tab == "pending" ? "#table-pending" : "#table-finish";
                $(document).find(table).html(response);
            }
        });
    }


    //var pages = $(document).find("#partialContainer .pagination .item");
    //var pages_has_class = pages.filter('.active, .disabled');
    //var pages_not_class = pages.filter(':not(.active):not(.disabled)');
    //$(document).on('click', '#partialContainer .pagination .item.active, #partialContainer .pagination .item.disabled', function () {
    //    // Xử lý sự kiện khi click vào phần tử có class "active" hoặc "disabled"
    //});

    // Khi load lại #partialContainer cần sử dụng sự kiện ủy thác cho $(document) để kích hoạt lại sự kiện đã đăng ký bị mất;
    $(document).on('click', '#table-pending .pagination .item:not(.active):not(.disabled)', function () {
        // Get settings từ session và mặc định nếu không tồn tại
        //var size = parseInt($(document).find('#dropdown_record_selected .text').text());
        var settings = sessionStorage.getItem('settings');
        settings = JSON.parse(settings);
        settings.page = parseInt($(this).data('value'));
        sessionStorage.setItem('settings', JSON.stringify(settings));
        load_partial_list_booking(settings.jsonString, settings.tab, settings.page);
    });


    $(document).on('click', '#table-finish .pagination .item:not(.active):not(.disabled)', function () {
        // Get settings từ session và mặc định nếu không tồn tại
        //var size = parseInt($(document).find('#dropdown_record_selected .text').text());
        var settings = sessionStorage.getItem('settings');
        settings = JSON.parse(settings);
        settings.page = parseInt($(this).data('value'));
        sessionStorage.setItem('settings', JSON.stringify(settings));
        load_partial_list_booking(settings.jsonString, settings.tab, settings.page);
    });


    $('#date_calendar_carbooking').calendar({
        type: 'month', // Chọn 'month' cho việc chọn tháng và 'year' cho việc chọn năm
        text: {
            months: ['Tháng 1', 'Tháng 2', 'Tháng 3', 'Tháng 4', 'Tháng 5', 'Tháng 6', 'Tháng 7', 'Tháng 8', 'Tháng 9', 'Tháng 10', 'Tháng 11', 'Tháng 12'],
            monthsShort: ['Th1', 'Th2', 'Th3', 'Th4', 'Th5', 'Th6', 'Th7', 'Th8', 'Th9', 'Th10', 'Th11', 'Th12'],
            today: 'Hôm nay',
            now: 'Bây giờ',
            am: 'AM',
            pm: 'PM'
        },
        formatter: {
            month: 'MM/YYYY',
        },
        initialDate: new Date(),
        //minDate: new Date(), vô hiệu hóa để cho phép chọn tháng trước
        maxDate: new Date(new Date().getFullYear() + 1, 11, 31),
        onChange: function (date, text) {
            if (date) {
                // Get ngày đầu tháng
                var firstDayOfMonth = new Date(date.getFullYear(), date.getMonth(), 1);
                // Get ngày cuối tháng
                var lastDayOfMonth = new Date(date.getFullYear(), date.getMonth() + 1, 0);
                // Định dạng ngày thành chuỗi (vd: '2024/03/01 - 2024/03/31')
                var dayOfMonth = firstDayOfMonth.getFullYear() + '/' + (firstDayOfMonth.getMonth() + 1) + '/' + firstDayOfMonth.getDate() + ' - ' + lastDayOfMonth.getFullYear() + '/' + (lastDayOfMonth.getMonth() + 1) + '/' + lastDayOfMonth.getDate();

                // Get settings từ session và mặc định nếu không tồn tại
                var settings = JSON.parse(sessionStorage.getItem('settings'));

                // Get giá trị của page và size
                //var page = parseInt($(document).find("#partialContainer .pagination .item.active").data("value"));
                //var size = parseInt($(document).find('#dropdown_record_selected .text').text());
                var tab = $('.ui.tab.segment.active').data("tab");

                // Cập nhật giá trị của time nếu chúng không tồn tại trong settings
                if (settings.jsonString == null) {
                    settings.jsonString = {};
                }
                settings.jsonString.time = dayOfMonth;

                // Lưu settings vào session
                sessionStorage.setItem('settings', JSON.stringify(settings));
                load_partial_list_booking(settings.jsonString, tab, 1);
            }
        }
    });


    $('.ui.radio.checkbox').checkbox();
    $('.ui.radio.checkbox').on('click', function () {
        // Get the value of the clicked radio button
        var vat = $(this).find('input').val();
        // Get settings từ session và mặc định nếu không tồn tại
        var settings = JSON.parse(sessionStorage.getItem('settings'));

        // Get giá trị của page và size
        //var page = parseInt($(document).find("#partialContainer .pagination .item.active").data("value"));
        //var size = parseInt($(document).find('#dropdown_record_selected .text').text());
        var tab = $('.ui.tab.segment.active').data("tab");

        // Xóa vat
        if (vat !== "") {
            if (settings.jsonString == null) {
                settings.jsonString = {};
            }
            settings.jsonString.vat = vat;
        }
        else {
            delete settings.jsonString['vat'];
        }

        // Lưu settings vào session
        sessionStorage.setItem('settings', JSON.stringify(settings));
        load_partial_list_booking(settings.jsonString, tab, 1);
    });

    // Show popup content
    $('.popup').popup();

    document.addEventListener('DOMContentLoaded', function () {
        // Chọn phần tử con trực tiếp của #tab_carbooking
        var item = document.querySelector('#tab_carbooking > .item');
        var settings = sessionStorage.getItem('settings');
        settings = JSON.parse(settings);
        setSession(settings.jsonString, item.dataset.tab, 1);
        load_partial_list_booking(settings.jsonString, item.dataset.tab, 1);
    });


    var dropdown_selected_status = $('#dropdown_selected_status');
    dropdown_selected_status.dropdown({
        onChange: function (value, text, $selectedItem) {
            // Get settings từ session và mặc định nếu không tồn tại
            var settings = JSON.parse(sessionStorage.getItem('settings'));
            // Cập nhật giá trị của keyword nếu chúng không tồn tại trong settings
            if (value !== "") {
                if (settings.jsonString == null) {
                    settings.jsonString = {};
                }
                settings.jsonString.status = value;
            }
            else {
                //delete settings.jsonString['status'];
                settings.jsonString.status = null;
            }
            // Lưu settings vào session
            sessionStorage.setItem('settings', JSON.stringify(settings));
            load_partial_list_booking(settings.jsonString, settings.tab, 1);
        }
    });


    var image_success = "/images/success.png";
    var image_failure = "/images/fail.png";
    var dict = {
        "success": image_success,
        "failure": image_failure
    };
    function message(imagesUrl, response) {
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
            title: response,
            confirmButtonText: 'Đóng',
        }).then((result) => {
            if (result.isConfirmed) {
                window.location.reload();
            }
        });
    }


    $(document).on('click', '#btn_send_booking', function () {
        var id = $(this).data("value");
        var url = `../CarBooking/SendBooking`;
        // Gọi AJAX trước
        $.ajax({
            url: url,
            type: 'POST',
            contentType: 'application/json',
            //data: JSON.stringify({ id: id, settings: settings }),
            data: JSON.stringify({ id: id }),
            success: function (response) {
                $('#modal_booking_detail').modal('hide');
                var imagesUrl = response == "Đặt xe thành công!" ? dict.success : dict.failure;
                // Hiển thị thông báo
                message(imagesUrl,response)
            },
            error: function (xhr, status, error) {
                console.error('error:', error);
            },
            complete: function () {
                $('#modal_booking_detail').modal('hide');
            }
        });
    });


    $(document).on('input', '#otherFee', function (event) {
        // Format số với dấu phân cách hàng nghìn
        let formattedValue = Number(event.target.value.replace(/[^\d]/g, '')).toLocaleString('en-US');
        // Set giá trị đã định dạng vào input
        event.target.value = formattedValue;

        let unformattedValue = event.target.value.replace(/[^\d]/g, '');
        $(this).attr('data-originalValue', unformattedValue);

    })

    var save_other_fee = $('#saveOtherFeeBtn');
    save_other_fee.on('click', function () {
        var otherFee = $('#otherFee').attr('data-originalValue');
        var evcode = document.getElementById('header_booking_code').textContent;
        var reason = $('#other_fee_reason').val();
        $.ajax({
            url: '../CarBooking/UpdateOtherFee',
            data: { evcode: evcode, otherFee: otherFee, reason: reason },
            type: 'POST',
            success: function (rs) {
                if (rs.success) {
                    let newTotalPrice = rs.newTotalPrice;
                    document.getElementById('totalPriceValue').innerHTML = parseFloat(newTotalPrice).toLocaleString('en-US');
                    document.getElementById('totalPrice_Car_' + evcode.trim()).innerHTML = parseFloat(newTotalPrice).toLocaleString('en-US')
                    document.getElementById('otherFeeValue').innerHTML = parseFloat(otherFee).toLocaleString('en-US');
                    document.getElementById('other_fee_reason_Value').innerHTML = rs.reason;

                    Swal.fire({
                        imageUrl: "/images/success.png",
                        imageWidth: 100,
                        imageHeight: 100,
                        title: rs.message,
                        confirmButtonText: 'Đóng',
                    }).then((result) => {
                        if (result.isConfirmed) {
                            $('#saveOtherFeeBtn-Close').click();
                        }
                    });
                }
                else {
                    Swal.fire({
                        imageUrl: "/images/fail.png",
                        imageWidth: 100,
                        imageHeight: 100,
                        title: rs.message,
                        confirmButtonText: 'Đóng',
                    });
                }
            }
        })
    });

    $(document).on('click', '.btnApproveChuyenKhoan', function (e) {
        e.preventDefault();
        var evcode = $(this).data('evcode');

        // Hiển thị hộp thoại xác nhận
        Swal.fire({
            title: 'Bạn có thực sự muốn cập nhật trạng thái?',
            text: 'Bạn sẽ không thể hoàn tác sau khi thực hiện hành động này',
            showCancelButton: true,
            confirmButtonText: 'Có',
            cancelButtonText: 'Không',
            icon: 'warning'
        }).then((result) => {
            if (result.isConfirmed) {
                // Người dùng chọn "Có", thực hiện hàm UpdateThanhToanChuyenKhoan
                $.ajax({
                    url: '../CarBooking/UpdateThanhToanChuyenKhoan',
                    data: { evcode: evcode },
                    type: 'POST',
                    success: function (rs) {
                        if (rs.success) {
                            document.getElementById('payment-status-' + evcode).innerHTML = "Thanh toán thành công";
                            let btnApprovedChuyenKhoan = document.getElementById('btnApproveChuyenKhoan_' + evcode);
                            btnApprovedChuyenKhoan.style.display = "none";
                            let selectElement = document.querySelector(`select[data-evcode="${evcode}"]`);
                            selectElement.value = "CONFIRM";
                            let btnStatusEnViet = document.getElementById('btn-status-enviet_' + evcode);
                            btnStatusEnViet.classList.add("btn-success");
                            btnStatusEnViet.innerHTML = "Đã thanh toán";

                            Swal.fire({
                                imageUrl: "/images/success.png",
                                imageWidth: 100,
                                imageHeight: 100,
                                title: rs.message,
                                confirmButtonText: 'Đóng',
                            });
                        } else {
                            Swal.fire({
                                imageUrl: "/images/fail.png",
                                imageWidth: 100,
                                imageHeight: 100,
                                title: rs.message,
                                confirmButtonText: 'Đóng',
                            });
                        }
                    }
                });
            }
        });
    });



    $(document).on('click', '.btn_sync_booking', function () {
        var id = $(this).data('value');
        var url = `../CarBooking/SyncStatusBooking?id=${id}`;
        $.ajax({
            url: url,
            type: 'GET',
            success: function (response) {
                var imagesUrl = response == "Cập nhật trạng thái thành công!" ? dict.success : dict.failure;
                message(imagesUrl, response);
                var settings = JSON.parse(sessionStorage.getItem('settings'));
                load_partial_list_booking(settings.jsonString, settings.tab, settings.page);
            },
            error: function (xhr, status, error) {
                console.error('error:', error);
            }
        });
    });


    $(document).on('change', '#status_enviet',function () {
        var selectedValue = $(this).val();
        $('#btn_status_booking').data('status', selectedValue);
    });

    $(document).on('click', '#btn_status_booking', function () {
        var id = $(this).data('value');
        var status = $('#btn_status_booking').data('status');
        var url = `../CarBooking/ChangeStatus`;
        $.ajax({
            url: url,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ id: id, status: status }),
            success: function (data) {
                var imagesUrl = data === "Đổi trạng thái thành công!" ? dict.success : dict.failure;
                message(imagesUrl, data);
            },
            error: function (error) {
                alert("Có lỗi xảy ra, vui lòng liên hệ IT");
                console.error('error:', error);
            }
        });
    });

    var btn_cancel_booking = $('#btn_cancel_booking');
    btn_cancel_booking.on('click', function () {
        var cancellation_reason = $('#cancellation_reason');
        var evcode = cancellation_reason.data('value');
        var reason = cancellation_reason.val();
        var url = `../CarBooking/CancelBooking`;
        if (reason === "") {
            alert("Vui lòng nhập lý do");
        } else {
            $.ajax({
                url: url,
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({ evcode: evcode, reason: reason }),
                success: function (data) {
                    $('#cancel_booking_detail').modal('hide');
                    var imagesUrl = data.msg === "Hủy chuyến thành công!" ? dict.success : dict.failure;
                    // Hiển thị thông báo
                    message(imagesUrl, data.msg);
                },
                error: function (error) {
                    console.error('error:', error);
                }
            });
        }
    });

    $(document).on("click", ".evcode", function () {
        var evcode = $(this).text();
        var cancellation_reason = $('#cancellation_reason');
        cancellation_reason.attr('data-value', evcode);
        var url = `../CarBooking/LoadPartialModalBookingDetail`;
        fetch(url, {
            method: 'POST', 
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({evcode: evcode})
        })
        .then(response => response.text())
        .then(data => {
            //console.log(data);
            $('#modal_booking_detail').html(data);         
        })
        .catch(error => {
            console.error('error:', error);
        });
    });

    $(document).on('click', '#btn_payment_status', function (e) {
        var idbooking = $(this).data('value');
        var payment_type = $('#payment_type').val();
        var url = `../CarBooking/UpdatePayment`;
        fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ idbooking: idbooking, payment_type: payment_type })
        })
        .then(response => response.text())
        .then(data => {
            var imagesUrl = data == "Cập nhật phương thức thanh toán thành công" ? dict.success : dict.failure;
            message(imagesUrl, data);
        })
        .catch(error => {
            console.error('error:', error);
        });
    });


    $(document).on('click', '#btn_get_link', function (e) {
        e.preventDefault();
        var id = $(this).data('value');
        var input = $(document).find('#input_link_payment');
        var url = `../CarBooking/GetLinkPayment`;
        fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ id: id })
        })
        .then(response => response.text())
        .then(data => {
            input.val(data);
        })
        .catch(error => {
            console.error('error:', error);
        });
    });


    $(document).on('click', '#btn_copy_link', function (e) {
        e.preventDefault();
        var input = $(document).find('#input_link_payment')[0];
        // Chọn toàn bộ văn bản trong input
        input.select();
        input.setSelectionRange(0, 99999);
        // Sao chép văn bản đã chọn vào clipboard
        document.execCommand('copy');
    });


})(jQuery);
