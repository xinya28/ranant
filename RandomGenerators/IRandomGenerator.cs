namespace ranant.RandomGenerators
{
    public interface IRandomGenerator
    {
        int Next();
        int Next(int minValue, int maxValue);
    }
}