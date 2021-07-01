using PDFGenerator.Models.ClientModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDFGenerator.Models.ViewModels
{
    public class FixesViewModel
    {
        public IEnumerable<Fix> Fixes { get; set; }
        public Fix Fix { get; set; }
        public string barcode { get; set; }
    }
}
