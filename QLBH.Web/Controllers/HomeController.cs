using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;

namespace QLBH.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();

            string apiProductList = "http://localhost:5003/api/Product/List";

            var response = await client.GetAsync(apiProductList);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();

                DataTable dtProducts = JsonConvert.DeserializeObject<DataTable>(jsonString);

                return View(dtProducts);
            }
            return View(new DataTable());
        }
    }
}
