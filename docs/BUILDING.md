# Building on Windows

MKUltraBlurGlass is based on official 2.3.1 commit 828f09503193aaadf2a00edcbcbadbf21b39e057. The separate [pinned baseline procedure](BASELINE.md) builds the original baseline, not this fork.

Both fork x64 binaries were built successfully with VS2022, MSVC 14.44.35207 and Windows SDK 10.0.22621. Use 64-bit MSBuild, /m:1, /nr:false and PreferredToolArchitecture=x64. The projects use /Z7 and UTF-8 compilation. If incremental linking reports LNK1103, clean and rebuild the affected project.

## Requirements

- Visual Studio 2022 with Desktop development with C++, MSVC v143, and a Windows SDK.
- NuGet package restore support. Package versions are recorded in each project's `packages.config`.
- [MiaoUI Lite v2](https://github.com/Maplespe/MiaoUILite), built with compatible x64 settings. The GUI includes `Mui.h` and expects the `MUI_LITE_LIBRARY` MSBuild property to point to a directory containing `src/include` and `library`. Use commit 3537e787417463b72eba556be01e89070781096b. The corresponding source bundle includes this dependency. Build its Release x64 static library first.

## Compile

1. Open `DWMBlurGlass.sln` in Visual Studio 2022.
2. Restore NuGet packages for the solution.
3. Set `MUI_LITE_LIBRARY` to your MiaoUI Lite checkout/build location in the build environment or MSBuild properties. Restart Visual Studio after changing environment variables.
4. Select **Release | x64** and build the solution. The x64 project settings use C++20. Do not assume the legacy x86 configurations are supported.

From a Developer PowerShell with NuGet CLI and MSBuild available:

```powershell
nuget restore DWMBlurGlass.sln
msbuild DWMBlurGlass.sln /m:1 /nr:false /p:PreferredToolArchitecture=x64 /p:Configuration=Release /p:Platform=x64 /p:MUI_LITE_LIBRARY=C:\src\MiaoUILite\MiaoUI
```

Expected x64 output location: `Build/x64/Release/`. A missing NuGet import error means restore did not produce the expected `packages` directory.

## Runtime files

A successful compile is not a complete distribution. Keep `MKUltraBlurGlass.exe` and `MKUltraBlurGlassExt.dll` together. The GUI loads `data/defaultres.dmres` and `data/lang/*.xml`. The resource archive is checked in at `DWMBlurGlass/uiproject/DWMBlurGlass/defaultres.dmres`; translations are in `Languagefiles/`.

Compare packaging against a known working upstream release, including symbol helper DLLs and optional effect assets. Do not assume these instructions enumerate every runtime asset. The UI project JSON contains an upstream absolute resource path; adjust it locally if editing resources.

Build and package in a development folder. Test installation separately on a recoverable Windows environment before replacing your working copy.

## Fork checks and uninstaller

Run each tests/border-geometry, tests/titlebar-geometry and tests/color-context run-tests.py with the repository path as its argument. These isolated checks do not replace live compositor testing. Build the uninstaller with tools/uninstaller/build.ps1; see its README for checks. Include credits, license texts and corresponding source with distributions. Replace all runtime language XML files with Languagefiles/*.xml from this fork. Menu sound is embedded at build time.
