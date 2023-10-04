namespace Tech2023.Web.Extensions;

public static class RandomExtensions
{
    /// <summary>
    /// Executes the given action for a random amount of times
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void ForEachRandom(this Random random, int max, Action action)
    {
        foreach (var _ in Enumerable.Range(0, random.Next(1, max))) // guarantee at least one
        {
            action();
        }
    }
}
