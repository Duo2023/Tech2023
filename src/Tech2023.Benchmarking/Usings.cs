global using BenchmarkDotNet.Running;
global using BenchmarkDotNet.Attributes;

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Performance", "CA1822:Mark members as static",
    Justification = "Benchmark assembly has mutliple methods that are static in implementation but cannot be static due to BenchmarkDotNet")]
