using Repository;

namespace CodeItRepository
{
    public class CarroFactory
    {
        public static ICarroRepository Create(bool isTest = false)
        {
            if (isTest)
                return new CarroTestRepository();

            return new CarroRepository();
        }
    }
}
