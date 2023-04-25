using BTLWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using X.PagedList;

namespace BTLWeb.Controllers
{
    public class CheckoutController : Controller
    {
        QlbanMayAnhContext db = new QlbanMayAnhContext();
        public JsonResult DanhMucHoaDon(string maKhachHang)
        {
            int? page = 1;
            int pageSize = 8;
            int pageNumber = page == null || page < 1 ? 1 : page.Value;
            var lst = db.THoaDonBans
                .Where(x => x.MaKhachHang.Equals(maKhachHang))
                .OrderBy(x => x.MaHoaDon);

            PagedList<THoaDonBan> pageList = new PagedList<THoaDonBan>(lst, pageNumber, pageSize);
            return new JsonResult(pageList);
        }
    }
}
