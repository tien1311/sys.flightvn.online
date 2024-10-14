const orderHeaderId = document.getElementById("ID").value;

function LoadViewComponent() {
    $.post(
        `../Voucher/LoadOrderStatusInDetailBooking`,
        { orderHeaderId: orderHeaderId },
        (data, status) => {
            if (status == "success") {
                // Xóa các phần tử được load từ đầu
                $("#loadStatusDetailContainer").nextAll().remove();
                $("#loadStatusDetailContainer").after(data);
            } else {
                console.error("Failed to load content:", xhr.status, xhr.statusText);
            }
        }
    );
    $(`#loadStatusBookingContainer-${orderHeaderId}`).load(`../Voucher/LoadOrderStatusInBooking`, { orderHeaderId: orderHeaderId });
}
function UpdateStatusBooking() {
    // Lấy ra status cũ
    let oldOrderStatusId = document.getElementById('Current_Status').dataset.currentStatusId
    // lấy ra note và status id mới
    let newStatusIdValue = document.getElementById('Tinhtrang').value
    let newNoteStatusValue = document.getElementById('Note').value;
    // disabled button
    document.getElementById('btn_cancel_booking').disabled = true;
    document.getElementById('btn_Change_Status_booking').disabled = true;
    CustomSweetAlert_Confirm('Xác nhận cập nhật trạng thái', 'Email sẽ được gửi đến khách hàng').then((result) => {
        if (result.isConfirmed) {
            $.post(
                `../Voucher/UpdateStatusBooking`,
                {
                    OrderStatusId: newStatusIdValue,
                    Id: orderHeaderId,
                    NoteStatus: newNoteStatusValue,
                    oldStatusId: oldOrderStatusId
                },
                (data) => {
                    // Gọi lại ViewComponent
                    LoadViewComponent();
                    // Enable button
                    document.getElementById('btn_cancel_booking').disabled = false;
                    document.getElementById('btn_Change_Status_booking').disabled = false;
                    if (data.success == true) {
                        if (newStatusIdValue == 4 || newStatusIdValue == 5) {
                            // disabled button
                            document.getElementById('btn_cancel_booking').disabled = true;
                            document.getElementById('btn_Change_Status_booking').disabled = true;
                        }
                        CustomSweetAlert_Success(data.message);
                    } else {
                        CustomSweetAlert_Error(data.message);
                    }
                }
            )
        } else {
            // Enable button
            document.getElementById('btn_cancel_booking').disabled = false;
            document.getElementById('btn_Change_Status_booking').disabled = false;
        }
    })
}

//region Handle Event

// Hủy Booking
document.getElementById('btn_cancel_booking').onclick = (e) => {
    // disabled button
    document.getElementById('btn_cancel_booking').disabled = true;
    document.getElementById('btn_Change_Status_booking').disabled = true;
    CustomSweetAlert_Confirm('Xác nhận hủy Booking', 'Email hủy Booking sẽ được cập nhật tới khách hàng', true).then((result) => {
        if (result.isConfirmed) {
            // Lấy ra status cũ
            let oldOrderStatusId = document.getElementById('Current_Status').dataset.currentStatusId
            // Lấy giá trị input
            const reason = result.value.trim();
            let newNoteStatusValue = document.getElementById('Note').value;
            // Lấy ra NoteStatus Mới 
            $.post(
                `../Voucher/CancelBooking`,
                {
                    Id: orderHeaderId,
                    NoteStatus: newNoteStatusValue,
                    oldStatusId: oldOrderStatusId,
                    CancelReason: reason,
                },
                (data) => {
                    LoadViewComponent();
                    document.getElementById('btn_cancel_booking').disabled = false;
                    document.getElementById('btn_Change_Status_booking').disabled = false;
                    if (data.success) {
                        CustomSweetAlert_Success(data.message)
                        document.getElementById('btn_cancel_booking').disabled = true;
                        document.getElementById('btn_Change_Status_booking').disabled = true;
                    } else {
                        // enable button
                        CustomSweetAlert_Error(data.message)
                    }
                }
            )
        } else {
            document.getElementById('btn_cancel_booking').disabled = false;
            document.getElementById('btn_Change_Status_booking').disabled = false;
        }
    })
}
//endregion