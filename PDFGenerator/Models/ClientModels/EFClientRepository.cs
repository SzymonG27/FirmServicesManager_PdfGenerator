using PDFGenerator.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDFGenerator.Models.ClientModels
{
    public class EFClientRepository : IClientRepository
    {
        private ApplicationDbContext context;
        public EFClientRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }
        public IQueryable<Client> Clients => context.Clients;

        public void SaveClient(Client client)
        {
            var isInDB = context.Clients.FirstOrDefault(p => p.ID == client.ID);
            if (isInDB == null)
            {
                context.Clients.Add(client);
            }
            else
            {
                Client dbEntry = context.Clients
                    .FirstOrDefault(c => c.ID == client.ID);
                if (dbEntry != null)
                {
                    dbEntry.FirstName = client.FirstName;
                    dbEntry.SurName = client.SurName;
                    dbEntry.isFirm = client.isFirm;
                    dbEntry.PhoneNumber = client.PhoneNumber;
                }
            }
            context.SaveChanges();
        }
        public Client DeleteClient(string clientID)
        {
            Client dbEntry = context.Clients
                .FirstOrDefault(c => c.ID == clientID);
            if (dbEntry != null)
            {
                context.Clients.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;

        }
    }
}
