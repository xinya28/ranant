using System;

namespace ranant.RandomGenerators
{
    public class CSharpRandomGenerator : IRandomGenerator
    {
        private Random rand = new Random();
        public int Next() => rand.Next();
        public int Next(int minValue, int maxValue) => rand.Next(minValue, maxValue);
    }
}