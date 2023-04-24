using BTLWeb.Constants;
using BTLWeb.Models;
using BTLWeb.Models.ViewModels;
using BTLWeb.Service;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using System.Text.Json;
using System.Linq;
using System.Collections.Generic;

namespace BTLWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GioHangAPIController : ControllerBase
    {
        QlbanMayAnhContext db = new QlbanMayAnhContext();
        HoaDonBanService hoaDonBanService = new HoaDonBanService();
         
        [HttpPost("add/{productId}")]
        public JsonResult Add(string productId, [FromBody] List<ShoppingCartViewModel> cart)
        {
            /*var cart = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.SessionCart);*/
            if (cart == null)
            {
                cart = new List<ShoppingCartViewModel>();
            }
            if (cart.Any(x => x.ProductId == productId))
            {
                foreach (var item in cart)
                {
                    if (item.ProductId == productId)
                    {
                        item.Quantity += 1;
                    }
                }
            }
            else
            {
                ShoppingCartViewModel newItem = new ShoppingCartViewModel();
                newItem.ProductId = productId;
                var product = db.TDanhMucSps.Find(productId);
                newItem.Product = product;
                newItem.Quantity = 1;
                cart.Add(newItem);
            }

            HttpContext.Session.Set<List<ShoppingCartViewModel>>(CommonConstants.SessionCart, cart);

            return new JsonResult(cart);
        }

        [HttpGet("check-khach-hang/{username}")]
        public JsonResult CheckKhachHang([FromQuery] string username)
        {
            if (username != null)
            {
                var result = db.TKhachHangs.Find(username);
                if (result != null)
                {
                    return new JsonResult("true");
                }
            }
            return new JsonResult("true");
        }

        [HttpPost("create-order-no-create-khach-hang")]
        public JsonResult CreateOrderNoCreateKhachHang([FromBody] string orderViewModel)
        {
            var username = HttpContext.Session.GetString("Username");

            var order = new JavaScriptSerializer().Deserialize<OrderViewModel>(orderViewModel);
            order.NgayHoaDon = DateTime.Now.ToString();
            var orderNew = new THoaDonBan();

            orderNew.UpdateOrder(order);
            orderNew.MaKhachHang = username;
            var cart = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.SessionCart);
            if (cart == null || cart.Count() == 0)
            {
                return new JsonResult("false");
            }
            List<TChiTietHdb> orderDetails = new List<TChiTietHdb>();

            foreach (var item in cart)
            {
                var detail = new TChiTietHdb();
                detail.MaSp = item.ProductId;
                detail.SoLuongBan = item.Quantity;
                detail.DonGiaBan = item.Product.GiaLonNhat;
                orderDetails.Add(detail);
            }

            if (hoaDonBanService.Create(orderNew, orderDetails) == true)
            {
                return new JsonResult("true");
            }
            else
            {
                return new JsonResult("false");
            }
        }

        [HttpPost("CreateOrder")]
        public JsonResult CreateOrder(string orderViewModel, string khachHang)
        {
            TKhachHang _khachHang = new JavaScriptSerializer().Deserialize<TKhachHang>(khachHang);
            if (HttpContext.Session.GetString("Username") != null)
            {
                _khachHang.Username = HttpContext.Session.GetString("Username");
                _khachHang.MaKhanhHang = HttpContext.Session.GetString("Username");
                db.TKhachHangs.Add(_khachHang);
                db.SaveChanges();
            }

            var order = new JavaScriptSerializer().Deserialize<OrderViewModel>(orderViewModel);
            order.NgayHoaDon = DateTime.Now.ToString();
            var orderNew = new THoaDonBan();

            orderNew.UpdateOrder(order);
            orderNew.MaKhachHang = _khachHang.MaKhanhHang;
            var cart = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.SessionCart);
            if (cart == null || cart.Count() == 0)
            {
                return new JsonResult(new
                {
                    status = false
                });
            }
            List<TChiTietHdb> orderDetails = new List<TChiTietHdb>();

            foreach (var item in cart)
            {
                var detail = new TChiTietHdb();
                detail.MaSp = item.ProductId;
                detail.SoLuongBan = item.Quantity;
                detail.DonGiaBan = item.Product.GiaLonNhat;
                orderDetails.Add(detail);
            }

            if (hoaDonBanService.Create(orderNew, orderDetails) == true)
            {
                return new JsonResult(new
                {
                    status = true
                });
            }
            else
            {
                return new JsonResult(new
                {
                    status = false
                });
            }
        }
    }
}