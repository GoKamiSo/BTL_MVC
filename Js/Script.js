//gửi yêu cầu ajax thêm vào giỏ hàng 
function ThemVaoGioHang(maTruyen, hinhAnh, tenTruyen, gia, soLuong, accountId) {
    $.ajax({
        type: "POST",
        url: '/GioHang/ThemVaoGioHang',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            MaTruyen: maTruyen,
            HinhAnh: hinhAnh,
            TenTruyen: tenTruyen,
            Gia: gia,
            SoLuongMua: soLuong,
            AccountId: accountId
        }),
        success: function (response) {
            if (response.success) {
                Swal.fire({
                    icon: 'success',
                    title: 'Thêm vào giỏ hàng thành công.',
                    showConfirmButton: false,
                    confirmButtonText: 'OK'
                });
            } else {
                Swal.fire({
                    icon: 'warning',
                    title: response.message,
                    showConfirmButton: false,
                    confirmButtonText: 'OK'
                });
            }
        },
        error: function () {
            alert("Có lỗi xảy ra, vui lòng thử lại.");
        }
    });
}
function getProductQuantity() {
    var quantity = document.getElementById('productQuantity').value;
    return parseInt(quantity, 10); 
}
//gửi yc ajax tăng giảm số lượng
$(document).ready(function () {
    // Giảm số lượng
    $('.btn-decrease').on('click', function () {
        var maTruyen = $(this).data('id');
        var quantitySpan = $(this).siblings('.quantity');
        var newQuantity = parseInt(quantitySpan.text()) - 1;

        if (newQuantity > 0) {
            updateQuantity(maTruyen, newQuantity, quantitySpan);
        }
    });

    // Tăng số lượng
    $('.btn-increase').on('click', function () {
        var maTruyen = $(this).data('id');
        var quantitySpan = $(this).siblings('.quantity');
        var newQuantity = parseInt(quantitySpan.text()) + 1;

        updateQuantity(maTruyen, newQuantity, quantitySpan);
    });

    function updateTotal(tongTien) {
        $('tfoot').find('.tongtien').text(tongTien + '₫');
    }

    // Hàm cập nhật số lượng
    function updateQuantity(maTruyen, newQuantity, quantitySpan) {
        $.ajax({
            type: "POST",
            url: '/GioHang/CapNhatSoLuong',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({ maTruyen: maTruyen, soLuong: newQuantity }),
            success: function (response) {
                if (response.success) {
                    quantitySpan.text(newQuantity);
                    var row = quantitySpan.closest('tr');
                    row.find('.thanhtien').text(response.thanhTien + '₫');
                    updateTotal(response.tongTien);
                } else {
                    Swal.fire({
                        icon: 'warning',
                        title: 'Không thể tăng thêm!',
                        text: response.message,
                        showConfirmButton: false,
                        confirmButtonText: 'OK'
                    });
                }
            },
            error: function () {
                alert("Có lỗi xảy ra, vui lòng thử lại.");
            }
        });
    }
    function XoaSP(maTruyen) {
        $.ajax({
            type: "POST",
            url: '/GioHang/XoaHang',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({ maTruyen: maTruyen }),
            success: function (response) {
                if (response.success) {
                    var row = $('button[data-id="' + maTruyen + '"]').closest('tr');
                    row.remove();
                    updateTotal(response.tongTien);
                } else {
                    Swal.fire({
                        icon: 'warning',
                        title: 'Không thể xóa sản phẩm!',
                        text: response.message,
                        showConfirmButton: false,
                        confirmButtonText: 'OK'
                    });
                }
            },
            error: function () {
                alert("Có lỗi xảy ra, vui lòng thử lại.");
            }
        });
    }
    $('.btn-xoa').on('click', function () {
        var maTruyen = $(this).data('id');
        XoaSP(maTruyen);
    });
});

