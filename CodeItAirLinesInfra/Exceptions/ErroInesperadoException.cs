using System;

namespace CodeItAirLinesInfra
{
    public class ErroInesperadoException : Exception
    {
        public ErroInesperadoException()
        {

        }

        public ErroInesperadoException(string mensagemErro)
            : base(mensagemErro)
        {

        }

    }

}
