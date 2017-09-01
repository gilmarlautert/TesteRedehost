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
            
            var cnn = ConnectionMultiplexer.Connect("redis-17834.c15.us-east-1-2.ec2.cloud.redislabs.com:17834");
            _cache = cnn.GetDatabase();
        }

        // GET api/values
        [HttpGet] 
        public IEnumerable<string> Get(string text)
        {

            var key = "tld";
            var entries = new List<string>();
            //_cache.SortedSetScan(key).Select;
            foreach (var res2 in _cache.SortedSetScan(key, text))
            {
                entries.Add(res2.Element);
            };
            
            return entries;
        }
    }
}