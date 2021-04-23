using PDFGenerator.Models.AccountModels;
using PDFGenerator.Models.ClientModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDFGenerator.Models.ViewModels
{
    public class AppUserViewModel
    {
        public IEnumerable<AppUser> AppUsers { get; set; }
        public AppUser AppUser { get; set; }
        public string Rank { get; set; }
    }
}
