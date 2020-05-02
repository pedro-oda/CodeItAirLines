using CodeItAirLinesBusiness;
using CodeItAirLinesInfra;
using CodeItAirLinesModels;
using CodeItRepository;
using Repository;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CodeItAirlinesTest
{
    public class TransporteTest
    {

        [Fact]
        public void Ok()
        {

            const int CARRO_PADRAO = 1;
            const int AEROPORTO_ID = 1;
            const int AVIAO_ID = 1;

            IPessoaRepository _pessoaTestRepository = PessoaFactory.Create(true);
            ICarroRepository _carroTestRepository = CarroFactory.Create(true);
            ILocalRepository _localTestRepository = LocalFactory.Create(true);
            TransporteService transporteService = new TransporteService(_pessoaTestRepository, _localTestRepository, _carroTestRepository);

            List<Pessoa> pessoas = _pessoaTestRepository.ListarPessoas();
            Carro carro = _carroTestRepository.GetCarro(CARRO_PADRAO);
            Local aeroporto = _localTestRepository.GetLocal(AEROPORTO_ID);
            Local aviao = _localTestRepository.GetLocal(AVIAO_ID);

            List<Pessoa> pessoasNoAviao = transporteService.Transportar(pessoas, carro, aeroporto, aviao);

            Assert.Equal(pessoas.OrderBy(x => x.Id), pessoasNoAviao.OrderBy(x => x.Id));
        }

        [Fact]
        public void Error()
        {
            IPessoaRepository _pessoaTestRepository = PessoaFactory.Create(true);
            ICarroRepository _carroTestRepository = CarroFactory.Create(true);
            ILocalRepository _localTestRepository = LocalFactory.Create(true);
            TransporteService transporteService = new TransporteService(_pessoaTestRepository, _localTestRepository, _carroTestRepository);

            List<Pessoa> pessoas = _pessoaTestRepository.ListarPessoas();
            pessoas.Remove(pessoas.FirstOrDefault(x => x.TipoTripulante == TipoTripulante.Policial));
            Carro carro = _carroTestRepository.GetCarro(1);
            Local aeroporto = _localTestRepository.GetLocal(1);
            Local aviao = _localTestRepository.GetLocal(2);

            Assert.Throws<RegraAcompanhanteException>(() => transporteService.Transportar(pessoas, carro, aeroporto, aviao));
        }
    }
}
