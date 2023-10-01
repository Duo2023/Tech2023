namespace Tech2023.DAL;

/// <summary>
/// Provides methods for parsing cambridge resource and formatting
/// </summary>
public static class Cambridge
{
    public static string GetString(int paperNumber, Season season, Variant variant)
    {
        return $"{paperNumber}_{Enum.GetName(season)}_{Enum.GetName(variant)}";
    }

    [SkipLocalsInit]
    public static bool TryParseResource(ReadOnlySpan<char> str, out int paperNumber, out Season season, out Variant variant)
    {
        Unsafe.SkipInit(out paperNumber);
        Unsafe.SkipInit(out season);
        Unsafe.SkipInit(out variant);

        int i = 0;

        foreach (var item in str.Split("_"))
        {
            switch (i)
            {
                case 0:
                    if (!int.TryParse(item, out paperNumber))
                    {
                        goto default;
                    }
                    break;
                case 1:
                    if (!Enum.TryParse(item, ignoreCase: true, out season))
                    {
                        goto default;
                    }
                    break;
                case 2:
                    if (!Enum.TryParse(item, ignoreCase: true, out variant))
                    {
                        goto default;
                    }
                    break;
                default:
                    paperNumber = default;
                    season = default;
                    variant = default;

                    return false;
            }

            i++;
        }

        return true;
    }

    /// <summary>
    /// Splits the span by the given sentinel, removing empty segments.
    /// </summary>
    /// <param name="span">The span to split</param>
    /// <param name="sentinel">The sentinel to split the span on.</param>
    /// <returns>An enumerator over the span segments.</returns>
    public static StringSplitEnumerator Split(this ReadOnlySpan<char> span, ReadOnlySpan<char> sentinel) => new(span, sentinel);

    public ref struct StringSplitEnumerator
    {
        private readonly ReadOnlySpan<char> _sentinel;
        private ReadOnlySpan<char> _span;

        public StringSplitEnumerator(ReadOnlySpan<char> span, ReadOnlySpan<char> sentinel)
        {
            _span = span;
            _sentinel = sentinel;
        }

        public bool MoveNext()
        {
            if (_span.Length == 0)
            {
                return false;
            }

            var index = _span.IndexOf(_sentinel, StringComparison.Ordinal);
            if (index < 0)
            {
                Current = _span;
                _span = default;
            }
            else
            {
                Current = _span[..index];
                _span = _span[(index + 1)..];
            }

            if (Current.Length == 0)
            {
                return MoveNext();
            }

            return true;
        }

        public ReadOnlySpan<char> Current { readonly get; private set; }

        public readonly StringSplitEnumerator GetEnumerator() => this;
    }
}
