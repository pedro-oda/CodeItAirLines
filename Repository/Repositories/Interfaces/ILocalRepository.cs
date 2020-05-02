using CodeItAirLinesModels;
using System.Collections.Generic;

namespace Repository
{
    public interface ILocalRepository
    {
        List<Local> ListarLocais();
        Local GetLocal(int id);
    }
}
