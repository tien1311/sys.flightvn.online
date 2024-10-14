
// Biểu đồ số lượng trạng thái
// var trangThaiChartData = @Html.Raw(Json.Serialize(Model));
var trangThaiChartData = JSON.parse(document.getElementById('EnVietChartData').value);

var chartContentTinhTrangElement = document.querySelector(".chart_content_tinhtrang");
var chartContentDoanhThuElement = document.querySelector(".chart_content_doanhthu");

if (trangThaiChartData == null) {
    chartContentTinhTrangElement.style.display = "none";
    chartContentDoanhThuElement.style.display = "none";

}
else {
    chartContentTinhTrangElement.style.display = "block";
    chartContentDoanhThuElement.style.display = "block";

}

// Tạo một đối tượng Map để nhóm dữ liệu theo name
var dataMap = new Map();

// Duyệt qua mỗi mục trong trangThaiChartData và thêm dữ liệu vào Map
trangThaiChartData.forEach(item => {
    if (!dataMap.has(item.name)) {
        // Nếu name chưa tồn tại trong Map, thêm name vào Map với một mảng chứa các giá trị "slHuy, slHoanThanh,..."
        dataMap.set(item.name, {
            // Dữ liệu chung của các booking
            slHuy: item.slHuy,
            slHoanThanh: item.slHoanThanh,
            slMoi: item.slMoi,

            // Dữ liệu của TourBooking
            slDaTiepNhan: item.slDaTiepNhan,
            slDaGiuCho: item.slDaGiuCho,
            slDaDatCoc: item.slDaDatCoc,

            //// Dữ liệu của VISA
            slDaNhanBooking: item.slDaNhanBooking,
            slDaTiepNhanHoSo: item.slDaTiepNhanHoSo,
            slDangXuLyHoSo: item.slDangXuLyHoSo,
            slYeuCauBoSungHoSo: item.slYeuCauBoSungHoSo,
            slNopHoSoThanhCong: item.slNopHoSoThanhCong,
            slDangXetDuyetHoSo: item.slDangXetDuyetHoSo,

            // Thêm các thuộc tính khác tương ứng nếu cần, hoặc nếu sau này có bổ sung
            slHoanTatThanhToan: item.slHoanTatThanhToan,

            // Dữ liệu của CarBooking
            slChoGui: item.slChoGui,
            slChoXuLy: item.slChoXuLy,
            slChoXacNhan: item.slChoXacNhan,
            slXacNhanChuyen: item.slXacNhanChuyen,
            slTuChoi: item.slTuChoi

        });
    } else {
        // Nếu name đã tồn tại trong Map, cộng dồn các giá trị "slHuy, slHoanThanh,..." với các giá trị tương ứng trong Map 
        var existingData = dataMap.get(item.name); // Nó có nghĩa là cộng dồn các số lượng Huỷ, Hoàn Thành,... của các tháng mà mình đã lọc

        // Dữ liệu chung của các booking
        existingData.slHuy += item.slHuy;
        existingData.slHoanThanh += item.slHoanThanh;
        existingData.slMoi += item.slMoi;

        // Dữ liệu của TourBooking
        existingData.slDaTiepNhan += item.slDaTiepNhan;
        existingData.slDaGiuCho += item.slDaGiuCho;
        existingData.slDaDatCoc += item.slDaDatCoc;

        //// Dữ liệu của VISA
        existingData.slDaNhanBooking += item.slDaNhanBooking;
        existingData.slDaTiepNhanHoSo += item.slDaTiepNhanHoSo;
        existingData.slDangXuLyHoSo += item.slDangXuLyHoSo;
        existingData.slYeuCauBoSungHoSo += item.slYeuCauBoSungHoSo;
        existingData.slNopHoSoThanhCong += item.slNopHoSoThanhCong;
        existingData.slDangXetDuyetHoSo += item.slDangXetDuyetHoSo;
        // Cộng dồn các thuộc tính khác tương ứng nếu cần, hoặc nếu sau này có bổ sung
        existingData.slHoanTatThanhToan += item.slHoanTatThanhToan;

        //// Dữ liệu của Car Booking
        existingData.slChoGui += item.slChoGui;
        existingData.slChoXuLy += item.slChoXuLy;
        existingData.slChoXacNhan += item.slChoXacNhan;
        existingData.slXacNhanChuyen += item.slXacNhanChuyen;
        existingData.slTuChoi += item.slTuChoi

    }
});
// Tạo các label từ keys của Map và tính tổng các giá trị tương ứng
var trangThaiLabels = Array.from(dataMap.keys());
var trangThaiData = Array.from(dataMap.values()).map(item => { // Anh em thêm ở trên rồi thì nhớ thêm ở dưới này, để lấy dữ liệu ra
    return [
        item.slHuy,
        item.slHoanThanh,
        item.slMoi,
        item.slDaTiepNhan,
        item.slDaGiuCho,
        item.slDaDatCoc,
        item.slDaNhanBooking,
        item.slDaTiepNhanHoSo,
        item.slDangXuLyHoSo,
        item.slYeuCauBoSungHoSo,
        item.slNopHoSoThanhCong,
        item.slDangXetDuyetHoSo,
        // Thêm ở dưới nha
        item.slHoanTatThanhToan,
        item.slChoGui,
        item.slChoXuLy,
        item.slChoXacNhan,
        item.slXacNhanChuyen,
        item.slTuChoi
    ];
});


