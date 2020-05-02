using CodeItAirLinesInfra;
using CodeItAirLinesModels;
using CodeItRepository;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeItAirLinesBusiness
{
    public class TransporteService
    {
        private readonly IPessoaRepository _pessoaRepository;
        private readonly ILocalRepository _localRepository;
        private readonly ICarroRepository _carroRepository;
        private const int CARRO_PADRAO = 1;
        private const int AEROPORTO_ID = 1;
        private const int AVIAO_ID = 1;

        public TransporteService(IPessoaRepository pessoaRepository,
            ILocalRepository localRepository,
            ICarroRepository carroRepository)
        {
            _pessoaRepository = pessoaRepository;
            _localRepository = localRepository;
            _carroRepository = carroRepository;
        }

        public void IniciarTranporte()
        {

            List<Pessoa> pessoas = _pessoaRepository.ListarPessoas();
            Carro carro = _carroRepository.GetCarro(CARRO_PADRAO);
            Local aeroporto = _localRepository.GetLocal(AEROPORTO_ID);
            Local aviao = _localRepository.GetLocal(AVIAO_ID);

            Transportar(pessoas, carro, aeroporto, aviao);
        }

        public List<Pessoa> Transportar(List<Pessoa> pessoas, Carro carro, Local aeroporto, Local aviao)
        {
            bool ultimaViagemRealizada = false;

            Validar(pessoas, carro);

            aeroporto.Pessoas.AddRange(pessoas);

            var indiceMotorista = aeroporto.Pessoas.WhereIf(x => x.PodeDirigir && x.TipoTripulante != TipoTripulante.Policial, aeroporto.Pessoas.Count(y => y.TipoTripulante == TipoTripulante.Presidiario) > 1
                && aeroporto.Pessoas.Count(y => y.TipoTripulante == TipoTripulante.Policial) == 1)
                .Where(x => x.PodeDirigir).RandomElement();

            carro.Motorista = aeroporto.Pessoas.ElementAt(indiceMotorista);
            while (aeroporto.Pessoas.Count > carro.Capacidade - 1)
            {
                while (carro.Motorista != null && carro.Passageiros.Count < carro.Capacidade - 1)
                {
                    var indicePassageiro = RetornaIndicePassageiro(aeroporto.Pessoas, carro, aviao.Pessoas);

                    if (indicePassageiro < 0)
                        break;

                    if (carro.Motorista == null)
                        break;

                    ValidarTransferencia(aeroporto, carro, indicePassageiro);
                    aeroporto.Pessoas.ChangeList(carro.Passageiros, indicePassageiro);
                }

                if (carro.Passageiros.Count > 0)
                {
                    ultimaViagemRealizada = carro.Capacidade == 2 ? aeroporto.Pessoas.Count == carro.Capacidade - 1 : aeroporto.Pessoas.Count < carro.Capacidade - 1;
                    ValidarTransferencia(aviao, carro, null, true);

                    if (carro.Motorista == null)
                        break;

                    Console.WriteLine(string.Format(ultimaViagemRealizada ? Mensagens.LogUltimaViagem : Mensagens.LogTransporte, aeroporto.Nome, aviao.Nome, RetornarTripulante(carro.Motorista.TipoTripulante, carro.Motorista.Nome), RetornarPassageiros(carro.Passageiros)));

                    if (ultimaViagemRealizada)
                    {
                        aviao.Pessoas.Add(carro.Motorista);
                        aeroporto.Pessoas.Remove(carro.Motorista);
                    }

                    carro.Passageiros.ChangeList(aviao.Pessoas, null, true);
                }
            }

            if (!ultimaViagemRealizada)
            {
                aviao.Pessoas.Add(carro.Motorista);
                aeroporto.Pessoas.Remove(carro.Motorista);
                aeroporto.Pessoas.ChangeList(carro.Passageiros, null, true);
                ValidarTransferencia(aviao, carro, null, true);
                Console.WriteLine(string.Format(Mensagens.LogUltimaViagem, aeroporto.Nome, aviao.Nome, RetornarTripulante(carro.Motorista.TipoTripulante, carro.Motorista.Nome), RetornarPassageiros(carro.Passageiros)));
                carro.Passageiros.ChangeList(aviao.Pessoas, null, true);
            }

            carro.Motorista = null;

            return aviao.Pessoas;
        }

        private void ValidarTransferencia(Local local, Carro carro, int? indiceTripulante, bool desembarqueCarro = false)
        {
            if (desembarqueCarro)
            {
                if (local.Pessoas.Any(x => x.TipoTripulante == TipoTripulante.Oficial && local.Pessoas.Count == 2 && carro.Passageiros.Count == 2 && carro.Passageiros.Any(y => y.TipoTripulante == TipoTripulante.ChefeServico))
                    || local.Pessoas.Any(x => x.TipoTripulante == TipoTripulante.ChefeServico && local.Pessoas.Count == 12 && carro.Passageiros.Count == 2 && carro.Passageiros.Any(y => y.TipoTripulante == TipoTripulante.Oficial)))
                {
                    throw new RegraAcompanhanteException($"O {Enum.GetName(typeof(TipoTripulante), TipoTripulante.Oficial)} não pode ficar sozinho com { Enum.GetName(typeof(TipoTripulante), TipoTripulante.ChefeServico)}");
                }

                if (local.Pessoas.Any(x => x.TipoTripulante == TipoTripulante.Comissario && local.Pessoas.Count == 2 && carro.Passageiros.Count == 2 && carro.Passageiros.Any(y => y.TipoTripulante == TipoTripulante.Piloto))
                    || local.Pessoas.Any(x => x.TipoTripulante == TipoTripulante.Piloto && local.Pessoas.Count == 2 && carro.Passageiros.Count == 2 && carro.Passageiros.Any(y => y.TipoTripulante == TipoTripulante.Comissario)))
                {
                    throw new RegraAcompanhanteException($"O {Enum.GetName(typeof(TipoTripulante), TipoTripulante.Comissario)} não pode ficar sozinho com { Enum.GetName(typeof(TipoTripulante), TipoTripulante.Piloto)}");
                }

                if (local.Pessoas.Any(x => x.TipoTripulante == TipoTripulante.Presidiario && local.Pessoas.Count > 0 && carro.Passageiros.Count > 1 && carro.Passageiros.Any(y => y.TipoTripulante != TipoTripulante.Policial)))
                {
                    throw new RegraAcompanhanteException($"O {Enum.GetName(typeof(TipoTripulante), TipoTripulante.Presidiario)} não pode ficar com alguem sem a presença de um { Enum.GetName(typeof(TipoTripulante), TipoTripulante.Policial)}");
                }

            }
            else
            {
                if (carro.Motorista?.TipoTripulante == TipoTripulante.ChefeServico && carro.Capacidade == 2 && local.Pessoas.ElementAt(indiceTripulante.Value).TipoTripulante == TipoTripulante.Oficial
                    || local.Pessoas.Count == 2 && local.Pessoas.Any(y => y.TipoTripulante == TipoTripulante.ChefeServico && y.TipoTripulante == TipoTripulante.Oficial))
                {
                    throw new RegraAcompanhanteException($"O {Enum.GetName(typeof(TipoTripulante), TipoTripulante.Oficial)} não pode ficar sozinho com { Enum.GetName(typeof(TipoTripulante), TipoTripulante.ChefeServico)}");
                }

                if (carro.Motorista?.TipoTripulante == TipoTripulante.Piloto && carro.Capacidade == 2 && local.Pessoas.ElementAt(indiceTripulante.Value).TipoTripulante == TipoTripulante.Comissario
                    || local.Pessoas.Count == 2 && local.Pessoas.Any(y => y.TipoTripulante == TipoTripulante.Piloto && y.TipoTripulante == TipoTripulante.Comissario))
                {
                    throw new RegraAcompanhanteException($"O {Enum.GetName(typeof(TipoTripulante), TipoTripulante.Comissario)} não pode ficar sozinho com { Enum.GetName(typeof(TipoTripulante), TipoTripulante.Piloto)}");
                }

                if (carro.Motorista?.TipoTripulante != TipoTripulante.Policial && local.Pessoas.ElementAt(indiceTripulante.Value).TipoTripulante == TipoTripulante.Presidiario && !carro.Passageiros.Any(y => y.TipoTripulante == TipoTripulante.Policial)
                    || local.Pessoas.ElementAt(indiceTripulante.Value).TipoTripulante == TipoTripulante.Policial && local.Pessoas.Count(y => y.TipoTripulante == TipoTripulante.Policial) == 1 && local.Pessoas.Any(y => y.TipoTripulante == TipoTripulante.Presidiario))
                {
                    throw new RegraAcompanhanteException($"O {Enum.GetName(typeof(TipoTripulante), TipoTripulante.Presidiario)} não pode ficar com alguem sem a presença de um { Enum.GetName(typeof(TipoTripulante), TipoTripulante.Policial)}");
                }
            }
        }

        private void Validar(List<Pessoa> pessoas, Carro carro)
        {
            if (!pessoas.Any(x => x.TipoTripulante == TipoTripulante.Policial) && pessoas.Any(x => x.TipoTripulante == TipoTripulante.Presidiario))
            {
                throw new RegraAcompanhanteException($"O {Enum.GetName(typeof(TipoTripulante), TipoTripulante.Presidiario)} não pode ficar com alguem sem a presença de um { Enum.GetName(typeof(TipoTripulante), TipoTripulante.Policial)}");
            }

             if(carro == null)
            {
                throw new ErroInesperadoException($"Não existem carros para a realização do transporte");
            }
        }

        private string RetornarPassageiros(List<Pessoa> pessoas)
        {
            string tripulante = string.Empty;
            foreach (var passageiro in pessoas)
            {
                tripulante += RetornarTripulante(passageiro.TipoTripulante, passageiro.Nome);
            }
            return tripulante;
        }

        private string RetornarTripulante(TipoTripulante tipoTripulante, string nome)
        {
            return $" {Enum.GetName(typeof(TipoTripulante), tipoTripulante)}({nome}) ";
        }

        private int RetornaIndicePassageiro(List<Pessoa> pessoas, Carro carro, List<Pessoa> DestinoFinal)
        {
            var passageiro = default(List<Pessoa>);

            if (!passageiro.AnyOrNotNull())
            {
                passageiro = pessoas.Join(pessoas.Where(x => !x.PodeDirigir && x.TipoTripulante == carro.Motorista.Equipe), x => x, y => y, (x, y) => x).ToList();
            }

            if (!passageiro.AnyOrNotNull())
            {
                passageiro = pessoas.Join(pessoas.Where(x => x.PodeDirigir && carro.Motorista.Id != x.Id && (carro.Motorista.TipoTripulante == TipoTripulante.Policial
                     && carro.Passageiros.Any(z => z.TipoTripulante == TipoTripulante.Presidiario)
                    || (x.TipoTripulante != carro.Motorista.Equipe && x.TipoTripulante != TipoTripulante.Policial))),
                         x => x, y => y, (x, y) => x).ToList();

                if (passageiro.AnyOrNotNull())
                {
                    var NovoMotorista = passageiro.FirstOrDefault();
                    passageiro.Clear();
                    passageiro.Add(carro.Motorista);
                    carro.Motorista = NovoMotorista;
                }
                else
                {
                    passageiro = pessoas.Join(pessoas.Where(x => x.PodeDirigir && carro.Motorista.Id != x.Id && x.TipoTripulante != carro.Motorista.Equipe),
                         x => x, y => y, (x, y) => x).ToList();
                    if (passageiro.AnyOrNotNull())
                    {
                        var NovoMotorista = passageiro.FirstOrDefault();
                        passageiro.Clear();
                        passageiro.Add(carro.Motorista);
                        carro.Motorista = NovoMotorista;
                    }
                }
            }

            return pessoas.FindIndex(x => x == passageiro.FirstOrDefault());
        }

    }
}
