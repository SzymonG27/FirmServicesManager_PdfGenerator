using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDFGenerator.Models.ClientModels
{
    public interface IFixRepository
    {
        IQueryable<Fix> Fixes { get; }
        void SaveFix(Fix fix);
        Fix DeleteFix(int FixID);
    }
}
