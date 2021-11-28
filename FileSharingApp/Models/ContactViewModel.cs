using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FileSharingApp.Models
{
    public class ContactViewModel
    {
        public string Id { get; set; }
        [Required]
        public string  Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Message { get; set; }

        public bool IsClosed { get; set; }
        public DateTime SendDate { get; set; }

    }
}
