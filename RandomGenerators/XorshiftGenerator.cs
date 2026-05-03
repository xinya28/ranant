using System;

namespace ranant.RandomGenerators
{
    public class XorshiftGenerator : IRandomGenerator
    {
        private uint state;

        public XorshiftGenerator()
        {
            state = (uint)DateTime.Now.Ticks;
        }

        public int Next()
        {
            uint x = state;
            x ^= x << 13;
            x ^= x >> 17;
            x ^= x << 5;
            state = x;
            return (int)(x & 0x7FFFFFFF);
        }

        public int Next(int minValue, int maxValue)
        {
            return minValue + (int)((Next() / (double)int.MaxValue) * (maxValue - minValue));
        }
    }
}