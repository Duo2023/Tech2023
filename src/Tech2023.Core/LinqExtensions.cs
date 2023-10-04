namespace Tech2023.Core;

/// <summary>
/// Provides extensions for Linq types
/// </summary>
public static class LinqExtensions
{
    // this method is used because we have multiple collections nested inside of each other, it is more readable if we have nested collections it reads better

    /// <summary>
    /// Performs the specified action over every element in the <see cref="IEnumerable{T}"/>
    /// </summary>
    /// <typeparam name="T">The type of elements in the enumerable</typeparam>
    /// <param name="enumerable">The enumerable</param>
    /// <param name="action">The action to invoke on the elements</param>
    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        ArgumentNullException.ThrowIfNull(enumerable);
        ArgumentNullException.ThrowIfNull(action);

        foreach (var item in enumerable)
        {
            action(item);
        }
    }
}
