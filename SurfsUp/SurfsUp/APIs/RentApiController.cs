using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SurfsUp.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentApiController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Rent([FromForm] HttpClient client)
        {
            return Ok(await client.GetStringAsync("https://localhost:7154/api/Rentals/rent?id=63AFBFE5-7CF2-48B0-AD75-8CBDF9056BD1"));
        }
    }
}
