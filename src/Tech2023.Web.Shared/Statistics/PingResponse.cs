using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json.Serialization;
using Tech2023.Core.Json.Converters;

namespace Tech2023.Web.Shared.Statistics;

public readonly struct PingResponse : IEquatable<PingResponse>
{
    internal readonly TimeSpan _runtime;
    internal readonly string _ipAddress;

    public PingResponse(TimeSpan runtime, string ipAddress)
    {
        _runtime = runtime;
        _ipAddress = ipAddress;
    }

    [JsonPropertyName("runtime")]
    [JsonConverter(typeof(TimeSpanConverter))]
    public TimeSpan Runtime
    {
        get => _runtime;
    }

    [JsonPropertyName("ip")]
    public string Ip
    {
        get => _ipAddress;
    }

    public bool Equals(PingResponse other)
    {
        return _runtime == other._runtime;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static PingResponse GetCurrentData(IPAddress address)
    {
        return new(TimeSpan.FromTicks(Environment.TickCount64), address.ToString());
    }

    public override bool Equals([AllowNull] object obj)
    {
        return obj is PingResponse response && Equals(response);
    }

    public static bool operator ==(PingResponse left, PingResponse right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(PingResponse left, PingResponse right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Returns the hashcode of the ping response
    /// </summary>
    public override int GetHashCode()
    {
        return HashCode.Combine(_runtime, _ipAddress);
    }
}
