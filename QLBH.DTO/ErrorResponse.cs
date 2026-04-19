using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBH.DTO
{
    public class ErrorResponse
    {
        public ErrorDetail error { get; set; }

        public class ErrorDetail
        {
            public string userMessage { get; set; }
            public string internalMessage { get; set; }
            public int code { get; set; }
        }
    }
}