trangThaiLabels.forEach((label, index) => {

    var listLabels = [               // A/e nhớ khi thêm trạng thái hay tình trạng nào mới, thì nhớ thêm vào list này nha
        "Số lượng Huỷ",
        "Số lượng Hoàn thành",
        "Số lượng Mới",
        "Số lượng Đã tiếp nhận",
        "Số lượng Đã giữ chỗ",
        "Số lượng Đã đặt cọc",
        "Số lượng Đã nhận Booking",
        "Số lượng Đã tiếp nhận hồ sơ",
        "Số lượng Đang xử lý hồ sơ",
        "Số lượng Yêu cầu bổ sung hồ sơ",
        "Số lượng Nộp hồ sơ thành công",
        "Số lượng Đang xét duyệt hồ sơ",
        "Số lượng Hoàn tất thanh toán",
        "Số lượng Chờ gửi",
        "Số lượng Chờ xử lý",
        "Số lượng Chờ xác nhận",
        "Số lượng Xác nhận chuyển",
        "Số lượng Từ chối"
    ];

    // Lấy dữ liệu cho biểu đồ hiện tại từ Map
    var currentData = trangThaiData[index];

    // Biến trung gian để lọc nhãn và dữ liệu
    var filteredLabels = [];
    var filteredData = [];

    // Lọc các label và dữ liệu để hiển thị khi dữ liệu != 0
    for (var i = 0; i < currentData.length; i++) {
        if (currentData[i] !== 0) {
            filteredLabels.push(listLabels[i]);
            filteredData.push(currentData[i]);
        }
    }

    var backgroundColorsTinhTrang = filteredLabels.map(function (label) { // Set màu mặc định cho các tình trạng, mục đích để dễ đọc
        switch (label) {                                                 // Anh em thêm fields ở thì nhớ thêm màu vào nha
            case listLabels[0]:
                return 'rgba(255, 0, 0, 0.5)';
            case listLabels[1]:
                return 'rgba(40, 167, 69, 0.5)';
            case listLabels[2]:
                return 'rgba(255, 206, 86, 0.5)';
            case listLabels[3]:
                return 'rgba(75, 192, 192, 0.5)';
            case listLabels[4]:
                return 'rgba(0, 0, 255, 0.5)';
            case listLabels[5]:
                return 'rgba(138, 43, 226, 0.5)';
            case listLabels[6]:
                return 'rgba(255, 69, 0, 0.5)';
            case listLabels[7]:
                return 'rgba(75, 192, 192, 0.5)';
            case listLabels[8]:
                return 'rgba(106, 90, 205, 0.5)';
            case listLabels[9]:
                return 'rgba(173, 216, 230, 0.5)';
            case listLabels[10]:
                return 'rgba(255, 99, 132, 0.5)';
            case listLabels[11]:
                return 'rgba(255, 140, 0, 0.5)';
            case listLabels[12]:
                return 'rgba(255, 130, 0, 0.5)';
            case listLabels[13]:
                return 'rgba(255, 165, 0, 0.5)'; // màu cam
            case listLabels[14]:
                return 'rgba(128, 0, 128, 0.5)'; // màu tím
            case listLabels[15]:
                return 'rgba(0, 128, 128, 0.5)'; // màu xanh lam
            case listLabels[16]:
                return 'rgba(128, 128, 0, 0.5)'; // màu xanh lá cây
            case listLabels[17]:
                return 'rgba(128, 0, 0, 0.5)'; // màu đỏ đậm
            default:
                return ''; // Ở đây sẽ bắt lỗi trường hợp anh/em không thêm màu thì mặc định nó sẽ là màu đen
        }
    });

    // Kiểm tra sự tồn tại của biểu đồ trạng thái
    var existingTrangThaiChart = document.getElementById('trangThaiChart_' + index);
    if (existingTrangThaiChart) {
        // Nếu biểu đồ đã tồn tại, xoá biểu đồ cũ
        existingTrangThaiChart.parentNode.removeChild(existingTrangThaiChart);
    }

    // Tạo một div mới với class col-xs-6 cho mỗi biểu đồ
    var newDiv = document.createElement('div');
    newDiv.className = 'col-xs-6 chart-item';

    // Tạo một thẻ canvas mới cho mỗi biểu đồ
    var newCanvas = document.createElement('canvas');
    newCanvas.id = 'trangThaiChart_' + index; // Đặt id riêng cho mỗi biểu đồ
    newCanvas.width = "200";
    newCanvas.height = "100";

    // Cấu hình biểu đồ mới
    var trangThaiContext = newCanvas.getContext('2d');
    var trangThaiChart = new Chart(trangThaiContext, {
        type: 'pie',
        data: {
            labels: filteredLabels,
            datasets: [{
                label: label,
                data: filteredData,
                backgroundColor: backgroundColorsTinhTrang,
            }]
        },
        options: {
            responsive: true,
            title: {
                display: true,
                text: label
            }
        }
    });

    // Kiểm tra sự tồn tại của bảng
    var existingDataTable_SoLuong = document.querySelector('.dataTableChart_' + index);

    if (existingDataTable_SoLuong) {
        // Nếu bảng đã tồn tại, chỉ cập nhật dữ liệu trên bảng hiện có
        var tbody = existingDataTable_SoLuong.querySelector('tbody');
        tbody.innerHTML = ''; // Xóa nội dung hiện tại của tbody để cập nhật lại dữ liệu mới

        // Tạo dòng cho mỗi dữ liệu trong filteredLabels và filteredData và thêm chúng vào tbody
        for (var i = 0; i < filteredLabels.length; i++) {
            var row = document.createElement('tr');
            var labelCell = document.createElement('td');
            labelCell.textContent = filteredLabels[i];
            row.appendChild(labelCell);
            var dataCell = document.createElement('td');
            dataCell.textContent = filteredData[i];
            row.appendChild(dataCell);
            tbody.appendChild(row);
        }
        var newDataTableTitle = document.createElement('h5');
        newDataTableTitle.className = 'dataTableTitle_' + index;
        newDataTableTitle.textContent = 'Dữ liệu ' + (label || ''); // Kiểm tra label có tồn tại không trước khi gán

        var divWrapper = document.createElement('div');
        divWrapper.style.height = '150px'; // Đặt chiều cao tối đa của cái bảng
        divWrapper.style.overflowY = 'auto'; // Tạo thanh cuộn dọc
        divWrapper.appendChild(existingDataTable_SoLuong);

        newDiv.appendChild(newCanvas);
        newDiv.appendChild(newDataTableTitle);
        newDiv.appendChild(divWrapper);

        // Xoá các col-xs-6 chart-item hiện có nếu tồn tại
        var existingChartItems = document.querySelectorAll('.chart-item');
        existingChartItems.forEach(function (item) {
            item.parentNode.removeChild(item);
        });

    } else {
        // Nếu bảng không tồn tại, tạo một bảng mới và thêm dữ liệu vào đó
        var newDataTable = document.createElement('table');
        newDataTable.className = 'dataTableChart_' + index + ' table';
        var tbody = document.createElement('tbody');
        // Tạo dòng cho mỗi dữ liệu trong filteredLabels và filteredData
        for (var i = 0; i < filteredLabels.length; i++) {
            var row = document.createElement('tr');
            var labelCell = document.createElement('td');
            labelCell.textContent = filteredLabels[i];
            row.appendChild(labelCell);
            var dataCell = document.createElement('td');
            dataCell.textContent = filteredData[i];
            row.appendChild(dataCell);
            tbody.appendChild(row);
        }
        newDataTable.appendChild(tbody);

        var newDataTableTitle = document.createElement('h5');
        newDataTableTitle.className = 'dataTableTitle_' + index;
        newDataTableTitle.textContent = 'Dữ liệu ' + (label || ''); // Kiểm tra label có tồn tại không trước khi gán

        var divWrapper = document.createElement('div');
        divWrapper.style.height = '150px'; // Đặt chiều cao tối đa
        divWrapper.style.overflowY = 'auto'; // Tạo thanh cuộn dọc 
        divWrapper.appendChild(newDataTable);

        newDiv.appendChild(newCanvas);
        newDiv.appendChild(newDataTableTitle);
        newDiv.appendChild(divWrapper);
    }

    // Chèn div mới vào trong row
    var chartContainer = document.querySelector('.chart-container .row');
    chartContainer.appendChild(newDiv);

    // // Chèn thẻ canvas mới vào trong div có class chart-item
    // var chartContainer = document.querySelector('.chart-item');
    // chartContainer.appendChild(newCanvas);
});

