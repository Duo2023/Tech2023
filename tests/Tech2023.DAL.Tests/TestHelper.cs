using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Tech2023.DAL.Tests;

internal class TestHelper
{
    public static string GetInvalidMessage(string paramName) => $"The parameter '{paramName}' provided to the unit test is invalid";

    public static string GenerateString(int length)
    {
        const int MinRange = 'a';
        const int MaxRange = 'z';

        return string.Create(length, 0, (span, _) =>
        {
            for (int i = 0; i < span.Length; i++)
            {
                char character = (char)Random.Shared.Next(MinRange, MaxRange);

                Debug.Assert(char.IsLetter(character));

                span[i] = character;
            }
        });
    }
}
