using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PDFGenerator.Models.ClientModels
{
    public class ClientFirmRelation
    {
        public string ClientID { get; set; }
        public int FirmID { get; set; }
    }
}
