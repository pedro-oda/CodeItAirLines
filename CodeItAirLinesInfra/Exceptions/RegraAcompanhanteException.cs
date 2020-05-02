using System;

namespace CodeItAirLinesInfra
{
    public class RegraAcompanhanteException : Exception
    {
        public RegraAcompanhanteException()
        {

        }

        public RegraAcompanhanteException(string mensagemErro)
            : base(mensagemErro)
        {

        }

    }

}
