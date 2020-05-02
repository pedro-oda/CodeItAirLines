using CodeItAirLinesModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace CodeItRepository
{
    public class PessoaTestRepository : IPessoaRepository
    {
        private string caminho = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\..\CodeItAirLines\res\PessoasTest.json";

        public List<Pessoa> ListarPessoas()
        {
            string json = File.ReadAllText(caminho);
            List<Pessoa> pessoas = JsonConvert.DeserializeObject<List<Pessoa>>(json);

            return pessoas;
        }

    }
}
