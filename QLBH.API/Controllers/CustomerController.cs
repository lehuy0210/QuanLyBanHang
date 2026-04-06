using Microsoft.AspNetCore.Mvc;
using QLBH.BLL;
using QLBH.DAL;

namespace QLBH.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly CustomerBLL bllKhachHang = new CustomerBLL();

        [HttpPost("List")]
        public IActionResult List()
        {
            return Ok(bllKhachHang.getKhachHang());
        }
    }
}
