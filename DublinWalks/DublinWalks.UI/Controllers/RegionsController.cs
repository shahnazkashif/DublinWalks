using DublinWalks.UI.Models;
using DublinWalks.UI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace DublinWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory )
        {
            this.httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult>Index()
        {
            List<RegionDto> response = new List<RegionDto>(); 
            try
            {
                // get all regions from web API
                var client = httpClientFactory.CreateClient();
                //ideally this url should come from app settings
                var httpResponseMessage = await client.GetAsync("http://localhost:5189/api/Regions");
                httpResponseMessage.EnsureSuccessStatusCode();
               // var response = await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>();
                 response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>());

                // ViewBag.Response = stringResponseBody;   
            }
            catch (Exception ex)
            {
                //log the exception
            }

            return View(response);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddRegionViewModel model)
        {
            var client = httpClientFactory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://localhost:5189/api/Regions"),
                Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json")

            };

           var httpResponseMessage = await client.SendAsync(httpRequestMessage);
            httpResponseMessage.EnsureSuccessStatusCode();

           var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDto>();
            if (response is not null)
            {
                return RedirectToAction("Index", "Regions");
            }

            return View();


        }


    }
}
