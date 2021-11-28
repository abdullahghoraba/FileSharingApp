using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FileSharingApp.Data
{
    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
 
        public string Email { get; set; }
      
        public string Subject { get; set; }
        public string Message { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }

        public bool IsClosed { get; set; }
        public DateTime SendDate { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
