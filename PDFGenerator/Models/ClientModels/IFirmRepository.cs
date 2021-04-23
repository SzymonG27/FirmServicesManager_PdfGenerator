using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDFGenerator.Models.ClientModels
{
    public interface IFirmRepository
    {
        public IQueryable<Firm> Firms { get; }
        void SaveFirm(Firm firm);
        Firm DeleteFirm(int FirmID);
    }
}
