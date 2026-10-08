using SmartInventory.BL.Common;
using SmartInventory.BL.DTOs.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.BL.Services.AccountService
{
    public interface IAccountService
    {
        Task<ServiceResponse<AuthResponseDto>> Login(LoginDto login);
        Task<ServiceResponse<string>> Register(RegisterDto register);
    }
}
