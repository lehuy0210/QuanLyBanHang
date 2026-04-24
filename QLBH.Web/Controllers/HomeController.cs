using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QLBH.DTO;
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

        public async Task<IActionResult> Index(int? CategoryID, int? SupplierID)
        {
            var client = _httpClientFactory.CreateClient();

            string apiProductList = "http://localhost:5003/api/Product";

            var resCat = await client.GetAsync("http://localhost:5003/api/Product/categories");
            if (resCat.IsSuccessStatusCode)
            {
                var jsonCat = await resCat.Content.ReadAsStringAsync();
                ViewBag.Categories = JsonConvert.DeserializeObject<List<CategoryDTO>>(jsonCat);
            }

            var resSup = await client.GetAsync("http://localhost:5003/api/Product/suppliers");
            if (resSup.IsSuccessStatusCode)
            {
                var jsonSup = await resSup.Content.ReadAsStringAsync();
                ViewBag.Suppliers = JsonConvert.DeserializeObject<List<SupplierDTO>>(jsonSup);
            }

            var response = await client.GetAsync(apiProductList);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();

                DataTable dtProducts = JsonConvert.DeserializeObject<DataTable>(jsonString);

                if (dtProducts != null && dtProducts.Rows.Count > 0)
                {
                    if (CategoryID.HasValue)
                    {
                        var rowsToKeep = dtProducts.AsEnumerable().Where(r => Convert.ToInt32(r["CategoryID"]) == CategoryID.Value);
                        dtProducts = rowsToKeep.Any() ? rowsToKeep.CopyToDataTable() : dtProducts.Clone();
                    }

                    if (SupplierID.HasValue && dtProducts.Rows.Count > 0)
                    {
                        var rowsToKeep = dtProducts.AsEnumerable().Where(r => Convert.ToInt32(r["SupplierID"]) == SupplierID.Value);
                        dtProducts = rowsToKeep.Any() ? rowsToKeep.CopyToDataTable() : dtProducts.Clone();
                    }
                }

                    return View(dtProducts);
            }
            return View(new DataTable());
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
