using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartInventory.BL.DTOs.Account;
using SmartInventory.BL.Services.AccountService;

namespace SmartInventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto login)
        {
            var res = await _accountService.Login(login);
            return res.IsSuccess ? Ok(res) : BadRequest(res);
        }

        // Register
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto register)
        {
            var res = await _accountService.Register(register);
            return res.IsSuccess ? Ok(res) : BadRequest(res);
        }
    }
}
