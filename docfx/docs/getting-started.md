# Getting Started

## Installation

Install the [nuget package](https://www.nuget.org/packages/chia-dotnet-bls/)

```bash
dotnet add package chia-dotnet-bls
```

## Sign and Verify a Message

```csharp
using chia.dotnet.bls;
using dotnetstandard_bip39; // https://www.nuget.org/packages/dotnetstandard-bip39/

const string MNEMONIC = "abandon abandon abandon";
const string MESSAGE = "hello world";

// create a secret key from a mnemonic
var bip39 = new BIP39();
var seed = bip39.MnemonicToSeedHex(MNEMONIC, "");
var sk = PrivateKey.FromSeed(seed);

// sign the message
var signature = sk.Sign(MESSAGE);

// verify the signature
var pk = sk.GetG1();
var result = pk.Verify(MESSAGE, signature);

Console.WriteLine($"Signature is valid: {result}");
```
