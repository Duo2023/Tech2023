using System.Collections;

namespace Tech2023.Core.Tests;

[Trait("Category", "ValueStringBuilder")]
public class ValueStringBuilderTests
{
    [Theory(DisplayName = "Capacity Constructor")]
    [ClassData(typeof(NumericRangeGenerator))]
    public void Capacity_Constructor(int capacity)
    {
        var builder = new ValueStringBuilder(capacity);

        builder.Capacity.Should().BeGreaterThanOrEqualTo(capacity, "Capacity should be equal or greater to supplied");

        builder._arrayToReturnToPool.Should().NotBeNull();

        builder.Length.Should().Be(0);

        builder.Dispose();
    }

    [Theory(DisplayName = "Buffer Constructor")]
    [ClassData(typeof(NumericRangeGenerator))]
    public void Buffer_Constructor(int stackallocAmount)
    {
        var builder = new ValueStringBuilder(stackalloc char[stackallocAmount]);

        stackallocAmount.Should().BeLessThanOrEqualTo(256);

        builder.Capacity.Should().BeGreaterThanOrEqualTo(stackallocAmount);

        builder._arrayToReturnToPool.Should().BeNull();
    }

    [Fact(DisplayName = "Append should resize buffer on grow")]
    public void Append_ShouldResize()
    {
        const int Capacity = 64;

        var builder = new ValueStringBuilder(stackalloc char[Capacity]);

        builder.Append(new string('*', Capacity));

        builder.Capacity.Should().Be(Capacity);

        builder.Append('*');

        builder.Capacity.Should().BeGreaterThan(Capacity, "The capacity should have expanded");

        var result = builder.ToString();

        result.All(c => c == '*').Should().BeTrue("All the characters should be the same");

        result.Length.Should().Be(Capacity + 1);
    }

    [Fact(DisplayName = "uint append should be valid")]
    public void Append_UIntValid()
    {
        var builder = new ValueStringBuilder(stackalloc char[32]);

        builder.Append(1024);

        builder._arrayToReturnToPool.Should().BeNull();

        var result = builder.ToString();

        result.Should().Be("1024");
    }

    [Fact(DisplayName = "uint append should be valid on grow")]
    public void Append_UIntValid_ForGrow()
    {
        var builder = new ValueStringBuilder(stackalloc char[4]);

        uint value = 10245;

        builder.Append(value);

        builder._arrayToReturnToPool.Should().NotBeNull();

        builder.ToString().Should().Be("10245");
    }
}


file class NumericRangeGenerator : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        foreach (var i in  Enumerable.Range(0, 256))
        {
            yield return new object[] { i };
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
