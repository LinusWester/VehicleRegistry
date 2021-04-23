using System.Web.Http;
using VehicleRegistry.Domain.Classes;
using VehicleRegistry.DTO.dto;
using VehicleRegistry.Repository.Interfaces;

namespace VehicleRegistry.API.Controllers
{
    public class AccountController : ApiController
    {
        private readonly IAccountRepository accountRepository;
            public AccountController(IAccountRepository accountRepository)
            {
                this.accountRepository = accountRepository;
            }

            //[Authorize]
            [HttpPost]
            [Route("api/createaccount")]
            public IHttpActionResult CreateAccount(AccountRequestDto accountRequestDto)
            {
                var account = new Account(accountRequestDto.Username, accountRequestDto.Authorized, accountRequestDto.Password);
                accountRepository.RegisterAccount(account);
                return Ok();
            }
    }
    
}