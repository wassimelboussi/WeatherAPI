using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WeatherAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;

        public WeatherController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet("{city}")]
        public async Task<IActionResult> Get(string city)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid=52341d9c6ef5fead97057d96a85f62d0&units=metric");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var weather = JsonConvert.DeserializeObject<WeatherData>(content);

                return Ok($"Temperature in {city}: {weather.Main.Temp}°C");
            }
            else
            {
                return BadRequest("Invalid city name");
            }
        }
    }

    public class WeatherData
    {
        public MainData Main { get; set; }
    }

    public class MainData
    {
        public double Temp { get; set; }
    }
}
