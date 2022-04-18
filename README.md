# Voice100 sample for Godot

![screen shot](./screenshot.png)

This is a sample app of
[Voice100Sharp](https://github.com/kaiidams/Voice100Sharp).
Voice100Sharp is a tiny neural TTS/ASR runtime for C#.

## How to run

Make sure that [Git LFS](https://git-lfs.github.com/) is enabled.

```
git lfs install
```

Clone the repository

```
git https://github.com/kaiidams/Voice100Godot.git
```

Open the folder with
[Godot 3.4.4 Mono version](https://godotengine.org/download).

Run it if you run Windows 64-bit.

## Native library

This repository depends on NuGet packages. Voie100Sharp includes
a native library (`voice100_native.dll` for Windows and `libvoice100_native.so` for Linux).
You will have to extract `voice100_native.dll` or `libvoice100_native.so` for your platform
from NuGet package.

Download [voice100.native.0.3.0.nupkg](https://www.nuget.org/packages/Voice100.Native/)
and rename it `voice100.native.0.3.0.nupkg.zip` and extract it and find the library under
`runtime`.

Without library, you will see an error like this.

```
Unhandled Exception:                                                                                                    System.DllNotFoundException: voice100_native assembly:<unknown assembly> type:<unknown type> member:(null)                at (wrapper managed-to-native) Voice100.WORLDVocoder.Voice100Sharp_VocoderDecode(single[],single[],single[],int,int,double,int,single,int16[],int)                                                                                              at Voice100.WORLDVocoder.Decode (System.Single[] f0, System.Single[] logspc, System.Single[] codeap) [0x00000] in <edf55c53a3d04aca86e6f07037913091>:0                                                                                          at Voice100.SpeechSynthesizer.Predict (System.Int64[] aligned, System.Int64[]& outputText) [0x000b6] in <edf55c53a3d04aca86e6f07037913091>:0                                                                                                    at Voice100.SpeechSynthesizer.Speak (System.String text, System.Int16[]& audio, System.String[]& phonemes) [0x00028] in <edf55c53a3d04aca86e6f07037913091>:0                                                                                    at MyTestNode.OnClick () [0x00051] in MyTestNode.cs:45   
```