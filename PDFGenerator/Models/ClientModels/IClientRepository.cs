using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDFGenerator.Models.ClientModels
{
    public interface IClientRepository
    {
        public IQueryable<Client> Clients { get; }
        void SaveClient(Client client);
        Client DeleteClient(string ClientID);
    }
}
