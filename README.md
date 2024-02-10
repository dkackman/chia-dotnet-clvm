# chia-dotnet-clvm

[![.NET](https://github.com/dkackman/chia-dotnet-clvm/actions/workflows/dotnet.yml/badge.svg)](https://github.com/dkackman/chia-dotnet-clvm/actions/workflows/dotnet.yml)
[![CodeQL](https://github.com/dkackman/chia-dotnet-clvm/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/dkackman/chia-dotnet-clvm/actions/workflows/github-code-scanning/codeql)
[![NuGet Downloads](https://img.shields.io/nuget/dt/chia-dotnet-clvm)](https://www.nuget.org/packages/chia-dotnet-clvm/)

A direct port of [Rigidity](https://github.com/Rigidity)'s [node-clvm-lib](https://github.com/Chia-Network/node-clvm-lib).

## See Also

- [Documentation](https://dkackman.github.io/chia-dotnet-clvm/)
- [chia-dotnet](https://www.nuget.org/packages/chia-dotnet/)
- [chia-dotnet-bls](https://www.nuget.org/packages/chia-dotnet-bls/)
- [Chialisp](https://chialisp.com/)
- [chia-blockchain](https://chia.net)

## Examples

### Hello World

```csharp
var puzzleProgram = Program.FromSource("(q . \"hello world\")");
var result = puzzleProgram.Compile();

Console.WriteLine(result.Value);
Console.WriteLine(result.Cost);
```

___

_chia and its logo are the registered trademark or trademark of Chia Network, Inc. in the United States and worldwide._
