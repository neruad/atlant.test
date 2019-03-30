using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitcoinTransactions.Api.Models.Response
{
    public class ApiResponse
    {
        public int Code { get; set; }

        public string Message { get; set; }
    }

    public class ApiResponse<T>
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }
    }
}