////// Biểu đồ doanh thu của các kênh bán, VISA, TOUR, HOTEL,...
// var chartData = @Html.Raw(Json.Serialize(Model));

var chartData = JSON.parse(document.getElementById('EnVietChartData').value);

var chartContentDoanhThuElement = document.querySelector(".chart_content_doanhthu");

if (chartData == null) {
    chartContentDoanhThuElement.style.display = "none";
}
else {
    chartContentDoanhThuElement.style.display = "block";
}

//Mảng chứa các màu sắc cho từng sản phẩm
var colors = [
    'rgba(255, 99, 132, 0.2)',
    'rgba(54, 162, 235, 0.2)',
    'rgba(255, 206, 86, 0.2)',
    'rgba(75, 192, 192, 0.2)',
    'rgba(153, 102, 255, 0.2)',
    'rgba(255, 159, 64, 0.2)'
];
var borderColors = [
    'rgba(255, 99, 132, 1)',
    'rgba(54, 162, 235, 1)',
    'rgba(255, 206, 86, 1)',
    'rgba(75, 192, 192, 1)',
    'rgba(153, 102, 255, 1)',
    'rgba(255, 159, 64, 1)'
];

// Tạo một mảng duy nhất cho labels(ngày)
var labels = [...new Set(chartData.map(item => item.createdDate))];

