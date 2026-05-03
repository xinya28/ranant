using System.Security.Cryptography;

namespace ranant.RandomGenerators
{
    public class CryptoRandomGenerator : IRandomGenerator
    {
        private RandomNumberGenerator rng = RandomNumberGenerator.Create();

        public int Next()
        {
            byte[] bytes = new byte[4];
            rng.GetBytes(bytes);
            return System.Math.Abs(System.BitConverter.ToInt32(bytes, 0));
        }

        public int Next(int minValue, int maxValue)
        {
            return minValue + (Next() % (maxValue - minValue));
        }
    }
}