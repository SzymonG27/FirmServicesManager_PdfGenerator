using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PDFGenerator.Models.AccountModels
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string SurName { get; set; }

    }
}
