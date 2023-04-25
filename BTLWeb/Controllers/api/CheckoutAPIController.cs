using BTLWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace BTLWeb.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutAPIController : ControllerBase
    {
        QlbanMayAnhContext db = new QlbanMayAnhContext();
        [HttpPost("DanhMucHoaDon")]
        public JsonResult DanhMucHoaDon([FromQuery] int? page, [FromBody] string maKhachHang)
        {
            int pageSize = 8;
            int pageNumber = page == null || page < 1 ? 1 : page.Value;
            var lst = db.THoaDonBans.AsNoTracking().Where(x => x.MaKhachHang.Equals(maKhachHang)).OrderBy(x => x.MaHoaDon);
            PagedList<THoaDonBan> pageList = new PagedList<THoaDonBan>(lst, pageNumber, pageSize);
            return new JsonResult(pageList);
        }
    }
}
