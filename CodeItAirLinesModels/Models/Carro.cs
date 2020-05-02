using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeItAirLinesModels
{
    public class Carro : GenericModel
    {
        public Carro()
        {
            Passageiros = new List<Pessoa>();
        }
        public string Modelo { get; set; }
        public int Capacidade { get; set; }
        public List<Pessoa> Passageiros { get; set; }
        public Pessoa Motorista { get; set; }
    }
}
