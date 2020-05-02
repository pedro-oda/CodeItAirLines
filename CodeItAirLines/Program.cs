using CodeItAirLinesBusiness;
using CodeItRepository;
using System;

namespace CodeItAirLines
{
    class Program
    {
        static void Main(string[] args)
        {
            TransporteService transporteService = new TransporteService(PessoaFactory.Create(), LocalFactory.Create(), CarroFactory.Create());
            transporteService.IniciarTranporte();
            Console.ReadKey();
        }
    }
}
