using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDFGenerator.Models.ClientModels
{
    public interface IClientFirmRelationRepository
    {
        IQueryable<ClientFirmRelation> ClientFirmRelations { get; }
        void SaveClientFirmRelation(ClientFirmRelation clientFirmRelation);
    }
}
