using System;

namespace ranant.RandomGenerators
{
    public class LcGGenerator : IRandomGenerator
    {
        private uint state;

        public LcGGenerator()
        {
            state = (uint)DateTime.Now.Ticks;
        }

        public int Next()
        {
            state = 1103515245 * state + 12345;
            return (int)((state >> 16) & 0x7FFF);
        }

        public int Next(int minValue, int maxValue)
        {
            return minValue + (Next() % (maxValue - minValue));
        }
    }
}