using PDFGenerator.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDFGenerator.Models.ClientModels
{
    public class EFClientFirmRelationRepository : IClientFirmRelationRepository
    {
        private ApplicationDbContext context;
        public EFClientFirmRelationRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }
        public IQueryable<ClientFirmRelation> ClientFirmRelations => context.ClientFirmRelations;

        public void SaveClientFirmRelation(ClientFirmRelation clientFirmRelation)
        {
            if (clientFirmRelation.ClientID == null && clientFirmRelation.FirmID == 0)
            {
                context.ClientFirmRelations.Add(clientFirmRelation);
            }
            else
            {
                ClientFirmRelation dbEntry = context.ClientFirmRelations
                    .FirstOrDefault(cfr => cfr.ClientID == clientFirmRelation.ClientID && 
                    cfr.FirmID == clientFirmRelation.FirmID);
                if (dbEntry != null)
                {
                    //Działa
                }
            }
            context.SaveChanges();
        }
    }
}
