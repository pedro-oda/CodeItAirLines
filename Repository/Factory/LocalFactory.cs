using Repository;

namespace CodeItRepository
{
    public class LocalFactory
    {
        public static ILocalRepository Create(bool isTest = false)
        {
            if (isTest)
                return new LocalTestRepository();

            return new LocalRepository();
        }
    }
}
