using ExceptionDashboard.Core.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ExceptionDashboard.Core.Models.DTOs
{
    public class ExceptionDTO
    {
        public string Id { get; set; }
        public string ExceptionHeaderId { get; set; }
        public ExceptionStatus Status { get; set; }

        public string StatusText { get; set; }

        public string AppName { get; set; }

        [Required]
        public string ExceptionCode { get; set; }
        [Required]
        public string ExceptionMessage { get; set; }
        [Required]
        public string AppId { get; set; }
        [Required]
        public string StackTrace { get; set; }
    }
}
