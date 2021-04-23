using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PDFGenerator.Models.ClientModels
{
    public class Accesory
    {
        [Key]
        public int ID { get; set; }
        public int FixID { get; set; }
        public string NameOfAccesory { get; set; }
        public int? NumberOfAccesory { get; set; } //Optional
    }
}
