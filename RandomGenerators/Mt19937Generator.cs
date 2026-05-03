using System;

namespace ranant.RandomGenerators
{
    public class Mt19937Generator : IRandomGenerator
    {
        private MT19937 mt;
        public Mt19937Generator()
        {
            mt = new MT19937();
            mt.Init((uint)DateTime.Now.Ticks);
        }
        public int Next() => (int)mt.Next();
        public int Next(int minValue, int maxValue)
        {
            uint range = (uint)(maxValue - minValue);
            uint result = mt.Next() % range;
            return minValue + (int)result;
        }
    }

    public class MT19937
    {
        private const int W = 32;
        private const int N = 624;
        private const int M = 397;
        private const int R = 31;
        private const uint A = 0x9908B0DF;
        private const int U = 11;
        private const uint D = 0xFFFFFFFF;
        private const int S = 7;
        private const uint B = 0x9D2C5680;
        private const int T = 15;
        private const uint C = 0xEFC60000;
        private const int L = 18;
        private const uint F = 1812433253;

        private uint[] mt = new uint[N];
        private int index;
        private uint lowerMask;
        private uint upperMask;

        public MT19937()
        {
            lowerMask = (1U << R) - 1;
            upperMask = ~lowerMask;
        }

        public void Init(uint seed)
        {
            mt[0] = seed;
            for (int i = 1; i < N; i++)
            {
                uint prev = mt[i - 1];
                uint shifted = prev >> (W - 2);
                uint xor = prev ^ shifted;
                uint multiplied = xor * F;
                mt[i] = multiplied + (uint)i;
            }
            index = N;
        }

        public uint Next()
        {
            if (index >= N) Twist();
            uint y = mt[index++];
            y ^= (y >> U) & D;
            y ^= (y << S) & B;
            y ^= (y << T) & C;
            y ^= (y >> L);
            return y;
        }

        private void Twist()
        {
            for (int i = 0; i < N; i++)
            {
                uint x = (mt[i] & upperMask) + (mt[(i + 1) % N] & lowerMask);
                uint xA = x >> 1;
                if ((x & 1) != 0) xA ^= A;
                mt[i] = mt[(i + M) % N] ^ xA;
            }
            index = 0;
        }
    }
}