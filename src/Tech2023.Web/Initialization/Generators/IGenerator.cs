namespace Tech2023.Web.Initialization.Generators;

/// <summary>
/// A generator used to generate elements of a specified type
/// </summary>
/// <typeparam name="T">The type of elements to generate</typeparam>
internal interface IGenerator<T>
{
    /// <summary>
    /// Generates an item
    /// </summary>
    /// <returns>The generated item</returns>
    T Generate();
}
