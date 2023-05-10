using System.Collections;

namespace Tech2023.Core.Tests;

public class HexStringProvider : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        for (int i = 0; i < 100; i++)
        {
            uint local = (uint)Random.Shared.Next();

            yield return new object[] { local, $"#{local:x}" }; // slow way of doing it
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
