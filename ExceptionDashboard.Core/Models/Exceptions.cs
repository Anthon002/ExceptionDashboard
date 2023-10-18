using ExceptionDashboard.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionDashboard.Core.Models
{
    public class Exceptions
    {
        [Key]
        public string Id { get; set; }
        public string ExceptionMessage { get; set; }
        public string StackTrace { get; set; }
        public string ExceptionHeaderId { get; set; }
        public ExceptionStatus Status { get; set; }
        public string ExceptionCode {get; set;}
        public string AppId { get; set; }
    }
}
