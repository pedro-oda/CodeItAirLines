using CodeItAirLinesModels;
using System.Collections.Generic;

namespace Repository
{
    public interface ICarroRepository
    {
        List<Carro> ListarCarros();
        Carro GetCarro(int id);
    }
}
