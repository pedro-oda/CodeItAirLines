using CodeItAirLinesModels;
using System.Collections.Generic;

namespace Repository
{
    public interface ICarroTestRepository
    {
        List<Carro> ListarCarros();
        Carro GetCarro(int id);
    }
}
