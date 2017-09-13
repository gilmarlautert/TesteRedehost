using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjetoRedehost.Services.tld.cache;
using ProjetoRedehost.Services.whois;

namespace ProjetoRedehost.Controllers
{
    [Route("api/[controller]")]
    public class WhoisController : Controller
    {
        private readonly ITldCache _cache;
        private readonly IWhois _whois;
        public WhoisController(ITldCache chache,  IWhois whois)
        {
            _cache = chache;
            _whois = whois;
        }

        // GET api/values
        [HttpGet] 
        public IEnumerable<string> Get()
        {
            return _cache.ListAll();
        }

        [HttpPost]
        public async Task<IActionResult> Post(string domain)
        {
            try
            {
                return Ok( await _whois.WhoisAsync(domain) );
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}