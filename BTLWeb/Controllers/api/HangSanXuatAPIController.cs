using BTLWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BTLWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangSanXuatAPIController : ControllerBase
    {
        QlbanMayAnhContext db = new QlbanMayAnhContext();
        [HttpGet("hangsanxuat")]
        public JsonResult GetAllProductByBrand([FromQuery] string mahangSx)
        {
            var lst = db.TDanhMucSps.Where(x => x.MaHangSx == mahangSx).OrderBy(x => x.TenSp).ToList();
            return new JsonResult(lst);
        }

        [HttpGet]
        public JsonResult GetAllBrand()
        {
            var lst = db.TDanhMucSps.ToList();
            return new JsonResult(lst);
        }
    }
}
