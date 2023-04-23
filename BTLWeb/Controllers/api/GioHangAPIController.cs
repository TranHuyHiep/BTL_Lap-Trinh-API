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
    [ApiController]
    [Route("api/[controller]")]
    public class GioHangAPIController : ControllerBase
    {
        QlbanMayAnhContext db = new QlbanMayAnhContext();
        HoaDonBanService hoaDonBanService = new HoaDonBanService();
         
        [HttpGet]
        public ActionResult<List<ShoppingCartViewModel>> Get()
        {
            var cart = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.SessionCart);
            if (cart == null)
            {
                cart = new List<ShoppingCartViewModel>();
            }
            return Ok(cart);
        }

        [HttpPost("{productId}")]
        public IActionResult Add(string productId)
        {
            var cart = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.SessionCart);
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

            return Ok(new
            {
                status = true
            });
        }

        [HttpPut]
        public IActionResult Update(List<ShoppingCartViewModel> cartViewModel)
        {
            var cartSession = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.SessionCart);
            foreach (var item in cartSession)
            {
                foreach (var jitem in cartViewModel)
                {
                    if (item.ProductId == jitem.ProductId)
                    {
                        item.Quantity = jitem.Quantity;
                    }
                }
            }

            HttpContext.Session.Set<List<ShoppingCartViewModel>>(CommonConstants.SessionCart, cartSession);
            return Ok(new
            {
                status = true
            });
        }

        [HttpDelete]
        public IActionResult DeleteAll()
        {
            HttpContext.Session.Set<List<ShoppingCartViewModel>>(CommonConstants.SessionCart, new List<ShoppingCartViewModel>());
            return Ok(new
            {
                status = true
            });
        }

        [HttpGet]
        [Route("api/check-khach-hang")]
        public IActionResult CheckKhachHang()
        {
            var username = HttpContext.Session.GetString("Username");
            if (username != null)
            {
                var result = db.TKhachHangs.Find(username);
                if (result != null)
                {
                    return Ok(new { status = true });
                }
            }
            return Ok(new { status = false });
        }

        [HttpPost]
        [Route("api/create-order-no-create-khach-hang")]
        public IActionResult CreateOrderNoCreateKhachHang(string orderViewModel)
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
                return Ok(new { status = false });
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
                return Ok(new { status = true });
            }
            else
            {
                return Ok(new { status = false });
            }
        }
    }
}