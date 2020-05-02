using CodeItAirLinesModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Repository
{
    public class LocalRepository : ILocalRepository
    {
        private string caminho = AppDomain.CurrentDomain.BaseDirectory + @"..\..\res\Locais.json";

        public List<Local> ListarLocais()
        {
            string json = File.ReadAllText(caminho);
            List<Local> locais = JsonConvert.DeserializeObject<List<Local>>(json);

            return locais;
        }

        public Local GetLocal(int id)
        {
            return ListarLocais().FirstOrDefault(x => x.Id == id);
        }
    }
}
