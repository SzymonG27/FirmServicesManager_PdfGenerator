using PDFGenerator.Models.AccountModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDFGenerator.Models.ClientModels
{
    public class PdfMakerModel
    {
        public Client Client { get; set; }
        public Firm Firm { get; set; }
        public Fix Fix { get; set; }
        public List<string> FixNames { get; set; }
        public IEnumerable<Accesory> Accesories { get; set; }
        public AppUser AppUser { get; set; }
    }
}
