using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace ProjetoRedehost.Controllers
{
    [Route("api/[controller]")]
    public class WhoisController : Controller
    {
        private readonly IDatabase _cache;
        public WhoisController()
        {
            var cnn = ConnectionMultiplexer.Connect("redis-11461.c9.us-east-1-2.ec2.cloud.redislabs.com:11461");
            _cache = cnn.GetDatabase();
        }

        // GET api/values
        [HttpGet] 
        public IEnumerable<string> Get(string text)
        {
            var key = "tld";
            var entries = new List<string>();
            foreach (var res2 in _cache.SortedSetScan(key, text))
            {
                entries.Add(res2.Element);
            };
            
            return entries;
        }
    }
}