using BTLWeb.Constants;
using BTLWeb.Models;
using BTLWeb.Models.ViewModels;
using BTLWeb.Service;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using System.Text.Json;

namespace BTLWeb.Controllers
{
    public class ShoppingCartController : Controller
    {   
        QlbanMayAnhContext db = new QlbanMayAnhContext();
        HoaDonBanService hoaDonBanService = new HoaDonBanService();

        public ActionResult Index()
        {
            var cart = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.SessionCart);
            if (cart == null)
            {
                cart = new List<ShoppingCartViewModel>();
            }
            return View(cart);
        }

        [HttpPost]
        public JsonResult Add(string productId)
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

            return Json(new
            {
                status = true
            });
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            var cart = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.SessionCart);
            if (cart == null)
            {
                cart = new List<ShoppingCartViewModel>();
            }
            return Json(cart);
        }

        [HttpPost]
        public JsonResult Update(string cartData)
        {
            var cartViewModel = new JavaScriptSerializer().Deserialize<List<ShoppingCartViewModel>>(cartData);

            var cartSession = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.SessionCart);
            foreach (var item in cartSession)
            {
                foreach (var jitem in cartViewModel)
                {
                    if(item.ProductId == jitem.ProductId)
                    {
                        item.Quantity = jitem.Quantity;
                    }
                }
            }

            HttpContext.Session.Set<List<ShoppingCartViewModel>>(CommonConstants.SessionCart, cartSession);
            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public JsonResult DeleteAll()
        {

            HttpContext.Session.Set<List<ShoppingCartViewModel>>(CommonConstants.SessionCart, new List<ShoppingCartViewModel>());
            return Json(new
            {
                status = true
            });
        }

        public JsonResult CheckKhachHang(string username)
        {
            var result = db.TUsers.Where(s => s.Username.Equals(username));
            if (result == null)
            {
                return new JsonResult(false);
                }
            return new JsonResult(true);
        }

        public JsonResult CreateOrderNoCreateKhachHang(string orderViewModel, string cartLocal, string username)
        {
            var order = new JavaScriptSerializer().Deserialize<OrderViewModel>(orderViewModel);
            order.NgayHoaDon = DateTime.Now.ToString();
            var orderNew = new THoaDonBan();

            orderNew.UpdateOrder(order);
            orderNew.MaKhachHang = username;
            var cart = new JavaScriptSerializer().Deserialize<List<ShoppingCartViewModel>>(cartLocal);
            if (cart == null || cart.Count() == 0)
            {
                return new JsonResult(false);
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
                return new JsonResult(true);
            }
            else
            {
                return new JsonResult(false);
            }
        }

        public JsonResult CreateOrder(string orderViewModel, string khachHang, string cartLocal, string username)
        {
            TKhachHang _khachHang = new JavaScriptSerializer().Deserialize<TKhachHang>(khachHang);
            var temp = db.TUsers.Where(s => s.Username.Equals(username));
            if (username != null && temp == null)
            {
                _khachHang.Username = username;
                _khachHang.MaKhanhHang = username;
                db.TKhachHangs.Add(_khachHang);
                db.SaveChanges();
            }

            var order = new JavaScriptSerializer().Deserialize<OrderViewModel>(orderViewModel);
            order.NgayHoaDon = DateTime.Now.ToString();
            var orderNew = new THoaDonBan();

            orderNew.UpdateOrder(order);
            orderNew.MaKhachHang = _khachHang.MaKhanhHang;
            var cart = new JavaScriptSerializer().Deserialize<List<ShoppingCartViewModel>>(cartLocal);

            if (cart == null || cart.Count() == 0)
            {
                return Json(new
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

            if(hoaDonBanService.Create(orderNew, orderDetails) == true)
            {
                return new JsonResult(true);
            }
            else
            {
                return new JsonResult(false);
            }
        }
    }
}
