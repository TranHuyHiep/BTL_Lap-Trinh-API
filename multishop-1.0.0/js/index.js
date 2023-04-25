function signIn() {
    localStorage.clear()
    window.location.href = "../material-dashboard-2/pages/sign-in.html";    
}

function signUp() {
    localStorage.clear()
    window.location.href = "../material-dashboard-2/pages/sign-up.html";    
}
loadData()
function loadData() {
    let cart = localStorage.getItem("cart")
    cart = JSON.parse(cart)
    if(cart == null) {
        cart = []
        localStorage.setItem("cart", JSON.stringify(cart))
    }
    $("#numberCart").html(cart.length)
}

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

function phanTrang(soTrang) {
    if(soTrang == null) soTrang = 1;
    let str = '';
    if(soTrang > 1) str += `<li class="page-item"><button class="page-link" onclick="hienThiSanPham('${soTrang - 1}')">Previous</button></li>`
                        + `<li class="page-item"><button class="page-link" onclick="hienThiSanPham('${soTrang - 1}')">${soTrang - 1}</button></li>`;
    str += `<li class="page-item active"><span class="page-link"> ${soTrang} </span></li>`;
    str += `<li class="page-item"><button class="page-link" onclick="hienThiSanPham('${parseInt(soTrang) + 1}')"> ${parseInt(soTrang) + 1} </button></li>`
            + `<li class="page-item"><button class="page-link" onclick="hienThiSanPham('${parseInt(soTrang) + 1}')">Next</button></li>`;
    $("#phanTrang").html(str);
}

hienThiSanPham()
function hienThiSanPham(soTrang) {
    if(soTrang == null) {
        soTrang = 1
    }
    phanTrang(soTrang)
    $.ajax({
        url: "https://localhost:44368/api/SanPhamAPI/" + soTrang,
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
                            <a class="h6 text-decoration-none text-truncate" href="detail.html?maSp=${item.maSp}">${item.tenSp}</a>
                            <div class="d-flex align-items-center justify-content-center mt-2">
                                    <h5>${item.giaLonNhat.toLocaleString()}</h5><h6 class="text-muted ml-2"></h6>
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
}

function muaSp(productId) {
    var cart = localStorage.getItem("cart");
    console.log(cart);
    if(cart.length) {
        cart = JSON.parse(cart)
    } else {
        cart = []
    }
    console.log(cart);
    $.ajax({
        type: 'POST',
        url: 'https://localhost:44368/api/GioHangAPI/add/' + productId,
        contentType: "application/json",
        data: JSON.stringify(cart),
        success: function (res) {
            $('#noiDung').html("Đã thêm vào giỏ hàng!");
            $('#success_tic').modal('show');
            console.log(res);
            localStorage.setItem("cart", JSON.stringify(res));
            loadData()
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
                        <a class="h6 text-decoration-none text-truncate" href="detail.html?maSp=${item.maSp}">${item.tenSp}</a>
                        <div class="d-flex align-items-center justify-content-center mt-2">
                                <h5>${item.giaLonNhat.toLocaleString()}</h5><h6 class="text-muted ml-2"></h6>
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