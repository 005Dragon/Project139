namespace Code.Battle.Core
{
    public interface IRandom
    {
        int Range(int minValue, int maxValue);

        float Range(float minValue, float maxValue);
    }
}