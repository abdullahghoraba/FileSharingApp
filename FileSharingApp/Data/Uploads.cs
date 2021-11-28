using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileSharingApp.Data
{
    public class Uploads
    {
        public Uploads()
        {
            Id = Guid.NewGuid().ToString();
            UploadDate = DateTime.Now;
        }
        public string Id { get; set; }
        public string OriginalFileName { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public decimal Size { get; set; }
        public DateTime UploadDate { get; set; }
        public string UserId { get; set; }
        public ApplicationUser IdentityUser { set; get; }
        public long DownloadCount { get; set; }

    }
}
