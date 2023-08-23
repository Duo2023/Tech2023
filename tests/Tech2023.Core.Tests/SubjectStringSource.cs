using System.Collections;

namespace Tech2023.Core.Tests;

public class SubjectStringSource : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { "mathematics", "Mathematics" };
        yield return new object[] { "MATHEMATICS", "Mathematics" };
        yield return new object[] { "INFORMATION TECHNOLOGY", "Information Technology" };
        yield return new object[] { "SCIENCE", "Science" };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
