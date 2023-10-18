using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionDashboard.Core.Models.DTOs
{
    public class ExceptionRequest
    {
        public string ExceptionMessage { get; set; }
        public string ExceptionCode { get; set; }
        public string AppId { get; set; }
        public string StackTrace { get; set; }
    }
}
