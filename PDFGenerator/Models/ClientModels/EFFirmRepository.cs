using PDFGenerator.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDFGenerator.Models.ClientModels
{
    public class EFFirmRepository : IFirmRepository
    {
        private ApplicationDbContext context;
        public EFFirmRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }
        public IQueryable<Firm> Firms => context.Firms;

        public void SaveFirm(Firm firm)
        {
            if (firm.ID == 0)
            {
                context.Firms.Add(firm);
            }
            else
            {
                Firm dbEntry = context.Firms
                    .FirstOrDefault(p => p.ID == firm.ID);
                if (dbEntry != null)
                {
                    dbEntry.FirmName = firm.FirmName;
                    dbEntry.NIP = firm.NIP;
                }
            }
            context.SaveChanges();
        }

        public Firm DeleteFirm(int FirmID)
        {
            Firm dbEntry = context.Firms
                .FirstOrDefault(f => f.ID == FirmID);
            if (dbEntry != null)
            {
                context.Firms.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
