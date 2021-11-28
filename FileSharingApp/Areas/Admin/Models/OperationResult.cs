using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileSharingApp.Areas.Admin.Models
{
    public class OperationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public static OperationResult NotFound(string msg= "Item Not Found")
        {
            return new OperationResult()
            {
                Message = msg,
                Success = false
            };
        }
        public static OperationResult Sucesseded(string msg = "Operation Compleated Successfully !! ")
        {
            return new OperationResult()
            {
                Message = msg,
                Success = true
            };
        }
    }
}
