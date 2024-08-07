$(document).ready(function () {
    //The loai
    //Xử lý thêm thể loại
    var isTentlValid = false;

    function checkTentlExistence(tentl) {
        $.ajax({
            url: '/TheLoai/IsTentlExists',
            type: 'GET',
            data: { tentl: tentl },
            success: function (data) {
                if (data) {
                    $('#tentlError').text('Tên thể loại đã tồn tại!').show();
                    isTentlValid = false;
                } else {
                    $('#tentlError').text('').hide();
                    isTentlValid = true;
                }
            },
            error: function () {
                $('#tentlError').text('Lỗi khi kiểm tra dữ liệu.').show();
                isTentlValid = false;
            }
        });
    }

    $('#TenTL').on('keyup', function () {
        var tentl = $(this).val();
        checkTentlExistence(tentl);
    });

    function kiemtrarong(tentl) {
        if (tentl === "") {
            $('#tentlError').text('Tên thể loại không được để trống!').show();
            isTentlValid = false;
        }
    }

    $('#createForm').submit(function (event) {
        event.preventDefault();

        var tentl = $('#TenTL').val();
        kiemtrarong(tentl);

        if (!isTentlValid) {
            $('#message').text('Vui lòng kiểm tra các lỗi trước khi gửi.').css('color', 'red');
            return;
        }

        var formData = $(this).serialize();
        $.ajax({
            url: '/TheLoai/Create',
            type: 'POST',
            data: formData,
            success: function (response) {
                if (response.success) {
                    $('#message').text(response.message).css('color', 'green');
                    setTimeout(function () {
                        window.location.href = '/Admin/TheLoai/DanhSach';
                    }, 2000);
                } else {
                    $('#message').text(response.message).css('color', 'red');
                }
            },
            error: function () {
                $('#message').text('Lỗi khi gửi dữ liệu.').css('color', 'red');
            }
        });
    });
    //Xu ly cập nhật thể loại
    $('#editForm').submit(function (event) {
        event.preventDefault();

        var tentl = $('#TenTL').val();
        kiemtrarong(tentl);

        if (!isTentlValid) {
            $('#message').text('Vui lòng kiểm tra các lỗi trước khi gửi.').css('color', 'red');
            return;
        }

        var formData = $(this).serialize();
        $.ajax({
            url: '/TheLoai/EditTheLoai',
            type: 'POST',
            data: formData,
            success: function (response) {
                if (response.success) {
                    $('#message').text(response.message).css('color', 'green');
                    setTimeout(function () {
                        window.location.href = '/Admin/TheLoai/DanhSach';
                    }, 2000);
                } else {
                    $('#message').text(response.message).css('color', 'red');
                }
            },
            error: function () {
                $('#message').text('Lỗi khi gửi dữ liệu.').css('color', 'red');
            }
        });
    });

    //Truyen
    //Xu ly Them truyen
    var trValid = true;
    function kiemtrarong(value, errorSelector, errorMessage) {
        if (value.trim() === "") {
            $(errorSelector).text(errorMessage).show();
            trValid = false;
        } else {
            $(errorSelector).text('').hide();
        }
    }

    $('#createTr').submit(function (event) {
        event.preventDefault();

        var tenTruyen = $('#TenTruyen').val();
        var tacGia = $('#TacGia').val();
        var tomTat = $('#TomTat').val();
        var hinhAnhInput = $('input[name="fileUpload"]');
        var hinhAnh = hinhAnhInput.val();
        var ngayXuatBan = $('#NgayXuatBan').val();
        var soLuong = $('#SoLuong').val();
        var gia = $('#Gia').val();

        kiemtrarong(tenTruyen, '#TenTruyenError', 'Tên truyện không được để trống!');
        kiemtrarong(tacGia, '#TacGiaError', 'Tác giả không được để trống!');
        kiemtrarong(tomTat, '#TomTatError', 'Tóm tắt không được để trống!');

        if (ngayXuatBan === "") {
            $('#NgayXuatBanError').text('Ngày xuất bản không được để trống!').show();
            trValid = false;
        } else {
            var selectedDate = new Date(ngayXuatBan);
            var currentDate = new Date();
            if (selectedDate > currentDate) {
                $('#NgayXuatBanError').text('Ngày xuất bản không được sau thời gian hiện tại!').show();
                trValid = false;
            } else {
                $('#NgayXuatBanError').text('').hide();
            }
        }

        if (soLuong === "") {
            $('#SoLuongError').text('Số lượng không được để trống!').show();
            trValid = false;
        } else if (soLuong <= 0) {
            $('#SoLuongError').text('Số lượng tối thiếu là 1!').show();
            trValid = false;
        } else {
            $('#SoLuongError').text('').hide();
        }

        if (gia === "") {
            $('#GiaError').text('Giá tiền không được để trống!').show();
            trValid = false;
        } else if (gia < 1000) {
            $('#GiaError').text('Giá tiền tối thiểu là 1000 đồng!').show();
            trValid = false;
        } else {
            $('#GiaError').text('').hide();
        }

        var allowedExtensions = /(\.jpg|\.jpeg|\.png)$/i;
        if (hinhAnh === "") {
            $('#HinhAnhError').text('Chưa chọn hình ảnh!').show();
            trValid = false;
        } else if (!allowedExtensions.exec(hinhAnh)) {
            $('#HinhAnhError').text('Chỉ chấp nhận tệp jpg, jpeg, hoặc png!').show();
            trValid = false;
        } else {
                $('#HinhAnhError').text('').hide();

                var fileName = hinhAnh.split('\\').pop();
                $.ajax({
                    url: '/QLTruyen/CheckFileExists',
                    type: 'POST',
                    data: JSON.stringify({ fileName: fileName }),
                    contentType: 'application/json',
                    success: function (response) {
                        if (response.exists) {
                            $('#HinhAnhError').text('Ảnh này đã tồn tại!').show();
                            trValid = false;
                        } else {
                            $('#HinhAnhError').text('').hide();

                            if (trValid) {
                                var formData = new FormData($('#createTr')[0]);
                                $.ajax({
                                    url: '/QLTruyen/Create',
                                    type: 'POST',
                                    data: formData,
                                    processData: false,
                                    contentType: false,
                                    success: function (response) {
                                        if (response.success) {
                                            $('#message').text(response.message).css('color', 'green');
                                            window.location.href = '/Admin/QLTruyen/DanhSach';
                                        } else {
                                            $('#message').text(response.message).css('color', 'red');
                                        }
                                    },
                                    error: function () {
                                        $('#message').text('Lỗi khi gửi dữ liệu.').css('color', 'red');
                                    }
                                });
                            } else {
                                $('#message').text('Vui lòng kiểm tra các lỗi trước khi gửi.').css('color', 'red');
                            }
                        }
                    },
                    error: function () {
                        $('#message').text('Lỗi khi kiểm tra tệp.').css('color', 'red');
                    }
                });

        }
    });
    // Xu lý cập nhật truyện
    $('#EditTr').submit(function (event) {
        event.preventDefault();

        var tenTruyen = $('#TenTruyen').val();
        var tacGia = $('#TacGia').val();
        var tomTat = $('#TomTat').val();
        var hinhAnhInput = $('input[name="fileUpload"]');
        var hinhAnh = hinhAnhInput.val();
        var ngayXuatBan = $('#NgayXuatBan').val();
        var soLuong = $('#SoLuong').val();
        var gia = $('#Gia').val();

        kiemtrarong(tenTruyen, '#TenTruyenError', 'Tên truyện không được để trống!');
        kiemtrarong(tacGia, '#TacGiaError', 'Tác giả không được để trống!');
        kiemtrarong(tomTat, '#TomTatError', 'Tóm tắt không được để trống!');

        if (ngayXuatBan === "") {
            $('#NgayXuatBanError').text('Ngày xuất bản không được để trống!').show();
            trValid = false;
        } else {
            var selectedDate = new Date(ngayXuatBan);
            var currentDate = new Date();
            if (selectedDate > currentDate) {
                $('#NgayXuatBanError').text('Ngày xuất bản không được sau thời gian hiện tại!').show();
                trValid = false;
            } else {
                $('#NgayXuatBanError').text('').hide();
            }
        }

        if (soLuong === "") {
            $('#SoLuongError').text('Số lượng không được để trống!').show();
            trValid = false;
        } else if (soLuong <= 0) {
            $('#SoLuongError').text('Số lượng tối thiếu là 1!').show();
            trValid = false;
        } else {
            $('#SoLuongError').text('').hide();
        }

        if (gia === "") {
            $('#GiaError').text('Giá tiền không được để trống!').show();
            trValid = false;
        } else if (gia < 1000) {
            $('#GiaError').text('Giá tiền tối thiểu là 1000 đồng!').show();
            trValid = false;
        } else {
            $('#GiaError').text('').hide();
        }

        if (hinhAnh !== "") {
            var allowedExtensions = /(\.jpg|\.jpeg|\.png)$/i;
            if (!allowedExtensions.exec(hinhAnh)) {
                $('#HinhAnhError').text('Chỉ chấp nhận tệp jpg, jpeg, hoặc png!').show();
                trValid = false;
            } else {
                $('#HinhAnhError').text('').hide();

                var fileName = hinhAnh.split('\\').pop();
                $.ajax({
                    url: '/QLTruyen/CheckFileExists',
                    type: 'POST',
                    data: JSON.stringify({ fileName: fileName }),
                    contentType: 'application/json',
                    success: function (response) {
                        if (response.exists) {
                            $('#HinhAnhError').text('Ảnh này đã tồn tại!').show();
                            trValid = false;
                        } else {
                            $('#HinhAnhError').text('').hide();

                            if (trValid) {
                                var formData = new FormData($('#EditTr')[0]);
                                $.ajax({
                                    url: '/QLTruyen/EditTruyen',
                                    type: 'POST',
                                    data: formData,
                                    processData: false,
                                    contentType: false,
                                    success: function (response) {
                                        if (response.success) {
                                            $('#message').text(response.message).css('color', 'green');
                                            window.location.href = '/Admin/QLTruyen/DanhSach';
                                        } else {
                                            $('#message').text(response.message).css('color', 'red');
                                        }
                                    },
                                    error: function () {
                                        $('#message').text('Lỗi khi gửi dữ liệu.').css('color', 'red');
                                    }
                                });
                            } else {
                                $('#message').text('Vui lòng kiểm tra các lỗi trước khi gửi.').css('color', 'red');
                            }
                        }
                    },
                    error: function () {
                        $('#message').text('Lỗi khi kiểm tra tệp.').css('color', 'red');
                    }
                });
            }
        } else {
            if (trValid) {
                var formData = new FormData($('#EditTr')[0]);
                $.ajax({
                    url: '/QLTruyen/EditTruyen',
                    type: 'POST',
                    data: formData,
                    processData: false,
                    contentType: false,
                    success: function (response) {
                        if (response.success) {
                            $('#message').text(response.message).css('color', 'green');
                            window.location.href = '/Admin/QLTruyen/DanhSach';
                        } else {
                            $('#message').text(response.message).css('color', 'red');
                        }
                    },
                    error: function () {
                        $('#message').text('Lỗi khi gửi dữ liệu.').css('color', 'red');
                    }
                });
            } else {
                $('#message').text('Vui lòng kiểm tra các lỗi trước khi gửi.').css('color', 'red');
            }
        }
    });
})