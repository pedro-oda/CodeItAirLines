using CodeItAirLinesModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace CodeItRepository
{
    public class PessoaRepository : IPessoaRepository
    {
        private string caminho = AppDomain.CurrentDomain.BaseDirectory + @"..\..\res\Pessoas.json";

        public List<Pessoa> ListarPessoas()
        {
            string json = File.ReadAllText(caminho);
            List<Pessoa> pessoas = JsonConvert.DeserializeObject<List<Pessoa>>(json);

            return pessoas;
        }


    }
}
