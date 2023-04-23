using Azure;
using BTLWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using X.PagedList;

namespace BTLWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SanPhamAPIController : ControllerBase
    {
        QlbanMayAnhContext db = new QlbanMayAnhContext();

        [HttpGet("{page}")]
        public JsonResult Index(int? page)
        {
            int pageSize = 8;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstsanpham = db.TDanhMucSps.AsNoTracking().OrderBy(x => x.TenSp);
            PagedList<TDanhMucSp> lst = new PagedList<TDanhMucSp>(lstsanpham, pageNumber, pageSize);
            return new JsonResult(lst);
        }

        [HttpGet("search/{strSearch}")]
        public JsonResult Search(String strSearch)
        {
            List<TDanhMucSp> products = db.TDanhMucSps.ToList();
            return new JsonResult(products);
        }

        [HttpGet("SearchProductsByPriceRange")]
        public JsonResult SearchProductsByPriceRange([FromQuery] string[] priceRange)
        {
            List<TDanhMucSp> products = db.TDanhMucSps.ToList();

            if (priceRange == null || priceRange.Contains("all"))
            {
                return new JsonResult(products);
            }

            var filteredProducts = new List<TDanhMucSp>();
            foreach (var range in priceRange)
            {
                var priceRangeArray = range.Split('-');
                int minPrice = int.Parse(priceRangeArray[0]);
                int maxPrice = int.Parse(priceRangeArray[1]);

                var tempFilteredProducts = products.Where(p => p.GiaLonNhat >= minPrice && p.GiaLonNhat <= maxPrice).ToList();
                filteredProducts.AddRange(tempFilteredProducts);
            }

            return new JsonResult(filteredProducts.Distinct());
        }

        [HttpGet("SanPhamTheoLoai")]
        public JsonResult SanPhamTheoLoai([FromQuery] String maloai,[FromQuery] int? page)
        {
            int pageSize = 8;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstsanpham = db.TDanhMucSps.AsNoTracking().Where(x => x.MaLoai == maloai).OrderBy(x => x.TenSp);
            PagedList<TDanhMucSp> lst = new PagedList<TDanhMucSp>(lstsanpham, pageNumber, pageSize);
            return new JsonResult(lst);
        }

        [HttpGet("SanPhamTheoHang")]
        public JsonResult SanPhamTheoHang([FromQuery] String mahangSx, int? page)
        {
            int pageSize = 8;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstsanpham = db.THangSxes.AsNoTracking().Where(x => x.MaHangSx == mahangSx).OrderBy(x => x.HangSx);
            PagedList<THangSx> lst = new PagedList<THangSx>(lstsanpham, pageNumber, pageSize);
            return new JsonResult(lst);
        }

        [HttpGet("ChiTietSanPham")]
        public JsonResult ChiTietSanPham([FromQuery] string maSp)
        {
            var sanpham = db.TDanhMucSps.SingleOrDefault(x => x.MaSp == maSp);
            var anhSanPham = db.TAnhSps.Where(x => x.MaSp == maSp).ToList();
            var result = new
            {
                sanpham = sanpham,
                anhSanPham = anhSanPham
            };
            return new JsonResult(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return new JsonResult(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
