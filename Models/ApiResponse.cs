using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChowHub.Models
{
    public class ApiResponse<T>
    {
        public int Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
    }
}