using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.BL.DTOs.Account
{
    public record AuthResponseDto
    {
        public string token { get; set; } = string.Empty;
        public DateTime Expiration {  get; set; }
    }
}
