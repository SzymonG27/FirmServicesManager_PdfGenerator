using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDFGenerator.Models.ClientModels
{
    public interface IAccesoryRepository
    {
        IQueryable<Accesory> Accesories { get; }
        void SaveAccesory(Accesory accesory);
        Accesory DeleteAccesory(int AccesoryID);
    }
}
