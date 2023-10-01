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

        Assert.True(builder.Capacity >= capacity, "Capacity should be equal or greater to supplied");
        Assert.NotNull(builder._arrayToReturnToPool);
        Assert.Equal(0, builder.Length);

        builder.Dispose();
    }

    [Theory(DisplayName = "Buffer Constructor")]
    [ClassData(typeof(NumericRangeGenerator))]
    public void Buffer_Constructor(int stackallocAmount)
    {
        var builder = new ValueStringBuilder(stackalloc char[stackallocAmount]);

        Assert.True(stackallocAmount <= 256, "Range should not be greater than 256 chars for tests");
        Assert.True(builder.Capacity >= stackallocAmount, "Capacity should be equal or greater to supplied");
        Assert.Null(builder._arrayToReturnToPool);
    }

    [Fact(DisplayName = "Append should resize buffer on grow")]
    public void Append_ShouldResize()
    {
        const int Capacity = 64;

        var builder = new ValueStringBuilder(stackalloc char[Capacity]);

        builder.Append(new string('*', Capacity));

        Assert.Equal(Capacity, builder.Capacity);

        builder.Append('*');

        Assert.True(builder.Capacity > Capacity, "The capacity of the builder should be greater than the original capacity");

        var result = builder.ToString();

        Assert.True(result.All(c => c == '*'));
        Assert.Equal(Capacity + 1, result.Length);
    }

    [Fact(DisplayName = "uint append should be valid")]
    public void Append_UIntValid()
    {
        var builder = new ValueStringBuilder(stackalloc char[32]);

        builder.Append(1024);

        Assert.Null(builder._arrayToReturnToPool);

        var result = builder.ToString();

        Assert.Equal("1024", result);
    }

    [Fact(DisplayName = "uint append should be valid on grow")]
    public void Append_UIntValid_ForGrow()
    {
        var builder = new ValueStringBuilder(stackalloc char[4]);

        uint value = 10245;

        builder.Append(value);

        Assert.NotNull(builder._arrayToReturnToPool);

        Assert.Equal("10245", builder.ToString());
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
