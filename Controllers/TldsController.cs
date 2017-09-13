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
using ProjetoRedehost.Services.tld;
using ProjetoRedehost.ViewModels;
using ProjetoRedehost.Exceptions;

namespace ProjetoRedehost.Controllers
{
    // #if !DEBUG
    // [Authorize]
    // #endif
    [Route("api/[controller]")]
    public class TldsController : Controller
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly ILogger _logger;
        private readonly IDatabase _cache;

        private readonly ITld _tldService;

        private readonly string _key = "tld";
        public TldsController(ApplicationDbContext appDbContext,
            ILogger<TldsController> logger,
            ITld tldService)
        {
            _logger = logger;
            _appDbContext=appDbContext;
            _tldService = tldService;
        }

        // GET api/tlds
        [HttpGet] 
        public IEnumerable<TldViewModel> Get()
        {
            return _tldService.ListAll().Select(x=> new TldViewModel{
                Id = x.Id,
                Extension = x.Extension,
                UsuarioAlteracao = x.UsuarioAlteracao,
                DataAlteracao = x.DataAlteracao
            });
        }

        // GET api/tlds/1
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try{
                var tld = _tldService.Find(id);
                var result = new TldViewModel(){
                    Id = tld.Id,
                    Extension = tld.Extension,
                    UsuarioAlteracao = tld.UsuarioAlteracao,
                    DataAlteracao = tld.DataAlteracao
                };
                return Ok(result);
            }
            catch(NotFoundException)
            {
                return NotFound();
            }
        }

        // POST api/tlds
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]TldViewModel value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try{
                var tld = new Tld() {
                    Extension = value.Extension
                };
                var result = _tldService.Add(tld);
                return Ok(result);
            }
            catch(BadRequestException e)
            {
                return BadRequest(e.Message);
            }
            catch(NotFoundException)
            {
                return NotFound();
            }
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

        // PUT api/tlds/5
        [HttpPut("{id}")]
        public async Task<IActionResult>  Put(int id, [FromBody]TldViewModel value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            try{
                var tld = new Tld() {
                    Extension = value.Extension
                };
                _tldService.Edit(tld);
                return Ok();
            }
            catch(BadRequestException e)
            {
                return BadRequest(e.Message);
            }
            catch(NotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try{
                _tldService.Remove(id);
                return Ok();
            }
            catch(BadRequestException e)
            {
                return BadRequest(e.Message);
            }
            catch(NotFoundException)
            {
                return NotFound();
            }
        }
    }
}
