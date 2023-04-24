$.ajax({
    url: "https://localhost:44368/api/AccessAPI/login",
    type: "GET",
    success: function (result) {
        // Xử lý kết quả trả về
        console.log(result);
    }
});

$.ajax({
    url: "https://localhost:44368/api/hangsanxuatapi",
    type: "GET",
    success: function (result) {
        // Xử lý kết quả trả về
        console.log(result);
        var menu = $("#menu");
        var str = '';
        $.each(result, function (index, item) {
            str += `<a onclick="SanPhamTheoHangSanXuat('` + item.maHangSx + `')" class="nav-item nav-link">` + item.hangSx + `</a>`;
        });
        menu.html(str);
    },
    error: function (xhr, textStatus, errorThrown) {
        // Xử lý lỗi
        console.log(textStatus + ": " + errorThrown);
    }
});

$.ajax({
    url: "https://localhost:44368/api/SanPhamAPI/1",
    type: "GET",
    success: function (result) {
        // Xử lý kết quả trả về
        console.log(result);
        var sanphams = $("#sanphams");
        var str = '';
        $.each(result, function (index, item) {
            str +=
                `<div class="col-lg-3 col-md-4 col-sm-6 pb-1">
                    <div class="product-item bg-light mb-4">
                        <div class="product-img position-relative overflow-hidden">
                            <img class="img-fluid w-100" src="img/ImageCamera/${item.anhDaiDien}" alt="${item.anhDaiDien}">
                            <div class="product-action">
                                <a class="btn btn-outline-dark btn-square" onclick="muaSp('${item.maSp}')"><i class="fa fa-shopping-cart"></i></a>
                                <a class="btn btn-outline-dark btn-square" href=""><i class="far fa-heart"></i></a>
                                <a class="btn btn-outline-dark btn-square" href=""><i class="fa fa-sync-alt"></i></a>
                                <a class="btn btn-outline-dark btn-square" href=""><i class="fa fa-search"></i></a>
                            </div>
                        </div>
                        <div class="text-center py-4">
                            <a class="h6 text-decoration-none text-truncate" href="">${item.tenSp}</a>
                            <div class="d-flex align-items-center justify-content-center mt-2">
                                <h5>${item.giaLonNhat}</h5><h6 class="text-muted ml-2"></h6>
                            </div>
                            <div class="d-flex align-items-center justify-content-center mb-1">
                                <small class="fa fa-star text-primary mr-1"></small>
                                <small class="fa fa-star text-primary mr-1"></small>
                                <small class="fa fa-star text-primary mr-1"></small>
                                <small class="fa fa-star text-primary mr-1"></small>
                                <small class="fa fa-star text-primary mr-1"></small>
                                <small>(99)</small>
                            </div>
                        </div>
                    </div>
                </div>`;
        });
        sanphams.html(str);
    },
    error: function (xhr, textStatus, errorThrown) {
        // Xử lý lỗi
        console.log(textStatus + ": " + errorThrown);
    }
});

function muaSp(productId) {
    $.ajax({
        url: 'https://localhost:44368/api/GioHangAPI/add/' + productId,
        type: 'POST',
        success: function (res) {
            if (res == "true") {
                showGioHang()
                $('#noiDung').html("Add to cart successed!");
                $('#success_tic').modal('show');
            }
            else {
                console.log(res);
            }
        },
        error: function (error) {
            console.log(error);
        }
    })
}

function showGioHang() {
    $.ajax({
        url: "https://localhost:44368/api/GioHangAPI",
        type: "GET",
        success: function (result) {
            // Xử lý kết quả trả về
            console.log(result);
        },
        error: function (xhr, textStatus, errorThrown) {
            // Xử lý lỗi
            console.log(textStatus + ": " + errorThrown);
        }
    });
}

function SanPhamTheoHangSanXuat(maHangSx) {
    $.ajax({
        url: 'https://localhost:44368/api/hangsanxuatapi/hangsanxuat?mahangSx=' + maHangSx,
        type: 'GET',
        dataType: 'json',
        success: function (result) {
            var sanphams = $("#sanphams");
            var str = '';
            $.each(result, function (index, item) {
                str +=
                    `<div class="col-lg-3 col-md-4 col-sm-6 pb-1">
                    <div class="product-item bg-light mb-4">
                        <div class="product-img position-relative overflow-hidden">
                            <img class="img-fluid w-100" src="img/ImageCamera/${item.anhDaiDien}" alt="${item.anhDaiDien}">
                            <div class="product-action">
                                <a class="btn btn-outline-dark btn-square" onclick="muaSp('${item.maSp}')"><i class="fa fa-shopping-cart"></i></a>
                                <a class="btn btn-outline-dark btn-square" href=""><i class="far fa-heart"></i></a>
                                <a class="btn btn-outline-dark btn-square" href=""><i class="fa fa-sync-alt"></i></a>
                                <a class="btn btn-outline-dark btn-square" href=""><i class="fa fa-search"></i></a>
                            </div>
                        </div>
                        <div class="text-center py-4">
                            <a class="h6 text-decoration-none text-truncate" href="">${item.tenSp}</a>
                            <div class="d-flex align-items-center justify-content-center mt-2">
                                <h5>${item.giaLonNhat}</h5><h6 class="text-muted ml-2"></h6>
                            </div>
                            <div class="d-flex align-items-center justify-content-center mb-1">
                                <small class="fa fa-star text-primary mr-1"></small>
                                <small class="fa fa-star text-primary mr-1"></small>
                                <small class="fa fa-star text-primary mr-1"></small>
                                <small class="fa fa-star text-primary mr-1"></small>
                                <small class="fa fa-star text-primary mr-1"></small>
                                <small>(99)</small>
                            </div>
                        </div>
                    </div>
                </div>`;
            });
            sanphams.html(str);
        }
    })
}