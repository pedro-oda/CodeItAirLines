using CodeItAirLinesModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Repository
{
    public class CarroRepository : ICarroRepository
    {
        private string caminho = AppDomain.CurrentDomain.BaseDirectory + @"..\..\res\Carro.json";

        public List<Carro> ListarCarros()
        {
            string json = File.ReadAllText(caminho);
            List<Carro> carros = JsonConvert.DeserializeObject<List<Carro>>(json);

            return carros;
        }


        public Carro GetCarro(int id)
        {
            return ListarCarros().FirstOrDefault(x => x.Id == id);
        }
    }
}