// Tạo một mảng duy nhất cho names (sản phẩm)
var names = [...new Set(chartData.map(item => item.name))];

// Tạo một mảng mới cho datasets
var datasets = names.map((name, index) => {
    return {
        label: name,
        data: labels.map(date => {
            var item = chartData.find(item => item.name === name && item.createdDate === date);
            return item ? item.totalPrice : 0;
        }),
        backgroundColor: colors[index % colors.length],
        borderColor: borderColors[index % borderColors.length],
        borderWidth: 1,
    };
});


// Kiểm tra sự tồn tại của biểu đồ
var existingChart = document.getElementById('doanhThuChart');
if (existingChart) {
    // Nếu biểu đồ đã tồn tại, xoá biểu đồ cũ
    existingChart.parentNode.removeChild(existingChart);
}

// Tạo một canvas mới cho biểu đồ
var newCanvas_DoanhThu = document.createElement('canvas');
newCanvas_DoanhThu.id = 'doanhThuChart'; // Đặt id cho canvas
var chartContainer_DoanhThu = document.getElementById('chart-container_doanhthu');
chartContainer_DoanhThu.appendChild(newCanvas_DoanhThu);

var context = newCanvas_DoanhThu.getContext('2d');
var clientsChart = new Chart(context, {
    type: 'line',
    data: {
        labels: labels,
        datasets: datasets // Sử dụng mảng datasets đã tạo
    },
    options: {
        scales: {
            yAxes: [{
                ticks: {
                    beginAtZero: true,
                    callback: function (value, index, values) {
                        return value.toLocaleString('en-US'); // Định dạng kiểu như: 10,000,000
                    }
                },
                scaleLabel: {
                    display: true,
                    labelString: 'Tổng tiền bán của các kênh'
                },
            }],
            xAxes: [{
                ticks: {
                    beginAtZero: true,
                    callback: function (value, index, values) {
                        var date = new Date(value);
                        var day = date.getDate().toString().padStart(2, '0');
                        var month = (date.getMonth() + 1).toString().padStart(2, '0'); // Tháng bắt đầu từ 0
                        var year = date.getFullYear();
                        // return `${day}/${month}/${year}`;
                        return `${month}/${year}`;
                    }
                },
                scaleLabel: {
                    display: true,
                    labelString: 'Tháng'
                }
            }]
        }
    }
});


