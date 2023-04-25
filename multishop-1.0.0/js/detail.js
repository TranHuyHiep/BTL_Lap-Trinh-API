const urlParams = new URLSearchParams(window.location.search);
const maSp = urlParams.get('maSp');
let cart = localStorage.getItem("cart")
cart = JSON.parse(cart)
$("#numberCart").html(cart.length)
$.ajax({
    url: 'https://localhost:44368/api/sanphamapi/chitietsanpham?masp=' + maSp,
    type: 'GET',
    dataType: 'json',
    success: function (result) {
        $.ajax({
            url: 'https://localhost:44368/api/sanphamapi/ChiTietAnhSanPham?masp=' + maSp,
            type: 'GET',
            dataType: 'json',
            success: function (a) {
                $.ajax({
                    url: 'https://localhost:44368/home/ChiTietSanPhamComment?masp=' + maSp,
                    type: 'GET',
                    dataType: 'json',
                    success: function (b) {
                        let numReview = b.length;
                        var chitiet = $("#chitiet");
                        var str = '';
                        str +=
                            `<div class="row px-xl-5">
                            <div class="col-lg-5 mb-30">
                                <div id="product-carousel" class="carousel slide" data-ride="carousel">
                                    <div class="carousel-inner bg-light">
                                        <div class="carousel-item active">
                                            <img class="w-100 h-100" src="img/ImageCamera/${result.anhDaiDien}" alt="${result.anhDaiDien}" >
                                        </div>`;
                        a.forEach(element => {
                            str +=
                                `<div class="carousel-item">
                        <img class="w-100 h-100" src="img/ImageCamera/${element.tenFileAnh}" alt="" >
                    </div>`
                        })
                        str +=
                            `</div>
                    
                    <a class="carousel-control-prev" href="#product-carousel" data-slide="prev">
                        <i class="fa fa-2x fa-angle-left text-dark"></i>
                    </a>
                    <a class="carousel-control-next" href="#product-carousel" data-slide="next">
                        <i class="fa fa-2x fa-angle-right text-dark"></i>
                    </a>
                </div>
            </div>

            <div class="col-lg-7 h-auto mb-30">
                <div class="h-100 bg-light p-30">
                    <h3>${result.tenSp}</h3>
                    <div class="d-flex mb-3">
                        <div class="text-primary mr-2">
                            <small class="fas fa-star"></small>
                            <small class="fas fa-star"></small>
                            <small class="fas fa-star"></small>
                            <small class="fas fa-star-half-alt"></small>
                            <small class="far fa-star"></small>
                        </div>
                        <small class="pt-1">(99 Reviews)</small>
                    </div>
                    <h3 class="font-weight-semi-bold mb-4">${result.giaLonNhat}</h3>
                    <p class="mb-4">${result.gioiThieuSp}</p>
                    <div class="d-flex align-items-center mb-4 pt-2">
                        <div class="input-group quantity mr-3" style="width: 130px;">
                            <div class="input-group-btn">
                                <button class="btn btn-primary btn-minus">
                                    <i class="fa fa-minus"></i>
                                </button>
                            </div>
                            <input type="text" class="form-control bg-secondary border-0 text-center" value="1">
                            <div class="input-group-btn">
                                <button class="btn btn-primary btn-plus">
                                    <i class="fa fa-plus"></i>
                                </button>
                            </div>
                        </div>
                        <button class="btn btn-primary px-3" onclick="muaSp('${result.maSp}')"><i class="fa fa-shopping-cart mr-1"></i> Add To
                            Cart</button>
                    </div>
                    <div class="d-flex pt-2">
                        <strong class="text-dark mr-2">Share on:</strong>
                        <div class="d-inline-flex">
                            <a class="text-dark px-2" href="">
                                <i class="fab fa-facebook-f"></i>
                            </a>
                            <a class="text-dark px-2" href="">
                                <i class="fab fa-twitter"></i>
                            </a>
                            <a class="text-dark px-2" href="">
                                <i class="fab fa-linkedin-in"></i>
                            </a>
                            <a class="text-dark px-2" href="">
                                <i class="fab fa-pinterest"></i>
                            </a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row px-xl-5">
            <div class="col">
                <div class="bg-light p-30">
                    <div class="nav nav-tabs mb-4">
                        <a class="nav-item nav-link text-dark active" data-toggle="tab"
                            href="#tab-pane-1">Description</a>
                        <a class="nav-item nav-link text-dark" data-toggle="tab" href="#tab-pane-2">Information</a>
                        <a class="nav-item nav-link text-dark" data-toggle="tab" href="#tab-pane-3">Reviews (${numReview})</a>
                    </div>
                    <div class="tab-content">
                        <div class="tab-pane fade show active" id="tab-pane-1">
                            <h4 class="mb-3">Product Description</h4>
                            <p>//todo
                            </p>
                            
                        </div>
                        <div class="tab-pane fade" id="tab-pane-2">
                            <h4 class="mb-3">Additional Information</h4>
                            <p>Eos no lorem eirmod diam diam, eos elitr et gubergren diam sea. Consetetur vero aliquyam
                                invidunt duo dolores et duo sit. Vero diam ea vero et dolore rebum, dolor rebum eirmod
                                consetetur invidunt sed sed et, lorem duo et eos elitr, sadipscing kasd ipsum rebum
                                diam. Dolore diam stet rebum sed tempor kasd eirmod. Takimata kasd ipsum accusam
                                sadipscing, eos dolores sit no ut diam consetetur duo justo est, sit sanctus diam tempor
                                aliquyam eirmod nonumy rebum dolor accusam, ipsum kasd eos consetetur at sit rebum, diam
                                kasd invidunt tempor lorem, ipsum lorem elitr sanctus eirmod takimata dolor ea invidunt.
                            </p>
                            <div class="row">
                                <div class="col-md-6">
                                    <ul class="list-group list-group-flush">
                                        <li class="list-group-item px-0">
                                            Sit erat duo lorem duo ea consetetur, et eirmod takimata.
                                        </li>
                                        <li class="list-group-item px-0">
                                            Amet kasd gubergren sit sanctus et lorem eos sadipscing at.
                                        </li>
                                        <li class="list-group-item px-0">
                                            Duo amet accusam eirmod nonumy stet et et stet eirmod.
                                        </li>
                                        <li class="list-group-item px-0">
                                            Takimata ea clita labore amet ipsum erat justo voluptua. Nonumy.
                                        </li>
                                    </ul>
                                </div>
                                <div class="col-md-6">
                                    <ul class="list-group list-group-flush">
                                        <li class="list-group-item px-0">
                                            Sit erat duo lorem duo ea consetetur, et eirmod takimata.
                                        </li>
                                        <li class="list-group-item px-0">
                                            Amet kasd gubergren sit sanctus et lorem eos sadipscing at.
                                        </li>
                                        <li class="list-group-item px-0">
                                            Duo amet accusam eirmod nonumy stet et et stet eirmod.
                                        </li>
                                        <li class="list-group-item px-0">
                                            Takimata ea clita labore amet ipsum erat justo voluptua. Nonumy.
                                        </li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                        <div class="tab-pane fade" id="tab-pane-3">
                            <div class="row">
                                <div class="col-md-6">
                                    <h4 class="mb-4">${numReview} review cho ${result.tenSp}</h4>`

                        b.forEach(jtem => {
                            str +=
                                `<div class="media mb-4">
                                    <div class="media-body">
                                        <h6>${jtem.fullName}<small> - <i>${jtem.commentDate}</i></small></h6>
                                        <div class="text-primary mb-2">`;
                
                            for (let index = 1; index <= jtem.parentID; index++) {
                                str += `<i class="fas fa-star"></i>`
                            }
                            for (let index = 1; index < 5 - jtem.parentID; index++) {
                                str += `<i class="far fa-star"></i>`
                            }
                            str +=`</div>
                                        <p>${jtem.commentMsg}</p>
                                    </div>
                                </div>`
                        })

                        str += ``;

                        str += `</div>
                                <div class="col-md-6">
                                    <h4 class="mb-4">Leave a review</h4>
                                    <small>Your email address will not be published. Required fields are marked
                                        *</small>
                                    <div class="d-flex my-3">
                                        <p class="mb-0 mr-2">Your Rating * :</p>
                                        <div class="text-primary">
                                            <i id="ratingStar" class="ratingStar far fa-star" data-value="1"></i>
                                            <i id="ratingStar" class="ratingStar far fa-star" data-value="2" ></i>
                                            <i id="ratingStar" class="ratingStar far fa-star" data-value="3"></i>
                                            <i id="ratingStar" class="ratingStar far fa-star" data-value="4"></i>
                                            <i id="ratingStar" class="ratingStar far fa-star" data-value="5"></i>
                                        </div>
                                        <input type="hidden" name="rating" id="ratingValues" />
                                    </div>
                                    <div>
                                        <div class="form-group">
                                            <label for="message">Your Review *</label>
                                            <textarea id="txtCommentNew" cols="30" rows="5" class="form-control" value=""></textarea>
                                        </div>
                                        <div class="form-group mb-0">
                                            <input type="button" name="signup" id="signup" class="btn btn-success" value="Đánh giá" />
                                        </div>
                                </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>`;
                        chitiet.html(str);

                        $(".ratingStar").hover(function () {
                            $(".ratingStar").addClass("far").removeClass("fas");
                    
                            $(this).addClass("fas").removeClass("far");
                            $(this).prevAll(".ratingStar").addClass("fas").removeClass("far");
                        });
                        $(".ratingStar").click(function () {
                            var startValue = $(this).attr("data-value");
                            $("#ratingValues").val(startValue);
                        });
                        document.getElementById("signup").onclick = function () {
                            let data = {
                                id: 1,
                                commentMsg: $("#txtCommentNew").val(),
                                commentDate: new Date().toISOString().slice(0, 10),
                                productID: maSp,
                                userId: 1,
                                parentID: $("#ratingValues").val(),
                                fullname: JSON.parse(localStorage.getItem("username")),
                            }
                            $.ajax({
                                url: "https://localhost:44368/api/v1/addcoment",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                data: JSON.stringify(data),
                                async: true,
                                success: function (data) {
                                    location.reload()
                                },
                                error: function () {
                                    $("#loader_container").css("display", "none");
                                    alert("Cannot get data");
                                }
                            })
                        }
                    }
                });
            }
        })
    }
})


function muaSp(productId) {
    var cart = localStorage.getItem("cart");
    if (cart.length) {
        cart = JSON.parse(cart)
    } else {
        cart = []
    }
    $.ajax({
        type: 'POST',
        url: 'https://localhost:44368/api/GioHangAPI/add/' + productId,
        contentType: "application/json",
        data: JSON.stringify(cart),
        success: function (res) {
            alert("Thêm vào giỏ hàng thành công")
            localStorage.setItem("cart", JSON.stringify(res));
        },
        error: function (error) {
            console.log(error);
        }
    })
}
