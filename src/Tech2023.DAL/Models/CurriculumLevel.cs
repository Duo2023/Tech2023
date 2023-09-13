using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

using Tech2023.Core;
using Tech2023.DAL.Models;

namespace Tech2023.DAL;

public enum CurriculumLevel : byte
{
    /// <summary>Either NCEA L1 or Cambridge IGSCE</summary>
    L1 = 1,

    /// <summary>Either NCEA L2 or Cambridge AS/A</summary>
    L2 = 2,

    /// <summary>Either NCEA L3 or Cambridge A2</summary>
    L3 = 3
}

public static class CurriculumLevelHelpers
{
    const string NceaSpecifier = "LEVEL";
    const string IGSCE = "IGSCE";

    // over optimized but was fun

    public static bool TryParse(ReadOnlySpan<char> input, out CurriculumLevel level, out CurriculumSource source)
    {
        // The lengths of the texts 'AS' and 'A2' are both 2, and since chars are UTF16 they are 2 bytes, (2*2)=sizeof(uint) so we can do a direct uint == uint comparison
        if (input.Length == 2) 
        {
            uint value = Unsafe.As<char, uint>(ref MemoryMarshal.GetReference(input));

            // the string is different on different platforms, because the endianness can be little endian or big endian
            // this check IsLittleEndian is treated as a const at runtime so it is a no cost operation
            if (value == (BitConverter.IsLittleEndian ? 0x53_00_41 : 0x41_00_53)) // AS
            {
                level = CurriculumLevel.L2;
                source = CurriculumSource.Cambridge;
                return true;
            }
            else if (value == (BitConverter.IsLittleEndian ? 0x32_00_41 : 0x41_00_32)) // A2
            {
                level = CurriculumLevel.L3;
                source = CurriculumSource.Cambridge;
                return true;
            }
        }

        // first check the length so that we dont have to pay the cost for IGSCE where it does a memcmp
        if (input.Length == NceaSpecifier.Length + 1 && input.StartsWith(NceaSpecifier))
        {
            var result = (CurriculumLevel)(byte)(input[NceaSpecifier.Length] - '0'); // fast than using byte.Parse because we are only using one character

            if (!Enum.IsDefined(result))
            {
                goto Failed;
            }

            level = result;
            source = CurriculumSource.Ncea;
            return true;
        }
        else if (input.SequenceEqual(IGSCE)) // we cannot use == because it compares T* & int not whether they are equal
        {
            level = CurriculumLevel.L1;
            source = CurriculumSource.Cambridge;
            return true;
        }

    Failed:
        level = default;
        source = default;
        return false;
    }

    [Pure]
    public static string ToString(CurriculumLevel level, CurriculumSource source)
    {
        Debug.Assert(Enum.IsDefined(source));
        Debug.Assert(Enum.IsDefined(level));

        if (source == CurriculumSource.Cambridge)
        {
            return level switch
            {
                CurriculumLevel.L1 => IGSCE,
                CurriculumLevel.L2 => "AS",
                CurriculumLevel.L3 => "A2",
                _ => string.Empty,
            };
        }
        else if (source == CurriculumSource.Ncea)
        {
            return $"{NceaSpecifier}{(byte)level}";
        }
        else return string.Empty;
    }
}