// Kiểm tra xem bảng đã tồn tại hay chưa
var existingDataTable = document.querySelector('.dataTableChart_doanhThu');

if (existingDataTable) {
    // Nếu bảng đã tồn tại, cập nhật dữ liệu trong tbody của bảng
    var tbody_DoanhThu = existingDataTable.querySelector('tbody');
    tbody_DoanhThu.innerHTML = ''; // Xóa dữ liệu cũ trong tbody để chuẩn bị cập nhật dữ liệu mới

    // Cập nhật hoặc tạo lại thead
    var thead_DoanhThu = existingDataTable.querySelector('thead');
    thead_DoanhThu.innerHTML = ''; // Xóa dữ liệu cũ trong thead

    // Tạo dòng đầu tiên của thead và thêm một ô trống
    var headerRow = document.createElement('tr');
    var emptyCell = document.createElement('th');
    headerRow.appendChild(emptyCell);

    // Tạo các ô cho các labels của datasets và thêm chúng vào header row
    datasets.forEach((dataset, datasetIndex) => {
        var labelCell = document.createElement('th');
        labelCell.textContent = dataset.label;
        headerRow.appendChild(labelCell);
    });
    // Thêm header row vào thead
    thead_DoanhThu.appendChild(headerRow);

    // Tạo dòng cho mỗi datasets ở tbody
    labels.forEach((label, labelIndex) => {
        var row = document.createElement('tr');

        // Tạo ô cho createdDate
        var createdDateCell = document.createElement('td');
        // Xử lý định dạng của createdDate đang là "yyyy-MM" thành "MM-yyyy"
        var dateParts = label.split('-');
        var formattedDate = dateParts[1] + '-' + dateParts[0]; // cắt cái chuỗi đó chia làm 2 phần, sau đó đảo ngược chuỗi đó lại
        createdDateCell.textContent = formattedDate;
        row.appendChild(createdDateCell);

        // Tạo ô cho dữ liệu của từng datasets
        datasets.forEach((dataset) => {
            var dataCell = document.createElement('td');
            // Truy cập dữ liệu tương ứng với từng tháng và sản phẩm
            var data = dataset.data[labelIndex];
            // Định dạng số với dấu phẩy ngăn cách hàng nghìn
            var formattedData = data.toLocaleString();
            dataCell.textContent = formattedData + " VNĐ";
            row.appendChild(dataCell);
        });

        // Thêm dòng vào tbody
        tbody_DoanhThu.appendChild(row);

        newDataTable_DoanhThu.appendChild(thead_DoanhThu);
        newDataTable_DoanhThu.appendChild(tbody_DoanhThu);

        var parentElement_DoanhThu = document.getElementById('chart-container_doanhthu');

        parentElement_DoanhThu.appendChild(newDataTableTitle_DoanhThu);
        parentElement_DoanhThu.appendChild(newDataTable_DoanhThu);
    });
} else {
    // Nếu bảng chưa tồn tại, tạo mới bảng và thêm dữ liệu vào tbody
    var newDataTableTitle_DoanhThu = document.createElement('h5');
    newDataTableTitle_DoanhThu.className = 'dataTableTitle_doanhThu';
    newDataTableTitle_DoanhThu.innerHTML = 'Dữ liệu doanh thu theo tháng';

    var newDataTable_DoanhThu = document.createElement('table');
    newDataTable_DoanhThu.className = 'dataTableChart_doanhThu table';
    var thead_DoanhThu = document.createElement('thead');
    var tbody_DoanhThu = document.createElement('tbody');

    // Tạo dòng đầu tiên của thead và thêm một ô trống
    var headerRow = document.createElement('tr');
    var emptyCell = document.createElement('th'); // Sử dụng thay vì td vì đây là header row
    headerRow.appendChild(emptyCell);

    // Tạo các ô cho các labels của datasets và thêm chúng vào header row
    datasets.forEach((dataset, datasetIndex) => {
        var labelCell = document.createElement('th');
        labelCell.textContent = dataset.label;
        headerRow.appendChild(labelCell);
    });
    // Thêm header row vào thead
    thead_DoanhThu.appendChild(headerRow);

    // Tạo dòng cho mỗi datasets ở tbody
    labels.forEach((label, labelIndex) => {
        var row = document.createElement('tr');

        // Tạo ô cho createdDate
        var createdDateCell = document.createElement('td');
        // Xử lý định dạng của createdDate đang là "yyyy-MM" thành "MM-yyyy"
        var dateParts = label.split('-');
        var formattedDate = dateParts[1] + '-' + dateParts[0]; // cắt cái chuỗi đó chia làm 2 phần, sau đó đảo ngược chuỗi đó lại
        createdDateCell.textContent = formattedDate;
        row.appendChild(createdDateCell);

        // Tạo ô cho dữ liệu của từng datasets
        datasets.forEach((dataset) => {
            var dataCell = document.createElement('td');
            // Truy cập dữ liệu tương ứng với từng tháng và sản phẩm
            var data = dataset.data[labelIndex];
            // Định dạng số với dấu phẩy ngăn cách hàng nghìn
            var formattedData = data.toLocaleString();
            dataCell.textContent = formattedData + " VNĐ";
            row.appendChild(dataCell);
        });

        // Thêm dòng vào tbody
        tbody_DoanhThu.appendChild(row);
    });

    newDataTable_DoanhThu.appendChild(thead_DoanhThu);
    newDataTable_DoanhThu.appendChild(tbody_DoanhThu);

    var parentElement_DoanhThu = document.getElementById('chart-container_doanhthu');

    parentElement_DoanhThu.appendChild(newDataTableTitle_DoanhThu);
    parentElement_DoanhThu.appendChild(newDataTable_DoanhThu);
}



