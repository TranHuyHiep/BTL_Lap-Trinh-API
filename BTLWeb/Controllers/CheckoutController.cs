using BTLWeb.Constants;
using BTLWeb.Models;
using BTLWeb.Models.ViewModels;
using BTLWeb.Service;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using System.Text.Json;
using X.PagedList;

namespace BTLWeb.Controllers
{
    public class CheckoutController : Controller
    {
        QlbanMayAnhContext db = new QlbanMayAnhContext();
        public JsonResult DanhMucHoaDon(string maKhachHang)
        {
            var lst = db.THoaDonBans
                .Where(x => x.MaKhachHang.Equals(maKhachHang))
                .OrderBy(x => x.MaHoaDon);
            List<THoaDonBan> donBans = lst.ToList();
            return new JsonResult(donBans);
        }
        public JsonResult XacNhanDonHang(int maHoaDon)
        {
            var lst = db.THoaDonBans
                .Where(x => x.MaHoaDon == maHoaDon)
                .OrderBy(x => x.MaHoaDon);
            List<THoaDonBan> donBans = lst.ToList();

            foreach (var donBan in donBans)
            {
                donBan.Status = 1;
            }

            db.SaveChanges();

            return new JsonResult(donBans);
        }

        public JsonResult XacNhanGiaoHang(int maHoaDon)
        {
            var lst = db.THoaDonBans
                .Where(x => x.MaHoaDon == maHoaDon)
                .OrderBy(x => x.MaHoaDon);
            List<THoaDonBan> donBans = lst.ToList();

            foreach (var donBan in donBans)
            {
                donBan.Status = 2;
            }

            db.SaveChanges();

            return new JsonResult(donBans);
        }

        public JsonResult HuyDonHang(int maHoaDon)
        {
            var lst = db.THoaDonBans
                .Where(x => x.MaHoaDon == maHoaDon)
                .OrderBy(x => x.MaHoaDon);
            List<THoaDonBan> donBans = lst.ToList();

            foreach (var donBan in donBans)
            {
                donBan.Status = 3;
            }

            db.SaveChanges();

            return new JsonResult(donBans);
        }

    }
}
