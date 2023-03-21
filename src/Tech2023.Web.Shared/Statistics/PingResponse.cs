using System.Diagnostics.CodeAnalysis;

namespace Tech2023.Web.Shared.Statistics;

public readonly struct PingResponse : IEquatable<PingResponse>
{
    internal readonly TimeSpan _runtime;


    public PingResponse(TimeSpan runtime)
    {
        _runtime = runtime;
    }

    public TimeSpan Runtime
    {
        get => _runtime;
    }

    public bool Equals(PingResponse other)
    {
        return _runtime == other._runtime;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static PingResponse GetCurrentData()
    {
        return new(TimeSpan.FromTicks(Environment.TickCount64));
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

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }
}
