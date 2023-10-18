using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionDashboard.Core.Models
{
    public class ExceptionHeader
    {
        [Key]
        public string Id { get; set; }
        public string AppId { get; set; }
        public string ExceptionCode { get; set; }
    }
}
