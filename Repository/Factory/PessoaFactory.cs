namespace CodeItRepository
{
    public class PessoaFactory
    {
        public static IPessoaRepository Create(bool isTest = false)
        {
            if (isTest)
                return new PessoaTestRepository();

            return new PessoaRepository();
        }
    }
}
