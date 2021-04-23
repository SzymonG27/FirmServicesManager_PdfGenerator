using PDFGenerator.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDFGenerator.Models.ClientModels
{
    public class EFAccesoryRepository : IAccesoryRepository
    {
        private ApplicationDbContext context;
        public EFAccesoryRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }
        public IQueryable<Accesory> Accesories => context.Accesories;

        public void SaveAccesory(Accesory accesory)
        {
            if (accesory.ID == 0)
            {
                context.Accesories.Add(accesory);
            }
            else
            {
                Accesory dbEntry = context.Accesories
                    .FirstOrDefault(a => a.ID == accesory.ID);
                if (dbEntry != null)
                {
                    dbEntry.NameOfAccesory = accesory.NameOfAccesory;
                    dbEntry.NumberOfAccesory = accesory.NumberOfAccesory;
                }
            }
            context.SaveChanges();
        }

        public Accesory DeleteAccesory(int accesoryID)
        {
            Accesory dbEntry = context.Accesories
                .FirstOrDefault(a => a.ID == accesoryID);
            if (dbEntry != null)
            {
                context.Accesories.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
