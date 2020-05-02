using System;

namespace CodeItAirLinesModels
{
    public class Pessoa : GenericModel
    {
        public string Nome { get; set; }
        public TipoTripulante TipoTripulante { get; set; }
        public TipoTripulante? Equipe { get; set; }
        public bool PodeDirigir { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (!(obj is Pessoa))
                return false;

            if (this.Id == ((Pessoa)obj).Id)
                return true;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
