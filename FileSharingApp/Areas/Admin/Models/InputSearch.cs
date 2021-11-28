using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FileSharingApp.Areas.Admin.Models
{
    public class InputSearch
    {
        [MinLength(3)]
        [Required]
        public string Term { get; set; }
    }
}
