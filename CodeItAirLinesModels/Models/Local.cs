using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeItAirLinesModels
{
    public class Local : GenericModel
    {
        public Local()
        {
            Pessoas = new List<Pessoa>();
        }
        public string Nome { get; set; }
        public List<Pessoa> Pessoas { get; set; }
    }
}
