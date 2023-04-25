using BTLWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace BTLWeb.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeAdminAPIController : ControllerBase
    {
        QlbanMayAnhContext db = new QlbanMayAnhContext();

        [HttpGet("DanhMucSanPham")]
        public JsonResult DanhMucSanPham([FromQuery] int? page)
        {
            int pageSize = 8;
            int pageNumber = page == null || page < 1 ? 1 : page.Value;
            var lstSanPham = db.TDanhMucSps.AsNoTracking().OrderBy(x => x.TenSp);
            PagedList<TDanhMucSp> pageList = new PagedList<TDanhMucSp>(lstSanPham, pageNumber, pageSize);
            return new JsonResult(pageList);
        }

        [HttpGet("XacNhanDonHang")]
        public JsonResult XacNhanDonHang(String maSanPham)
        {
            // TODO
            return new JsonResult("");
        }


        [HttpGet("DanhMucHoaDon")]
        public JsonResult DanhMucHoaDon([FromQuery] int? page)
        {
            int pageSize = 8;
            int pageNumber = page == null || page < 1 ? 1 : page.Value;
            var lst = db.THoaDonBans.AsNoTracking().OrderBy(x => x.MaHoaDon);
            PagedList<THoaDonBan> pageList = new PagedList<THoaDonBan>(lst, pageNumber, pageSize);
            return new JsonResult(pageList);
        }

        [HttpGet("ThemSanPham")]
        public JsonResult ThemSanPham()
        {
            // Convert ViewBag to JSON
            var jsonData = new
            {
                MaChatLieu = db.TChatLieus.Select(cl => new { Value = cl.MaChatLieu, Text = cl.ChatLieu }),
                MaHangSx = db.THangSxes.Select(hs => new { Value = hs.MaHangSx, Text = hs.HangSx }),
                MaNuocSx = db.TQuocGia.Select(qg => new { Value = qg.MaNuoc, Text = qg.TenNuoc }),
                MaLoai = db.TLoaiSps.Select(ls => new { Value = ls.MaLoai, Text = ls.Loai }),
                MaDt = db.TLoaiDts.Select(ld => new { Value = ld.MaDt, Text = ld.TenLoai })
            };
            return new JsonResult(jsonData);
        }

        [ValidateAntiForgeryToken] //Để nhập đúng sản phẩm
        [HttpPost("ThemSanPham")]
        public JsonResult ThemSanPham([FromQuery] TDanhMucSp sanPham)
        {
            if (ModelState.IsValid)
            {
                db.TDanhMucSps.Add(sanPham);
                db.SaveChanges();
                return new JsonResult("Error");
            }
            return new JsonResult(sanPham);
        }

        [HttpGet("SuaSanPham")]
        public JsonResult SuaSanPham([FromQuery] String maSanPham)
        {
            var jsonData = new
            {
                MaChatLieu = db.TChatLieus.Select(cl => new { Value = cl.MaChatLieu, Text = cl.ChatLieu }),
                MaHangSx = db.THangSxes.Select(hs => new { Value = hs.MaHangSx, Text = hs.HangSx }),
                MaNuocSx = db.TQuocGia.Select(qg => new { Value = qg.MaNuoc, Text = qg.TenNuoc }),
                MaLoai = db.TLoaiSps.Select(ls => new { Value = ls.MaLoai, Text = ls.Loai }),
                MaDt = db.TLoaiDts.Select(ld => new { Value = ld.MaDt, Text = ld.TenLoai }),
                sanPham = db.TDanhMucSps.Find(maSanPham),
            };
            return new JsonResult(jsonData);
        }

        [ValidateAntiForgeryToken] //Để nhập đúng sản phẩm
        [HttpPost("SuaSanPham")]
        public JsonResult SuaSanPham([FromQuery] TDanhMucSp sanPham)
        {
            if (ModelState.IsValid)
            {
                db.Update(sanPham).State = EntityState.Modified;
                db.SaveChanges();
                return new JsonResult("false");
            }
            return new JsonResult(sanPham);
        }

        [HttpGet("XemChiTietSP")]
        public JsonResult XemChiTietSP([FromQuery] String maSanPham)
        {
            var jsonData = new
            {
                MaChatLieu = db.TChatLieus.Select(cl => new { Value = cl.MaChatLieu, Text = cl.ChatLieu }),
                MaHangSx = db.THangSxes.Select(hs => new { Value = hs.MaHangSx, Text = hs.HangSx }),
                MaNuocSx = db.TQuocGia.Select(qg => new { Value = qg.MaNuoc, Text = qg.TenNuoc }),
                MaLoai = db.TLoaiSps.Select(ls => new { Value = ls.MaLoai, Text = ls.Loai }),
                MaDt = db.TLoaiDts.Select(ld => new { Value = ld.MaDt, Text = ld.TenLoai }),
                sanPham = db.TDanhMucSps.Find(maSanPham),
            };
            return new JsonResult(jsonData);
        }

        [Route("XemChiTietHD")]
        [HttpGet]
        public JsonResult XemChiTietHD([FromQuery] int mahoadon)
        {
            var jsonData = new
            {
                lst = db.TChiTietHdbs.Where(x => x.MaHoaDon == mahoadon).AsNoTracking(),
            };
            return new JsonResult(jsonData);
        }

        [HttpPost("XoaSanPham")]
        public JsonResult XoaSanPham([FromQuery] String maSanPham)
        {
            var listChiTiet = db.TDanhMucSps.Where(x => x.MaSp == maSanPham);
            foreach (var item in listChiTiet)
            {
                if (db.TChiTietHdbs.Where(x => x.MaSp == item.MaSp) != null)
                {
                    return new JsonResult("Không xóa được sản phẩm này!");
                }
            }
            var listAnh = db.TAnhSps.Where(x => x.MaSp == maSanPham);
            if (listAnh != null) db.RemoveRange(listAnh);
            if (listChiTiet != null) db.RemoveRange(listChiTiet);
            db.Remove(db.TDanhMucSps.Find(maSanPham));
            db.SaveChanges();
            return new JsonResult("Sản phẩm đã được xóa");
        }

        [HttpGet("ThongKe")]
        public JsonResult ThongKe()
        {
            List<int> ans = new List<int>();
            int nam = 2023;
            for (int thang = 1; thang <= 12; thang++)
            {
                List<THoaDonBan> lstHoaDon = db.THoaDonBans.FromSqlRaw("SELECT * FROM THoaDonBan WHERE YEAR(CONVERT(datetime, NgayHoaDon, 101)) = {0} and month(CONVERT(datetime, NgayHoaDon, 101)) = {1} and status = 3", nam, thang).ToList();
                var sum = 0;
                foreach (var item in lstHoaDon)
                {
                    sum += (int)item.TongTienHd;
                }
                ans.Add(sum);
            }
            return new JsonResult(ans);
        }
    }
}
