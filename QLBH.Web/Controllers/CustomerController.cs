using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using static System.Net.WebRequestMethods;

namespace QLBH.Web.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CustomerController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> List()
        {
            var client = _httpClientFactory.CreateClient();

            string apiCustomer = "http://localhost:5003/api/Customer/List";

            var response = await client.PostAsync(apiCustomer, null);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();

                DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonString);

                return View(dt);
            }

            return View(new DataTable());
        }
    }
}
