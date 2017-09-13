using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjetoRedehost.Services.tld.cache;
using StackExchange.Redis;

namespace ProjetoRedehost.Controllers
{
    [Route("api/[controller]")]
    public class WhoisController : Controller
    {
        private readonly ITldCache _cache;
        public WhoisController(ITldCache chache)
        {
            _cache = chache;
            // var cnn = ConnectionMultiplexer.Connect("redis-11461.c9.us-east-1-2.ec2.cloud.redislabs.com:11461");
            // _cache = cnn.GetDatabase();
        }

        // GET api/values
        [HttpGet] 
        public IEnumerable<string> Get()
        {
            return _cache.ListAll();
            //return new string[] {".com",".com.br"};

            // var key = "tld";
            // var entries = new List<string>();
            // foreach (var res2 in _cache.SortedSetScan(key, text))
            // {
            //     entries.Add(res2.Element);
            // };
            
            // return entries;
        }

        [HttpPost]
        public async Task<IActionResult> Post(string domain)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            var stringTask = await client.GetStringAsync("http://whoiz.herokuapp.com/lookup.json?url="+domain);
            return Ok(stringTask);
        }
    }
}