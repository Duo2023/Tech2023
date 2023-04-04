using System.Collections;
using System.Text.Json;
using System.Text.Json.Serialization;
using Tech2023.Core.Json.Converters;

namespace Tech2023.Core.Tests.Json.Converters;

public class TimeSpanConverterTests
{
    [Theory]
    [ClassData(typeof(LongGenerator))]
    public void Serialize_And_DeserializeBack(long ticks)
    {
        TimeSpanTestModel model = new() { Duration = TimeSpan.FromTicks(ticks) };

        Assert.Equal(ticks, model.Duration.Ticks);

        var json = JsonSerializer.Serialize(model);

        Assert.NotNull(json);

        var ret = JsonSerializer.Deserialize<TimeSpanTestModel>(json);

        Assert.NotNull(ret);
        Assert.Equal(ticks, ret.Duration.Ticks);
    }
}

// this model is specific to the file and only used in tests
file class TimeSpanTestModel
{
    [JsonConverter(typeof(TimeSpanConverter))]
    [JsonPropertyName("duration")]
    public TimeSpan Duration { get; set; }
}

file class LongGenerator : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        for (int i = 0; i < 10; i++) // do at least 10 runs of this
        {
            yield return new object[] { Random.Shared.NextInt64() };
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
