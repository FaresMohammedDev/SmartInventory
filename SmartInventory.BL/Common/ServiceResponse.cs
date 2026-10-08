using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.BL.Common
{
    public class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;

        public static ServiceResponse<T> Success(T data, string message = "")
        {
            return new ServiceResponse<T>
            {
                Data = data,
                IsSuccess = true,
                Message = message
            };
        }

        public static ServiceResponse<T> Fail(string message)
        {
            return new ServiceResponse<T>
            {
                Data = default,
                IsSuccess = false,
                Message = message
            };
        }
    }
}
