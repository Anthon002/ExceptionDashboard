using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionDashboard.Core.Models.DTOs
{
    public class ExceptionHeaderDTO
    {
        // public string Id { get; set; }
        [Required]
        public string ExceptionCode { get; set; }
        [Required]
        public string AppId { get; set; }
    }
}
