using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Web;
using System.IO;

using Newtonsoft.Json;
using RestSharp.Extensions;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class TestController : ControllerBase
    {
        // GET: api/Test
        [HttpGet]
        public IEnumerable<string> Get()
        {
            try
            {
                Aforizm Res = new Aforizm();
                return new string[] { Res.Deserialize() };
            }
            catch
            {
                return new string[] { "" };
            }
        }

        // GET: api/Test/англ_назва_мiста
        [HttpGet("{id}", Name = "Get")]
        public async Task<string> Get(string id)
        {
            string rezult;
            try
            {
                using var client = new HttpClient();
                var content = await client.GetStringAsync($"http://api.openweathermap.org/data/2.5/weather?q={id}&appid=cc89b60d56dbc5e3212c523e9ffb4b7a");
                Pogoda_predstavlenie pogoda = System.Text.Json.JsonSerializer.Deserialize<Pogoda_predstavlenie>(content);
                rezult = "в " + id + " зараз " + pogoda.weather[0].main + " " + pogoda.weather[0].description + " температура " + Math.Round(pogoda.main["temp"] - 273.15) + " градусiв цельсiя";
            }
            catch
            {
                rezult = "Не знайдено";
            }
            return rezult;
        }

        // GET: api/Test/film/англ_назва_фільму
        [HttpGet("film/" + "{id}", Name = "Get2")]
        public async Task<string> Get2(string id)
        {
            string rezult;
            try
            {
                using var client = new HttpClient();
                var content = await client.GetStringAsync($"http://www.omdbapi.com/?t={id}&apikey=7ceb2af2");
                film f = System.Text.Json.JsonSerializer.Deserialize<film>(content);
                rezult = f.Title + "\nбув випущен в " + f.Year + "\nжанр: " + f.Genre + "\nАктори: " + f.Actors + "\nрейтинг imdb: " + f.imdbRating + "\nсюжет: " + f.Plot;
            }
            catch
            {
                rezult = "Не знайдено";
            }
            return rezult;
        }
        // GET: api/Test/resept/англ_назва_рецепту
        [HttpGet("resept/" + "{id}", Name = "Get3")]
        public async Task<string> Get3(string id)
        {
            string rez = "";
            try
            {
                using var client = new HttpClient();
                var content = await client.GetStringAsync($"https://api.spoonacular.com/recipes/search?apiKey=28ac32055a7f4736ae27d8a95d6ff17b&query={id}");
                Resept r = System.Text.Json.JsonSerializer.Deserialize<Resept>(content);
                if (r != null) foreach (Resept.resept1 x in r.results) rez += " назва: \n " + x.title + " \n сайт з рецептом: \n " + x.sourceUrl + " \n \n";
                if (rez == "") rez = "рецепт не знайдено";
            }
            catch
            {
                rez = "Не знайдено";
            }
            return rez;
        }
        // GET: api/Test/text/{1,2,3,4,5,6,8,11,12,13,14,15,16,18}
        [HttpGet("text/" + "{id}", Name = "Get4")]
        public async Task<string> Get4(string id)
        {
            string rez = "";
            try
            {
                using var client = new HttpClient();
                byte[] bytes = await client.GetByteArrayAsync($"http://rzhunemogu.ru/RandJSON.aspx?CType={id}");
            }
            catch
            {
            }
            return "ссилка на анекдот: http://rzhunemogu.ru/RandJSON.aspx?CType=1";

        }
        // GET: api/Test/hisory/id_чата
        [HttpGet("hisory/" + "{id}", Name = "Get5")]
        public async Task<string> Get5(string id)
        {
            string rezult = "";
            try
            {
                List<log> history = JsonConvert.DeserializeObject<List<log>>(System.IO.File.ReadAllText(@"user.json"));
                int k = 0;
                if (history == null) history = new List<log>();
                for (int i = 0; i < history.Count; i++)
                    if (history[i].id == int.Parse(id)) { rezult += "№" + (k++) + ") " + history[i].info + "\n"; }
            }
            catch
            {

            }
            return "Історія чату: \n" + rezult;
        }

        // POST: api/Test
        [HttpPost()]
        public async Task<string> Post([FromBody] log value)
        {
            try
            {
                List<log> history = JsonConvert.DeserializeObject<List<log>>(System.IO.File.ReadAllText(@"user.json"));
                if (history == null) history = new List<log>();
                history.Add(value);
                int n = 5000;
                if (history.Count > n)
                {
                    List<log> history2 = new List<log>();
                    for (int i = 0; i <= n / 2; i++)
                    {
                        history2.Add(history[history.Count - 1 - n / 2 + i]);
                    }
                    await System.IO.File.WriteAllTextAsync(@"user.json", JsonConvert.SerializeObject(history2, Formatting.Indented));
                }
                else
                    await System.IO.File.WriteAllTextAsync(@"user.json", JsonConvert.SerializeObject(history, Formatting.Indented));
            }
            catch
            {

            }
            return "add " + value.id + value.info;
        }

        // PUT: api/Test/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] log value)
        {
            try
            {
                List<log> history = JsonConvert.DeserializeObject<List<log>>(System.IO.File.ReadAllText(@"user.json"));
                int k = -1;
                int i;
                for (i = 0; i < history.Count; i++) if (history[i].id == value.id)
                    {
                        k++;
                        if (k == id) break;
                    }
                if (history[i].id == value.id)
                {
                    if (history == null) history = new List<log>();
                    else
                    {
                        history[i] = value;
                    }
                    await System.IO.File.WriteAllTextAsync(@"user.json", JsonConvert.SerializeObject(history, Formatting.Indented));
                }
            }
            catch
            {

            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}/{id2}")]
        public async Task Delete(int id, int id2) // id - номер повідомлення, id2 - номер чату
        {
            try
            {
                List<log> history = JsonConvert.DeserializeObject<List<log>>(System.IO.File.ReadAllText(@"user.json"));
                
                int k = -1;
                int i;
                for (i = 0; i < history.Count; i++) if (history[i].id == id2)
                    {
                        k++;
                        if (k == id) break;
                    }

                if (history == null)
                {
                    history = new List<log>();
                }
                else
                {
                    if (id != -1)
                    {
                        if (history[i].id == id2) history.RemoveAt(i);
                    }
                    else
                    {
                        for (int j = history.Count - 1; j >= 0; j--) if (history[j].id == id2) history.RemoveAt(j);
                    }
                }
                await System.IO.File.WriteAllTextAsync(@"user.json", JsonConvert.SerializeObject(history, Formatting.Indented));
            }
            catch
            {

            }
        }
    }
}
