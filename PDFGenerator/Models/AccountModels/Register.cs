using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PDFGenerator.Models.AccountModels
{
    public class Register
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Nazwa użytkownika")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Imię użytkownika")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Nazwisko użytkownika")]
        public string SurName { get; set; }

        [Required]
        [StringLength(999, ErrorMessage = "Hasło musi mieć minimum {2} długości.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź hasło")]
        [Compare("Password", ErrorMessage = "Hasło i potwierdzenie nie są takie same.")]
        public string ConfirmPassword { get; set; }

    }
}
