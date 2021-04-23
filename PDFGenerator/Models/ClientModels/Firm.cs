using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PDFGenerator.Models.ClientModels
{
    public class Firm
    {
        [Key]
        public int ID { get; set; }
        public string FirmName { get; set; }
        [MinLength(10, ErrorMessage = "Nip musi mieć 10 cyfr")]
        [MaxLength(10, ErrorMessage = "Nip musi mieć 10 cyfr")]
        public int NIP { get; set; }
    }
}
