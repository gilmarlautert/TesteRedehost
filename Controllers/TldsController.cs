using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjetoRedehost.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using StackExchange.Redis; 
using ProjetoRedehost.Data;

namespace ProjetoRedehost.Controllers
{
    #if !DEBUG
    [Authorize]
    #endif
    [Route("api/[controller]")]
    public class TldsController : Controller
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly ILogger _logger;
        private readonly IDatabase _cache;

        private readonly string _key = "tld";
        public TldsController(ApplicationDbContext appDbContext,
            ILogger<TldsController> logger)
        {
            _logger = logger;
            _appDbContext=appDbContext;
            
            var cnn = ConnectionMultiplexer.Connect("redis-17834.c15.us-east-1-2.ec2.cloud.redislabs.com:17834");
            _cache = cnn.GetDatabase();
        }

        // GET api/values
        [HttpGet] 
        public IEnumerable<Tld> Get()
        {
           return _appDbContext.Tlds;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
        
            var result = _appDbContext.Tlds.Find(id);
            if (result == null)
            {
                return NotFound();
            }
            
            return Ok(result);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult>  Post([FromBody]Tld value)
        {
            var tld = _appDbContext.Tlds.Where(b => b.Extension == value.Extension).FirstOrDefault();    
            if (existeTld(value))
            {
                    return BadRequest("TLD já existe");
            }        
    
            value.UsuarioAlteracao = value.UsuarioCriacao = User.Identity.Name;
            value.DataAlteracao = value.DataCriacao = DateTime.Now;     

            _appDbContext.Tlds.Add(value);
            
            _cache.SortedSetAdd(_key, value.Extension, 0);
            _appDbContext.SaveChanges();
            
            return new OkObjectResult(value);
        }

        private bool existeTld(Tld value)
        {
            var tld = _appDbContext.Tlds.Where(b => b.Extension == value.Extension).FirstOrDefault();    
            if (tld != null)
            {
                    return true;
            }   
            return false;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult>  Put(int id, [FromBody]Tld value)
        {
            if (existeTld(value))
            {
                    return BadRequest("TLD já existe");
            }             
              
            var result = _appDbContext.Tlds.Find(id);
            if (result != null)
            {
                try
                {
                    result.Extension = value.Extension;
                    result.UsuarioAlteracao = User.Identity.Name;
                    result.DataAlteracao = DateTime.Now;
                    _cache.SortedSetRemove(_key,result.Extension,0);
                    
                    _cache.SortedSetAdd(_key, value.Extension, 0);
                    _appDbContext.SaveChanges();
                    return new OkObjectResult(result);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else{
               return NotFound();
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = _appDbContext.Tlds.SingleOrDefault(b => b.Id == id);
            if (result != null)
            {
                try
                {
                    _cache.SortedSetRemove(_key,result.Extension);
                    _appDbContext.Remove(result);
                    _appDbContext.SaveChanges();
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                } 
            }
            else{
               return NotFound();
            }
        }
    }
}
