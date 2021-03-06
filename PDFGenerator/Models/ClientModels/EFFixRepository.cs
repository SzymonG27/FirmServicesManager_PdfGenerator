using PDFGenerator.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDFGenerator.Models.ClientModels
{
    public class EFFixRepository : IFixRepository
    {
        private ApplicationDbContext context;

        public EFFixRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }
        public IQueryable<Fix> Fixes => context.Fixes;

        public void SaveFix(Fix fix)
        {
            if (fix.ID == 0)
            {
                context.Fixes.Add(fix);
            }
            else
            {
                Fix dbEntry = context.Fixes
                    .FirstOrDefault(p => p.ID == fix.ID);
                if (dbEntry != null)
                {
                    dbEntry.Status = fix.Status;
                    dbEntry.DateOfRelease = fix.DateOfRelease;
                    dbEntry.ItemToFix = fix.ItemToFix;
                    dbEntry.WhatAccesory = fix.WhatAccesory;
                    dbEntry.WhatToFix = fix.WhatToFix;
                    dbEntry.CostOfRepair = fix.CostOfRepair;
                    dbEntry.PublicComments = fix.PublicComments;
                    dbEntry.PrivateComments = fix.PrivateComments;
                    dbEntry.PasswordIfExist = fix.PasswordIfExist;
                }
            }
            context.SaveChanges();
        }

        public Fix DeleteFix(int FixID)
        {
            Fix dbEntry = context.Fixes
                .FirstOrDefault(f => f.ID == FixID);
            if (dbEntry != null)
            {
                context.Fixes.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
