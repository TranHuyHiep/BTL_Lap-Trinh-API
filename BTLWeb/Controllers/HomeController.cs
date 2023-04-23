using Azure;
using BTLWeb.Constants;
using BTLWeb.Models;
using BTLWeb.Models.Dao;
using BTLWeb.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using Nancy.Session;
using Newtonsoft.Json.Converters;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Security.Cryptography;
using X.PagedList;

namespace BTLWeb.Controllers
{
    public class HomeController : Controller
    {
        QlbanMayAnhContext db = new QlbanMayAnhContext();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(int? page)
        {
            int pageSize = 8;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstsanpham  = db.TDanhMucSps.AsNoTracking().OrderBy(x => x.TenSp);
            PagedList<TDanhMucSp> lst = new PagedList<TDanhMucSp>(lstsanpham, pageNumber, pageSize);
            return View(lst);
        }

        public IActionResult Shop(int? page)
        {
            int pageSize = 8;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstsanpham = db.TDanhMucSps.AsNoTracking().OrderBy(x => x.TenSp);
            PagedList<TDanhMucSp> lst = new PagedList<TDanhMucSp>(lstsanpham, pageNumber, pageSize);
            return View(lst);
        }

        public IActionResult Search(String strSearch)
        {
            var lst = db.TDanhMucSps.Where(x => x.TenSp.ToLower().Contains(strSearch.ToLower())).OrderBy(x => x.TenSp);
            return View(lst);
        }
        public IActionResult SearchProductsByPriceRange(string[] priceRange)
        {
            List<TDanhMucSp> products = db.TDanhMucSps.ToList();

            if (priceRange == null || priceRange.Contains("all"))
            {
                return View(products);
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

            return View(filteredProducts.Distinct());
        }


        public IActionResult SanPhamTheoLoai(String maloai, int? page)
        {
            int pageSize = 8;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstsanpham = db.TDanhMucSps.AsNoTracking().Where(x => x.MaLoai == maloai).OrderBy(x => x.TenSp);
            PagedList<TDanhMucSp> lst = new PagedList<TDanhMucSp>(lstsanpham, pageNumber, pageSize);
            ViewBag.maloai = maloai;
            return View(lst);
        }

        public IActionResult SanPhamTheoHang(String mahangSx, int? page)
        {
            int pageSize = 8;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstsanpham = db.THangSxes.AsNoTracking().Where(x => x.MaHangSx == mahangSx).OrderBy(x => x.HangSx);
            PagedList<THangSx> lst = new PagedList<THangSx>(lstsanpham, pageNumber, pageSize);
            return View(lst);
        }
        public IActionResult ChiTietSanPham(string maSp)
        {
            var sanpham = db.TDanhMucSps.SingleOrDefault(x => x.MaSp== maSp);
            var anhSanPham = db.TAnhSps.Where(x => x.MaSp == maSp).ToList();

            ViewBag.ListComment = new CommentDao().ListComment(0,maSp);
            ViewBag.anhSanPham = anhSanPham;
            return View(sanpham);
        }

        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        [Route("api/v1/addcoment")]
        public IActionResult AddNewComment([FromBody] dynamic data)
        {   
            var format = "yyyy-MM-dd"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
            var cmt = Newtonsoft.Json.JsonConvert.DeserializeObject<Comment>(data.ToString(), dateTimeConverter);
            var id = db.TComments.Select(c => c.ID).Max();
            id = id == null ? 1 : id +1;
            cmt.ID = id;
            db.TComments.Add(cmt);
            db.SaveChanges();
            return Ok("okkkkk");
        }



    }
}