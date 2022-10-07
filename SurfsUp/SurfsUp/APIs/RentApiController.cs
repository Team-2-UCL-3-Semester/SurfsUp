﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SurfsUp.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentApiController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Rent([FromForm] HttpClient client, string userId, Guid id)
        {
            return Ok(await client.GetStringAsync($"https://localhost:7154/api/Rentals/rent?userId={userId}&id={id}"));
        }
    }
}