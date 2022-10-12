using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurfsUp.Models;
using static System.Net.WebRequestMethods;

namespace SurfsUp.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentApiController : ControllerBase
    {
        [HttpGet("Rent")]
        public async Task<IActionResult> Rent([FromForm] HttpClient client, string userId, Guid id)
        {
            return Ok(await client.GetStringAsync($"https://localhost:7154/rent?userId={userId}&id={id}"));

        
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index([FromForm] HttpClient client)
        {
            client.BaseAddress = new Uri("https://localhost:7154");
            return Ok(await client.GetStringAsync("/Index"));
        }
        
    }
}
