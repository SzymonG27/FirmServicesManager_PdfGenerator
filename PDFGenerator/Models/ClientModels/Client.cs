using PDFGenerator.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PDFGenerator.Models.ClientModels
{
    public class Client
    {
        public string ID { get; set; } = Guid.NewGuid().ToString();
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public int PhoneNumber { get; set; }

        [UIHint("isFirm")]
        public bool isFirm { get; set; }

        [EmailAddress]
        public string EMail { get; set; }

        [NotMapped]
        public string FirmName { get; set; }
        
        [NotMapped]
        [IntLength(10, 10, false)]
        public int NIP { get; set; }
    }
}
