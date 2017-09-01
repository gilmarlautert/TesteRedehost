using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ProjetoRedehost.Models;
using Microsoft.AspNetCore.Identity;
using ProjetoRedehost.Helpers;
using System.Threading.Tasks;
using ProjetoRedehost.Data;
using ProjetoRedehost.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ProjetoRedehost.Controllers
{
    [Route("api/[controller]")]
    public class AccountsController : Controller
    {
         private readonly UserManager<ApplicationUser> _userManager;
         private readonly IMapper _mapper;

        public AccountsController(UserManager<ApplicationUser> userManager,IMapper mapper,ApplicationDbContext appDbContext)
        {
            _userManager = userManager;
            _mapper=mapper;
        }

        // POST api/accounts
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdentity=_mapper.Map<ApplicationUser>(model);

            var result = await _userManager.CreateAsync(userIdentity, model.Password);

            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            return new OkObjectResult("Account created");
        }
       
     }
   
}