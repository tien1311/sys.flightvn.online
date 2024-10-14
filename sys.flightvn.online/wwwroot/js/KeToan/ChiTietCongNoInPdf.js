



$('#create_pdf').on('click', function () {

    if (confirm("Bạn muốn xác nhận công nợ")) {

        document.querySelector('#create_pdf').insertAdjacentHTML(
            'beforebegin',
            `<img src="/images/xacnhan.png" class="img-responsive" alt="">`);
        var myobj = document.getElementById("create_pdf");
        myobj.remove();

        document.querySelector('#nguoilapbieu').insertAdjacentHTML(
            'afterend',
            `<p><img src="/images/xacnhan.png" class="img-responsive" alt=""></p>   <p><b>@Model.NguoiLapBieu</b></p>`);

        document.querySelector('#ketoantruong').insertAdjacentHTML(
            'afterend',
            `<p><img src="/images/xacnhan.png" class="img-responsive" alt=""></p><p><b>Bùi Thị Ngọc Lan</b></p>`);

        var HTML_Width = $(".canvas_div_pdf").width();
        var HTML_Height = $(".canvas_div_pdf").height();
        var top_left_margin = 15;
        var PDF_Width = HTML_Width + (top_left_margin * 2);
        var PDF_Height = (PDF_Width * 1.5) + (top_left_margin * 2);
        var canvas_image_width = HTML_Width;
        var canvas_image_height = HTML_Height;

        var totalPDFPages = Math.ceil(HTML_Height / PDF_Height) - 1;


        html2canvas($(".canvas_div_pdf")[0], { allowTaint: true }).then(function (canvas) {
            canvas.getContext('2d');

            console.log(canvas.height + "  " + canvas.width);




            var imgData = canvas.toDataURL("image/jpeg", 1.0);

            var pdf = new jsPDF('p', 'pt', [PDF_Width, PDF_Height]);
            pdf.addImage(imgData, 'JPG', top_left_margin, top_left_margin, canvas_image_width, canvas_image_height);
            for (var i = 1; i <= totalPDFPages; i++) {
                pdf.addPage(PDF_Width, PDF_Height);
                pdf.addImage(imgData, 'JPEG', top_left_margin, -(PDF_Height * i) + (top_left_margin * 4), canvas_image_width, canvas_image_height);
            }
            //totalPDFPages



            //Save file
            //pdf.save("CongNoThang10.pdf");
            var binary = pdf.output();
            var reqData = btoa(binary);





            // var abc = pdf.output('datauristring');
            // var x = window.open();
            // x.document.open();
            // x.document.location=abc;
            //window.open(URL.createObjectURL(blob));
            //var blob = String(pdf.output());

            var RowID = $("#gridCongNo .congno").closest('tr').attr('id');
            //var blob = pdf.output();



            //Update tình trạng



            $.ajax({
                type: "POST",
                url: "../KeToan/UpdateFile",

                data: JSON.stringify({ rowID: RowID, dulieu: reqData }),
                dataType: "json",


                contentType: "application/json; charset=utf-8",
                beforeSend: function () {
                    //show loading

                },
                complete: function () {
                    //hide loading
                },
                success: function (resultData) {

                    location.reload(true);

                },
                error: function (xhr, status, p3, p4) {
                    var err = "Error " + " " + status + " " + p3 + " " + p4;
                    if (xhr.responseText)
                        err = xhr.responseText;
                    alert(err);
                }
            })

        });

    }




});