using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            //var rng = new Random();
            //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            //{
            //    Date = DateTime.Now.AddDays(index),
            //    TemperatureC = rng.Next(-20, 55),
            //    Summary = Summaries[rng.Next(Summaries.Length)]
            //})
            //.ToArray();
            return $"АПІ сервер запущено (не закривай вiкно браузера, бо сервер вимкнеться) \n " +
                $"Доступні команди: \n" +
                $" 1) отримати цитату великих людей гет запит (пересила запит на АПIшку з https://rapidapi.com/ як показував Корнага): https://localhost:44338/api/Test \n" +
                $" 2) отримати погоду за англiйською назвою мiста гет запит https://localhost:44338/api/Test/англ_назва_міста перетворює на гет запит http://api.openweathermap.org/data/2.5/weather?q=англ_назва_міста&appid=cc89b60d56dbc5e3212c523e9ffb4b7a отриману вiдповiдь десерiалiзує (бере з неї темературу, хмарнiсть, дощ)\n" +
                $" 3) Iнформацiя про фiльм по назвi https://localhost:44338/api/Test/film/англ_назва_фільму перетвоює на гет запит http://www.omdbapi.com/?t=название&apikey=7ceb2af2 \n" +
                $" 4) Пошук рецепту по назві https://localhost:44338/api/Test/resept/англ_назва_рецепту перетвоює на гет запит https://api.spoonacular.com/recipes/search?apiKey=28ac32055a7f4736ae27d8a95d6ff17b&query=англ_назва_рецепту \n" +
                $" 5) Генерацiя тексту https://localhost:44338/api/Test/text/номер перетвоює на гет запит http://rzhunemogu.ru/RandJSON.aspx?CType=номер \n" +
                $" 1 - Анекдот;"; // 2 - Рассказы; 3 - Стишки; 4 - Афоризмы; 5 - Цитаты; 6 - Тосты; 8 - Статусы; 11 - Анекдот(+18);  12 - Рассказы(+18); 13 - Стишки(+18); 14 - Афоризмы(+18); 15 - Цитаты(+18); 16 - Тосты(+18); 18 - Статусы(+18); ";
        }
    }
}