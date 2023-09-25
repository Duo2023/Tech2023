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
